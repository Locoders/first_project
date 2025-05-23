using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public int money = 100;
    public Transform selected;
    public List<Transform> items = new();
    Vector2 currentVelocity = Vector2.zero; // 현재 속도 변수
    private float elapsedTime = 0f; // 경과 시간 변수
    private readonly float duration = 1f; // 작업 완료까지 걸리는 시간 (1초)
    public bool isMoving = false; // 이동 중인지 여부
    void Start()
    {
        foreach (Transform item in transform)
        {
            items.Add(item);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (selected && money >= selected.GetComponent<Module>().price)
            {
                money -= selected.GetComponent<Module>().price;
                Destroy(selected.gameObject);
                selected = null;
            }
        }
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;

            Vector2 targetPosition = new(0, 0); // 목표 위치
            selected.transform.position = Vector2.SmoothDamp(selected.transform.position, targetPosition, ref currentVelocity, 0.2f);

            Quaternion targetRotation = Quaternion.Euler(0, 0, 0); // 목표 회전
            selected.transform.rotation = Quaternion.RotateTowards(selected.transform.rotation, targetRotation, 250 * Time.deltaTime);

            Vector3 targetScale = new(2, 2, 2); // 목표 크기
            selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, targetScale, 0.1f);

            // 작업 완료 시간 초과 시 selected를 null로 설정
            if (elapsedTime >= duration)
            {
                isMoving = false; // 더 이상 Update에서 처리하지 않음
            }
        }
    }
}
