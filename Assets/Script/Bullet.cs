using System.Collections;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime;
    public bool homing;
    public Transform target;
    public Transform parent_Unit;
    public Transform Hit_Effect_Prefab; // 피격 이펙트 프리팹
    private Vector3 targetDirection;
    private float targetAngle;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer; // 라인 렌더러 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>(); // 라인 렌더러 컴포넌트 가져오기
        Destroy(gameObject, lifeTime);
    }
    void Update()
    {
        if(homing)
        {
            targetDirection = (target.position - transform.position).normalized;
            float Tangle = -Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            if(180 - transform.eulerAngles.z < (Tangle + 90))
            {
                targetAngle = Tangle - 270 + transform.eulerAngles.z;
            }
            else
            {
                targetAngle = Tangle + 90 + transform.eulerAngles.z;
            }
        }

    }
    void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed; // 속도를 직접 설정하여 즉시 빠르게 이동

        if (homing)
        {
            if (-180 <= targetAngle && targetAngle < 0)
            {
                float torque = speed * 0.1f; // 회전력 계산
                rb.AddTorque(torque); // 시계 반대 방향으로 회전
            }
            else if (0 <= targetAngle && targetAngle <= 180)
            {
                float torque = speed * 0.1f; // 회전력 계산
                rb.AddTorque(-torque); // 시계 방향으로 회전
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("enemy") || other.gameObject.layer == LayerMask.NameToLayer("friendly"))
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(damage); // 피해를 입힘
            Instantiate(Hit_Effect_Prefab, transform.position, Quaternion.identity, GameGod.Projectiles); // 피격 이펙트 생성
            Destroy(gameObject); // 총알 파괴
        }
    }
    
}