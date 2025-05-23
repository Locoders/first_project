using System.Collections.Generic;
using UnityEngine;
public class UnitBooster : MonoBehaviour
{
    public Rigidbody2D rb;
    public Unit B;
    public int Qx; // 제 x사분면
    public float Rt;
    public float thrustPower = 1f;
    public float thrustSpeed = 0;
    List<int> Q;
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        B = transform.parent.GetComponent<Unit>();
        
        Q = new() { 1, 0, 0, 1 };
        if (transform.localPosition.x != 0 && transform.localPosition.y != 0)
        {
            if (transform.localPosition.x > 0 && transform.localPosition.y > 0)
            {
                Qx = 1;
            }
            else if (transform.localPosition.x < 0 && transform.localPosition.y > 0)
            {
                Qx = 2;
            }
            else if (transform.localPosition.x < 0 && transform.localPosition.y < 0)
            {
                Qx = 3;
            }
            else if (transform.localPosition.x > 0 && transform.localPosition.y < 0)
            {
                Qx = 4;
            }
            else
            {
                Qx = 0;
            }
        }
        Rt = transform.localEulerAngles.z / 90;
        if (B.boosters != null)
        {
            if ((int)Rt == 0)
            {
                B.boosters[0].Add(transform);
            }
            else if ((int)Rt == 1)
            {
                B.boosters[4].Add(transform);
            }
            else if ((int)Rt == 2)
            {
                B.boosters[1].Add(transform);
            }
            else if ((int)Rt == 3)
            {
                B.boosters[5].Add(transform);
            }
            if (Qx != 0)
            {
                List<int> temp = Q.GetRange(0, 4 - Qx);
                Q.RemoveRange(0, 4 - Qx);
                Q.AddRange(temp);
                if (Q[(int)Rt] == 0)
                {
                    B.boosters[2].Add(transform);
                }
                else
                {
                    B.boosters[3].Add(transform);
                }
                if ((int)Rt == 0 && Qx == 3)
                {
                    B.boosters[4].Add(transform);
                }
                if ((int)Rt == 0 && Qx == 4)
                {
                    B.boosters[5].Add(transform);
                }
            }
        }
    }
    public void UnitThrust()
    {
        Vector2 force = transform.up * thrustSpeed;
        rb.AddForceAtPosition(force, transform.position);
    }
}