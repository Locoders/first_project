using System.Collections.Generic;
using UnityEngine;
public class Boosters : MonoBehaviour
{
    public Rigidbody2D  rb; // 여기 바디
    public Movement     m;
    public int          Qx; // 제 x사분면
    public float        Rt;
    public float        thrustPower = 1f;
    
    List<int> Q;
    void Start () 
    {
        if(transform.tag != "booster")
        {
            m = transform.parent.GetComponent<Movement>();
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
        else
        {
            m = transform.parent.parent.GetComponent<Movement>();
            rb = transform.parent.parent.GetComponent<Rigidbody2D>();
        }

        Q = new(){1,0,0,1};
        if(transform.localPosition.x != 0 && transform.localPosition.y !=0)
        {
            if(transform.localPosition.x > 0 && transform.localPosition.y > 0)
            {
                Qx = 1;
            }
            else if(transform.localPosition.x < 0 && transform.localPosition.y > 0)
            {
                Qx = 2;
            }
            else if(transform.localPosition.x < 0 && transform.localPosition.y < 0)
            {
                Qx = 3;
            }
            else if(transform.localPosition.x > 0 && transform.localPosition.y < 0)
            {
                Qx = 4;
            }
            else
            {
                Qx = 0;
            }
        }
        Rt = transform.eulerAngles.z/90;
        if((int)Rt == 0)
        {
            m.Boosters[0].Add(transform);
        }
        else if ((int)Rt == 1)
        {
            m.Boosters[4].Add(transform);
        }
        else if ((int)Rt == 2)
        {
            m.Boosters[1].Add(transform);
        }
        else if ((int)Rt == 3)
        {
            m.Boosters[5].Add(transform);
        }
        if(Qx != 0)
        {
            List<int> temp = Q.GetRange(0, 4-Qx);
            Q.RemoveRange(0, 4-Qx);
            Q.AddRange(temp);
            if(Q[(int)Rt] == 0)
            {
                m.Boosters[2].Add(transform);
            }
            else
            {   
                m.Boosters[3].Add(transform);
            }
        }
    }    
    public void ApplyThrust(float power)
    {
        Vector2 force = transform.up * power;
        rb.AddForceAtPosition(force, transform.position);
    }
}