using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Launcher : MonoBehaviour
{
    private Transform _parent;
    private Unit _parent_Unit;
    public Transform bulletprefab = null; // 총알 프리팹
    private Transform bullet = null; // 소환된 총알
    public float attacktome;
    public float attackCooldown = 2f; // 쿨타임 2초
    public float B_speed = 10f;
    public int B_damage = 1;
    public float B_lifeTime = 2f;
    public bool B_homing = false;
    public bool spining = false;
    private bool firebool = false;
    private List<Transform> muzzles = new(){};
    public float timer = 0f; // 타이머
    string enemylayer;
    void Start()
    {
        foreach (Transform muzzle in transform)
        {
            muzzles.Add(muzzle); // 총구 위치
        }
        if(transform.parent.gameObject.layer == LayerMask.NameToLayer("friendly"))
        {
            enemylayer = "enemy";
        }
        else
        {
            enemylayer = "friendly";
        }
        _parent = transform.parent;
        _parent.GetComponent<Unit>().canons.Add(transform); // 총알 발사 위치를 리스트에 추가
        _parent_Unit = _parent.GetComponent<Unit>();
        StartCoroutine(UpdateRoutine());
    }
    void Update()
    {
        if (spining)
        {
            // 타겟을 바라보도록 회전 (일정한 속도로 방향으로 회전)
            Vector2 direction = (_parent_Unit.target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 20f);
        }
        if (firebool)
        {
            timer += Time.deltaTime;
            if(muzzles.Count == 1)
            {
                Fire(muzzles[0]);
            }
            else
            {
                foreach (Transform muzzle in muzzles)
                {
                    print(muzzle.name+ "발사");
                    Fire(muzzle); // 총구 위치에서 발사
                }
            }
        }
    }
    IEnumerator UpdateRoutine()
    {
        while (true)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, _parent_Unit.range, LayerMask.GetMask(enemylayer)); // 적과의 충돌 체크
            foreach (RaycastHit2D hit in hits)
            {
                // 적 태그를 가진 객체와 충돌 시
                if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer(enemylayer))
                {
                    firebool = true;
                    break;
                }
            }
            if(hits.Length == 0)//자신 객체를 봐서 1개임
            {
                firebool = false; // 사거리 안에 적이 없으면 발사 중지
            }
            yield return new WaitForSeconds(0.1f); // 0.1초마다 업데이트
        }
    }
    public void Fire(Transform Muzzle)
    {
        if (attackCooldown + attacktome < timer) // 사거리 안에 들어오면 발사
        {
            attacktome = 0;
            bullet = Instantiate(bulletprefab, Muzzle.transform.position, Muzzle.transform.rotation, GameGod.Projectiles); // 총알 생성
            bullet.gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(transform.parent.gameObject.layer) + "_projectile"); // 총알 레이어 설정
            Bullet bullet_Bullet = bullet.GetComponent<Bullet>();
            bullet_Bullet.target = _parent_Unit.target; // 총알 대상 설정
            bullet_Bullet.speed = B_speed; // 총알 속도 설정
            bullet_Bullet.damage = B_damage; // 총알 데미지 설정
            bullet_Bullet.lifeTime = B_lifeTime; // 총알 생존 시간 설정
            bullet_Bullet.homing = B_homing; // 총알 유도 여부 설정
            bullet_Bullet.parent_Unit = _parent; // 총알 부모 설정
            // 총알과 발사체의 충돌을 무시
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            Collider2D unitCollider = _parent.GetComponent<Collider2D>();
            if (bulletCollider != null && unitCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, unitCollider, true);
                Physics2D.IgnoreCollision(bulletCollider, _parent_Unit.core.GetComponent<Collider2D>(), true);
            }
            timer = 0; // 다음 공격 가능 시간 설정
        }
    }
}