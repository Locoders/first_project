using System.Collections.Generic;
using UnityEngine;

public class GameGod : MonoBehaviour
{
    public static GameObject _Core;
    public GameObject[] enemy_Prefabs;
    
    public Transform enemys;
    public Transform friendly_list;
    public Transform prjectile_list;
    public static Transform Projectiles;
    public int round = 1;
    public int max_enemy = 10;
    public int enemy_count;
    public float round_timer;
    
    public Transform station; // 정거장 오브젝트
    public Transform storeship; // 상점 우주선 오브젝트
    public float rotationSpeed = 45f; // 초당 회전 속도 (도 단위)
    int enemy_inst;
    float enemy_timer = 0.5f;
    float enemy_cooltime = 0.5f;
    Vector2 core_pos;
    List<Transform> enemy_list = new ();
    float storeshipx;
    
    float velocity = 0f; // 상점 우주선의 속도
    void Awake()
    {
        Projectiles = prjectile_list;
    }
    void OnEnable()
    {
        _Core.GetComponent<Movement>().enabled = true;
        _Core.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
    bool checkedtime_bool = false;
    public float checkedtime;
    void Update()
    {
        if(God.GameState == SceneControl.GameMod.name)
        {
            round_timer += Time.deltaTime;
            enemy_timer -= Time.deltaTime;

            enemy_list.RemoveAll(item => item == null);
            enemy_count = enemy_list.Count;
            
            if(max_enemy - enemy_inst + enemy_count <= 0)
            {
                if(!checkedtime_bool)
                {
                    RoundEnd();
                }
                else
                {
                    station.gameObject.SetActive(true);
                    storeship.gameObject.SetActive(true);
                    Station(station, storeship);
                }
            }
            if(enemy_inst < max_enemy)
            {
                EnemySpawn(enemy_cooltime);
            }
            
        }
    }
    public void RoundStart()
    {
        print("라운드 시작");
        round += 1;
    }
    void RoundEnd()
    {
        print("라운드 종료");
        Destroy(enemys.gameObject);
        Destroy(prjectile_list.gameObject);
        _Core.transform.position = new Vector2(0,0);
        _Core.GetComponent<Movement>().enabled = false;
        _Core.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _Core.GetComponent<Rigidbody2D>().inertia = 0;
        enemys = new GameObject("Enemys").transform;
        enemys.parent = transform;
        Checktime();
    }
    void Checktime()
    {
        if (!checkedtime_bool)
        {
            checkedtime = Time.time;
            checkedtime_bool = true;
        }
    }
    void EnemySpawn(float cooltime)
    {
        if(enemy_timer < 0)
        {
            core_pos = new Vector2(_Core.transform.localPosition.x, _Core.transform.localPosition.y);
            int ran_range = Random.Range(0, 360);
            GameObject enemy =
            Instantiate
            (
                enemy_Prefabs[Random.Range(0, enemy_Prefabs.Length)], 
                new Vector2(core_pos.x + Mathf.Cos(ran_range * Mathf.Deg2Rad) * 10 ,core_pos.y + Mathf.Sin(ran_range * Mathf.Deg2Rad) * 10), 
                Quaternion.identity,enemys
            );
            enemy_list.Add(enemy.transform);
            enemy_inst += 1;
            enemy_timer = cooltime;
        }
    }
    void Station(Transform station, Transform storeship)
    {
        if (Time.time - checkedtime < 4f)
        {
            // 정거장 크기
            float scale = Mathf.Lerp(0.3f, 1f, (Time.time - checkedtime) / 2f); 
            station.GetComponent<Transform>().localScale = new Vector2(scale, scale);
            // 정거장 투명도
            Color color = station.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(0f, 1f, (Time.time - checkedtime) / 2f); 
            station.GetComponent<SpriteRenderer>().color = color;
            station.GetChild(0).GetComponent<SpriteRenderer>().color = color;
            station.GetChild(1).GetComponent<SpriteRenderer>().color = color;
            // 상점 우주선위치
            if (2f < Time.time - checkedtime)
            {
                // 현재 x 위치를 업데이트
                storeshipx = storeship.position.x;

                // SmoothDamp를 사용하여 부드럽게 이동
                float newX = Mathf.SmoothDamp(storeshipx, -1.8125f, ref velocity, 2f/4);
                storeship.position = new Vector2(newX, storeship.position.y);
            }
        }
        else if (4f < Time.time - checkedtime && Time.time - checkedtime < 5f)
        {
            station.GetComponent<Transform>().localScale = new Vector2(1f, 1f);
            station.GetComponent<SpriteRenderer>().color = Color.white;
            station.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            station.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
            storeship.position = new Vector2(-1.8125f, storeship.position.y);
            storeship.GetComponent<Animator>().SetTrigger("open");
            
        }
        else if (5f < Time.time - checkedtime && Time.time - checkedtime < 7f)
        {
            Camera.main.GetComponent<Camera>().GetComponent<Following>().target = storeship.GetChild(0).gameObject;
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, 0.01f, ref velocity, 2f/4);
        }
        else
        {
            Camera.main.GetComponent<Camera>().GetComponent<Following>().enabled = false;
            Camera.main.orthographicSize = 5;
            Camera.main.transform.position = new Vector3(0, 0, -10);
            checkedtime = 0;
            checkedtime_bool = false;
            station.gameObject.SetActive(false);
            storeship.gameObject.SetActive(false);
            enemy_inst = 0;
            enemy_count = 0;
            SceneControl.Scenechanger(SceneControl.StoreMod);
        }
        // Z축을 기준으로 회전
        station.GetChild(0).Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}