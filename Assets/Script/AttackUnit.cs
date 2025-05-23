using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class AttackUnit : MonoBehaviour
{
    public Transform core;
    public Transform target;
    public float range = 5;
    public float radius = 10f; // 원의 반지름
    public float lineLength = 10f; // 선의 길이
    public float angleA; // 엥글 주작은 뭐야 시발련
    public bool sensor1 = false; // 전방 센서
    public bool sensor2 = false; // 거리 센서
    public int sensor3 = 0; // 좌우 센서
    public bool sensor4 = false; // 사거리 센서
    public float sensor5 = 0; // 방해물 감지 센서
    public List<List<Transform>> boosters = new() {new() {}, new (){}, new() {}, new() {}, new() {}, new() {}};
    public List<Transform> neerlist = new();
    public List<Transform> group1 = new ();
    List<Transform> momentlist = new ();
    List<bool> itemlist = new ();
    void Update()
    {
        group1?.RemoveAll(item => item == null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Info>().tags.Contains("group1") && collider.transform != transform)
            {
                momentlist.Add(collider.transform);
            }
        }
        group1 = new List<Transform>(momentlist);
        momentlist?.Clear();
        neerlist = group1.OrderBy(t => Vector3.Distance(t.position, transform.position)).ToList();
        int cnt = 0;
        itemlist = new List<bool>(new bool[neerlist.Count]);
        foreach (var item in neerlist)
        {
            if(!item.GetComponent<Info>().tags.Contains("ship"))
            {
                itemlist[cnt] = false;
            }
            else
            {
                itemlist[cnt] = true;
            }
            cnt++;
        }
        if(!itemlist.Contains(false))
        {
            target = core;
        }
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(1, 1), 45, transform.up, 1);
        foreach(RaycastHit2D hit in hits)
        {
            //hit.transform.GetComponent<Info>().tags.Contains("group1")
            print(hit.transform.name + target.transform.name +(hit.transform == target.transform));
            if (hit.transform == target.transform)
            {
                print(hit.transform.name);
                foreach (Transform booster in boosters[0])
                {
                    booster.GetComponent<UnitBooster>().thrustSpeed = booster.GetComponent<UnitBooster>().thrustPower;
                    booster.GetComponent<UnitBooster>().UnitThrust();
                }
            }
            else
            {
                if(!sensor4)
                {
                    foreach (Transform booster in boosters[0])
                    {
                        booster.GetComponent<UnitBooster>().thrustSpeed = booster.GetComponent<UnitBooster>().thrustPower * (2*(Math.Abs(angleA)/180));
                        booster.GetComponent<UnitBooster>().UnitThrust();
                    }
                }
                if(sensor3 == 1)
                {
                    foreach (Transform booster in boosters[2])
                    {
                        booster.GetComponent<UnitBooster>().thrustSpeed = booster.GetComponent<UnitBooster>().thrustPower * (2*(Math.Abs(angleA)/180));
                        booster.GetComponent<UnitBooster>().UnitThrust();
                    }
                }
                if(sensor3 == -1)
                {
                    foreach (Transform booster in boosters[3])
                    {
                        booster.GetComponent<UnitBooster>().thrustSpeed = booster.GetComponent<UnitBooster>().thrustPower * (2*(Math.Abs(angleA)/180));
                        booster.GetComponent<UnitBooster>().UnitThrust();
                    }
                }
            }
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 1f); // 감지 방향
        Gizmos.DrawWireCube(transform.position + transform.up * 1f, new Vector2(1, 1));
        if(Vector2.Distance(core.position,transform.position) > radius)
        {
            target = core;
        }
        else
        {
            target = neerlist[0];
        }
        Vector3 toA = (target.transform.position - transform.position).normalized;
        float angleToA = -Mathf.Atan2(toA.y, toA.x) * Mathf.Rad2Deg;
        if(180 - transform.eulerAngles.z < (angleToA + 90))
        {
            angleA = angleToA - 270 + transform.eulerAngles.z;
        }
        else
        {
            angleA = angleToA + 90 + transform.eulerAngles.z;
        }
        // 중심 선
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + (transform.up * lineLength);
        Gizmos.DrawLine(startPoint, endPoint);
        float angleStep = 360f / 60;
        if(0 <= angleA && angleA <= 180)
        {
            sensor3 = 1;
        }
        else if(-180 <= angleA && angleA < 0)
        {
            sensor3 = -1;
        }
        // 전방 선 2개
        Gizmos.color = Color.blue;
        Vector3 catchPoint1 = startPoint + Anglec(6, radius);
        Vector3 catchPoint2 = startPoint + Anglec(-6, radius);
        Gizmos.DrawLine(startPoint, catchPoint1);
        Gizmos.DrawLine(startPoint, catchPoint2);
        if(-6 < angleA && angleA < 6)
        {
            sensor1 = true;
        }
        else
        {
            sensor1 = false;
        }
        // 사거리 원
        Gizmos.color = Color.white;
        Vector3 previousPoint1 = transform.position;
        for (int i = 0; i <= 60; i++)
        {
            float angle = i * angleStep;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * range, Mathf.Sin(Mathf.Deg2Rad * angle) * range, 0);
            if (i > 0) Gizmos.DrawLine(previousPoint1, point);
            previousPoint1 = point;
        }
        if(Vector2.Distance(target.position, transform.position) < range)
        {
            sensor4 = true;
        }
        else
        {
            sensor4 = false;
        }
        // 감지 범위 원 그리기
        Gizmos.color = Color.green;
        Vector3 previousPoint2 = transform.position;
        for (int i = 0; i <= 60; i++)
        {
            float angle = i * angleStep;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0);
            if (i > 0) Gizmos.DrawLine(previousPoint2, point);
            previousPoint2 = point;
        }
        if(Vector2.Distance(target.position, transform.position) < radius)
        {
            sensor2 = true;
        }
        else
        {
            sensor2 = false;
        }
    }
    Vector3 Anglec(float angle, float range)
    {
        return new Vector3(Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.z + 90 - angle)) * range, Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z+(90 - angle))) * range, 0);
    }
    // void Draw()
    // {
    //     // 선 그리기
    //     Vector3 forwardStartPoint = transform.up * lineLength;
    //     Vector3 forwardEndPoint = forwardStartPoint + transform.up * lineLength;
    //     frontline.SetPosition(0, Vector3.zero); // 선의 시작점 (중심)
    //     frontline.SetPosition(1, forwardStartPoint);
    //     frontline.SetPosition(2, forwardEndPoint);

    //     // 원 그리기
    //     float angleStep = 360f / segments;
    //     for (int i = 0; i <= segments; i++)
    //     {
    //         float angle = i * angleStep;
    //         float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
    //         float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
    //         circleline.SetPosition(i, new Vector3(x, y, 0));
    //     }
    // }
}
