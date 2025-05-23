using UnityEngine;
public class LineController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineLength = 5f;  // 선의 길이
    public Vector3 direction;  // WASD 입력에 따른 방향
    public int UD = 0;
    public int LR = 0;
    void Start()
    {
        // LineRenderer 초기화
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;  // 두 개의 점 설정 (시작점과 끝점)
        
        // 시작점은 객체의 중심 (즉, 객체의 위치)
        lineRenderer.SetPosition(0, transform.position);
    }
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        // WASD 입력 처리
        UD = Keybool("W","S");
        LR = Keybool("D","A");
        // 방향 벡터 설정
        direction = new Vector3(LR, UD, 0).normalized;

        // 끝점 설정: 객체 위치 + 방향 * 선의 길이
        Vector3 endPosition = transform.position + direction * lineLength;
        
        // 선의 끝점을 LineRenderer에 반영
        lineRenderer.SetPosition(1, endPosition);
    }
    int Keybool(string A, string B)
    {
        System.Enum.TryParse(A, true, out KeyCode keyA);
        System.Enum.TryParse(B, true, out KeyCode keyB);
        int _bool;
        if(Input.GetKey(keyA) && Input.GetKey(keyB))
        {
            _bool = 0;
        }
        else if(Input.GetKey(keyA))
        {
            _bool = 1;
        }
        else if (Input.GetKey(keyB))
        {
            _bool = -1;
        }
        else
        {
            _bool = 0;
        }
        return _bool;
    } 
}
