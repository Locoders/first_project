using System.Collections.Generic;
using UnityEngine;

public class Recognition : MonoBehaviour
{
    public Transform Center;
    public Transform Up;
    public Transform Down;
    public Transform Left;
    public Transform Right;
    public List<Transform> check_transform = new() { null, null, null, null, null };

    void Mouse_checking()
    {
        Vector2 C = new(transform.localPosition.x, transform.localPosition.y);
        Vector2 U = new(transform.localPosition.x, transform.localPosition.y + 1);
        Vector2 D = new(transform.localPosition.x, transform.localPosition.y - 1);
        Vector2 L = new(transform.localPosition.x - 1, transform.localPosition.y);
        Vector2 R = new(transform.localPosition.x + 1, transform.localPosition.y);
        List<Vector2> check_position = new() { C, U, D, L, R };

        for (int i = 1; i <= 4; i++)
        {
            Collider2D collider = Physics2D.OverlapPoint(check_position[i]);
            if (collider != null)
            {
                check_transform[i] = collider.transform;
            }
            else
            {
                check_transform[i] = null;
            }
        }
        Vector2 bottomLeft = C - new Vector2(0.45f, 0.45f);
        Vector2 topRight = C + new Vector2(0.45f, 0.45f);
        Collider2D centerCollider = Physics2D.OverlapArea(bottomLeft, topRight);

        if (centerCollider != null)
        {
            check_transform[0] = centerCollider.transform;
        }
        else
        {
            check_transform[0] = null;
        }

        Center = check_transform[0];
        Up = check_transform[1];
        Down = check_transform[2];
        Left = check_transform[3];
        Right = check_transform[4];
    }

    void Update()
    {
        Mouse_checking();
    }
}
