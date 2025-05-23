using UnityEngine;

public class Module : MonoBehaviour
{
    public int price;
    public string module_name;
    bool check = false;
    Vector2 velocity = Vector2.zero; // 이동 속도를 저장할 변수

    void OnMouseDown()
    {
        Selected();
        MoveCenter();
    }
    void Selected()
    {
        transform.parent.GetComponent<Store>().selected = transform;
    }
    void MoveCenter()
    {
        check = !check;
    }
}
