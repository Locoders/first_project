using UnityEngine;
public class Following : MonoBehaviour
{
    public GameObject target;
    public float speed = 0.3f;
    private Vector2 velocity = Vector2.zero;
    void Update()
    {
        if (target != null)
        {
            Vector2 targetPosition = target.transform.position;
            Vector2 currentPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, speed);
            transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
        }
    }
}