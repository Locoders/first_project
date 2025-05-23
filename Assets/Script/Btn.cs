using UnityEngine;
using UnityEngine.Events;
public class Btn : MonoBehaviour
{
    public UnityEvent mainEvent;
    public UnityEvent secondEvent;
    public UnityEvent thirdEvent;
    void OnMouseUp()
    {
        mainEvent?.Invoke();
        secondEvent?.Invoke();
        thirdEvent?.Invoke();
    }
}
