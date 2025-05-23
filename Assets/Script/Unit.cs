using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public List<List<Transform>> boosters = new() { new() { }, new() { }, new() { }, new() { }, new() { }, new() { } };
    public List<Transform> canons = new();
    public List<Transform> neerlist = new();
    public List<Transform> group1 = new();
    public Transform core;
    public Transform target;
    public float Hp = 100;
    public float maxHp = 100;
    public float range = 5;
    public float o_radius = 10f;
    public float radius;
    public float rotatespeed = 100;
    public float thrustPower = 10;
    public bool s_range = false;
    public bool s_aim = false;
    public string enemy;
    private readonly List<Transform> momentlist = new();
    public Vector3 targetDirection;
    public float targetAngle;
    private Dictionary<Transform, UnitBooster> boosterComponents = new();
    void Awake()
    {
        core = GameGod._Core.transform;
    }
    void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("friendly"))
        {
            enemy = "enemy";
        }
        if(gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            enemy = "friendly";
        }
        // 캐싱
        foreach (var boosterList in boosters)
        {
            foreach (var booster in boosterList)
            {
                boosterComponents[booster] = booster.GetComponent<UnitBooster>();
            }
        }
        // 부스터 파워 설정
        foreach(var boosterList in boosters)
        {
            foreach(var booster in boosterList)
            {
                if(booster.GetComponent<UnitBooster>().thrustPower != thrustPower)
                {
                    booster.GetComponent<UnitBooster>().thrustPower = thrustPower;
                }
            }
        }
        StartCoroutine(CheckCollidersRoutine());
    }
    IEnumerator CheckCollidersRoutine()
    {
        while (true)
        {
            if(transform.gameObject.layer == LayerMask.NameToLayer("friendly"))
            {
                if(Vector2.Distance(core.position, transform.position) < o_radius)
                {
                    CheckColliders();
                    yield return new WaitForSeconds(0.25f);
                }
            }
            else if(transform.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                CheckColliders();
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
    void FixedUpdate()
    {
        foreach(var boosterList in boosters)
        {
            foreach(var booster in boosterList)
            {
                if(booster.GetComponent<UnitBooster>().thrustPower != thrustPower)
                {
                    booster.GetComponent<UnitBooster>().thrustPower = thrustPower;
                }
            }
        }
        // 타겟 지정
        Targeting();
        // 타겟 각도 계산
        if (ReferenceEquals(target, null))
        {
            target = null;
            CheckColliders();
        }
        else if(target != null)
        {
            targetDirection = (target.position - transform.position).normalized;
        }
       
        float Tangle = -Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        if (180 - transform.eulerAngles.z < (Tangle + 90))
        {
            targetAngle = Tangle - 270 + transform.eulerAngles.z;
        }
        else
        {
            targetAngle = Tangle + 90 + transform.eulerAngles.z;
        }
        if(50 < Mathf.Abs(targetAngle)) 
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - Mathf.Sign(targetAngle) * Time.deltaTime * rotatespeed);
        }
        else
        {
            if(0.05f < Mathf.Abs(targetAngle)) 
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - Mathf.Sign(targetAngle) * Time.deltaTime * Mathf.Sqrt(Mathf.Abs(targetAngle)/50) * rotatespeed);
            }
        }
        // 센서 업데이트
        if(target)
        {
            UpdateSensors();
        }
    }
    void UpdateSensors()
    {
        // 센서 4 무기 사정거리
        s_range = Vector2.Distance(target.position, transform.position) < range;
        // 센서 5 적이 정면
        s_aim = Mathf.Abs(targetAngle) < 5f;
        // 부스터 제어
        ControlBoosters();
    }

    void ControlBoosters()
    {
        if (s_range)
        {
            StopBooster(boosters[2]);
            StopBooster(boosters[3]);
        }
        else
        {
            if (s_aim)
            {
                SetBooster(0);
            }
        }
    }
    void Targeting()
    {
        if (Vector2.Distance(core.position, transform.position) > o_radius && transform.gameObject.layer == LayerMask.NameToLayer("friendly"))
        {
            target = core;
        }
        else
        {
            target = neerlist.Count > 0 ? neerlist[0] : core;
        }
    }
    void CheckColliders()
    {
        if (target != core)
        {
            radius = target != null ? Vector2.Distance(transform.position, target.position)+0.1f : o_radius;
        }
        else
        {
            radius = o_radius;
        }
        if(radius > o_radius)
        {
            radius = o_radius;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask(enemy));
        foreach (var collider in colliders)
        {
            if (collider.transform != transform && collider.gameObject.layer == LayerMask.NameToLayer(enemy))
            {
                momentlist.Add(collider.transform);
            }
        }
        group1 = new List<Transform>(momentlist);
        momentlist?.Clear();
        group1.RemoveAll(item => item == null);
        neerlist = group1.Count > 0 ? group1.OrderBy(t => Vector3.Distance(t.position, transform.position)).ToList() : new List<Transform>();
    }
    void StopBooster(List<Transform> boosters)
    {
        foreach (Transform booster in boosters)
        {
            var boosterComponent = boosterComponents[booster];
            boosterComponent.thrustSpeed = 0;
            boosterComponent.UnitThrust();
        }
    }
    void SetBooster(int N)
    {
        foreach (Transform booster in boosters[N])
        {
            var boosterComponent = boosterComponents[booster];
            boosterComponent.thrustSpeed = boosterComponent.thrustPower;
            boosterComponent.UnitThrust();
        }
    }
    public void TakeDamage(float damage)
    {
        //Timer();
        Hp -= damage; // 피해를 입힘
        Effect();
        if (Hp <= 0)
        {
            Destroy(gameObject); // 유닛 파괴
            //print(Time.time-starttime);
        }
    }
    void Effect()
    {
        StartCoroutine(ChangeColorCoroutine(transform.GetComponent<SpriteRenderer>(), 0.3f, Color.red, Color.white));
    }
    private IEnumerator ChangeColorCoroutine(SpriteRenderer spriteRenderer, float duration, Color startColor, Color targetColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        spriteRenderer.color = targetColor; // 최종 색상 설정
    }
    /*
    float starttime;
    bool testbool = true;
    void Timer()
    {
        if (testbool)
        {
            starttime = Time.time;
            testbool = false;
        }
    }
    */
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(core.position, o_radius);
    }
    */
}