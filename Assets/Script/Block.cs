using UnityEngine;
public class Block : MonoBehaviour
{
    void Update()
    {
        if(transform.GetComponent<FixedJoint2D>())
        {
            FixedJoint2D[] scripts = gameObject.GetComponents<FixedJoint2D>();
            foreach (var script in scripts)
            {
                if (script.connectedBody == null)
                {
                    Destroy(script);
                }
            }
        }
    }
}
