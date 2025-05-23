using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    public new Camera camera;
    public float zoomSpeed = 5f;  // 마우스 휠 스크롤에 따른 줌 속도
    public float minZoom = 2f;    // 최대 줌 인 값
    public float maxZoom = 10f;   // 최대 줌 아웃 값
    List<string> movement_keys;
    public List<List<Transform>> Boosters = new(){new(){},new(){},new(){},new(){},new(){},new(){}};
    void Start()
    {
        movement_keys = new List<string>(){"w","s","d","a","q","e"};
    }
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // 카메라 크기 조절
        camera.orthographicSize -= scroll * zoomSpeed;
        // 줌 한계를 설정 (minZoom과 maxZoom 사이로 제한)
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        if(God.GameState == "GameMod")
        {
            Keyfuck();
        }
    }
    void Keyfuck()
    {
        foreach(string i in movement_keys)
        {
            if (Input.GetKey(Keytrans(i)))
            {
                int k = movement_keys.IndexOf(i);
                foreach(Transform t in Boosters[k])
                {
                    t.GetComponent<Boosters>().ApplyThrust(t.GetComponent<Boosters>().thrustPower);
                }
            }
        }
    }
    KeyCode Keytrans(string A)
    {
        System.Enum.TryParse(A, true, out KeyCode key);
        return key;
    }
}
