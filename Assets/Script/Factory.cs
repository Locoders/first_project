using System.Collections.Generic;
using UnityEngine;
public class Factory : MonoBehaviour
{
    public Transform unit;
    public List<Transform> units;
    public int current_unit = 0;
    public int max_unit = 3;
    public float cooltime = 5f; 
    bool fuck = true;
    
    public float timer = 0f;
    void Start()
    {
        timer = Time.deltaTime;
    }
    void Update()
    {
        units.RemoveAll(item => item == null);
        current_unit = units.Count;
        if(God.GameState == "GameMod")
        {
            if(timer <= cooltime)
            {
                timer += Time.deltaTime;
            }
            if (timer > cooltime)
            {
                if(current_unit < max_unit && fuck)
                {
                    fuck = false;
                    transform.GetComponent<Animator>().SetTrigger("production");
                }
            }
        }
        if(God.GameState == "EditMod")
        {
            transform.GetComponent<Animator>().Play("Idle");
        }
    }
    public void Instunit()
    {
        Transform inst = Instantiate(unit,transform.position,transform.rotation);
        units.Add(inst);
        timer = 0f;
        fuck = true;
    }
}
