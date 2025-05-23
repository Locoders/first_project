using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public bool isClicked = false;

    void OnMouseDown()
    {
        isClicked = true; // 버튼이 눌렸음을 알림
    }
}
