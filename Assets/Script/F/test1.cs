using UnityEngine;
using System.Collections.Generic;

public class Test1 : MonoBehaviour
{
    public List<GameObject> collidingObjects = new(); // 충돌 중인 객체 리스트

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject); // 충돌한 객체 추가
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject); // 충돌이 끝난 객체 제거
        }
    }

    void Update()
    {
        // 현재 A 객체에 들어와 있는 모든 객체 출력
        foreach (GameObject obj in collidingObjects)
        {
            Debug.Log($"현재 A 객체에 있는 객체: {obj.name}");
        }
    }
}
