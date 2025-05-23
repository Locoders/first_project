using UnityEngine;
public class Edit_senser : MonoBehaviour
{
    public Animator anim;
    public Transform hit;
    void OnCollisionStay2D(Collision2D collision)
    {
        hit = collision.transform;
    }
}