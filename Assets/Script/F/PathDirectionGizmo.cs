using UnityEngine;
using UnityEngine.AI;

public class PathDirectionGizmo : MonoBehaviour
{
    public Transform target; // 목표 지점
    private NavMeshAgent agent;
    private Vector3 nextDirection; // 다음 이동 방향
    public float directionAngle; // 막대 방향의 각도 (도 단위)

    [SerializeField] private float indicatorLength = 1.0f; // 방향 표시 길이

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // 자동 회전 비활성화 (NavMeshAgent의 불필요한 회전 방지)
        agent.updateRotation = false;
        agent.updateUpAxis = false; // ✅ NavMeshAgent가 XY 평면에서 동작하도록 변경
    }

    void Update()
    {
        UpdateDirection();
    }

    void UpdateDirection()
    {
        NavMeshPath path = new();
        if (agent.CalculatePath(target.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            if (path.corners.Length > 1)
            {
                Vector3 direction = (path.corners[1] - transform.position).normalized;
                nextDirection = direction * indicatorLength;

                // ✅ Z축이 아니라 XY 평면에서 방향 각도 계산
                float angleToT = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if(180 - transform.eulerAngles.z < (angleToT + 90))
                {
                    directionAngle = angleToT - 270 + transform.eulerAngles.z;
                }
                else
                {
                    directionAngle = angleToT + 90 + transform.eulerAngles.z;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + nextDirection);
        }
    }
}
