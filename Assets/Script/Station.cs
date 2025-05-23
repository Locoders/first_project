using UnityEngine;

public class Station : MonoBehaviour
{
    public float rotationSpeed = 45f; // 초당 회전 속도 (도 단위)
    float startime = 0f; // 시작 시간
    public GameObject station; // 정거장 오브젝트
    public GameObject ship; // 상점 우주선 오브젝트
    void OnEnable()
    {
        startime = Time.time; // 시작 시간 기록
    }
    // Update는 매 프레임 호출됩니다.
    void Update()
    {
        // 크기를 1초 동안 점점 원래대로 복원
        if (Time.time - startime < 2f)
        {
            // 정거장 크기
            float scale = Mathf.Lerp(0.3f, 1f, (Time.time - startime) / 2f); 
            transform.GetComponent<Transform>().localScale = new Vector2(scale, scale);
            // 정거장 투명도
            Color color = transform.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(0f, 1f, (Time.time - startime) / 2f); 
            transform.GetComponent<SpriteRenderer>().color = color;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
            // 상점 우주선위치
            transform.GetChild(2).position = 
            new Vector2
            (
                Mathf.Lerp(transform.GetChild(2).position.x, 0, (Time.time - startime) / 2f),
                transform.GetChild(2).position.y
            ); 
        }
        // Z축을 기준으로 회전
        transform.GetChild(0).Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}