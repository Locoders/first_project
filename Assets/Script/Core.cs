using UnityEngine;

public class Core : MonoBehaviour
{
    public float        Attack_range = 6;
    public float        Health = 100;
    void Awake()
    {
        GameGod._Core = gameObject;
    }
    void OnDrawGizmos()
    {
        /*
        // 객체의 위치와 회전을 반영하여 Gizmos로 시각화
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + transform.up * Attack_range;
        Gizmos.DrawLine(startPoint, endPoint);

        // 원 그리기
        Gizmos.color = Color.green;
        Vector3 previousPoint = transform.position;
        float angleStep = 360f / 50;
        for (int i = 0; i <= 50; i++)
        {
            float angle = i * angleStep;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * Attack_range, Mathf.Sin(Mathf.Deg2Rad * angle) * Attack_range, 0);
            if (i > 0) Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
        */
    }
}
