
using UnityEngine;


public class tlqkf : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            transform.position = new Vector3(transform.position.x + 1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.position = new Vector3(transform.position.x - 1, 0, 0);
        }
    }
}
