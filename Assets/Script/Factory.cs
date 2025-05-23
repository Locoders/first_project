using System.Collections.Generic;
using UnityEngine;
public class Factory : MonoBehaviour
{
    public List<Transform>  units;
    public Transform    unit;
    public Transform    Select;
    public int          current_unit = 0;
    public int          max_unit = 3;
    public float        cooltime = 5f; 
    private bool        fuck = true;
    
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
    void OnMouseDown()
    {
        F_UnitChoice();
    }
    void F_UnitChoice()
    {
        
    }

    // 선택된 옵션에 따라 Factory에 영향을 주는 메서드
    public void ApplyOption(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                print("Option 1 selected: 속성 A 변경");
                // 속성 A 변경 로직
                break;
            case 1:
                print("Option 2 selected: 속성 B 변경");
                // 속성 B 변경 로직
                break;
            case 2:
                print("Option 3 selected: 속성 C 변경");
                // 속성 C 변경 로직
                break;
        }
    }
}