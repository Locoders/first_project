using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Mouse : MonoBehaviour
{
    public Transform EditGod;
    public Transform Parent;
    public Transform seleted;
    Recognition rec;
    Transform fuck;
    bool decided;
    List<Transform> rec_list;
    public bool install;
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = mousePosition;
        //selected 마우스 따라오게
        if(EditMod.selectbody)
        {
            rec = EditMod.selectbody.transform.GetComponent<Recognition>();
            rec_list = new(){rec.Up, rec.Left, rec.Down, rec.Right};
            EditMod.selectbody.transform.position = new Vector2(Mathf.Round(mousePosition.x),Mathf.Round(mousePosition.y));
            seleted = EditMod.selected;
            if(Input.GetKeyDown("r") && new[] {"booster","shield"}.Contains(seleted.tag))
            {
                if(EditMod.selectbody.transform.eulerAngles.z != -270)
                {
                    EditMod.selectbody.transform.rotation = Quaternion.Euler(0, 0,EditMod.selectbody.transform.eulerAngles.z - 90);
                }
                else
                {
                    EditMod.selectbody.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            if(Input.GetKeyDown("z") && new[] {"shield"}.Contains(seleted.tag))
            {
                if(EditMod.selectbody.transform.GetChild(0).localEulerAngles.y != 180)
                {
                    EditMod.selectbody.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    EditMod.selectbody.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            if ( new[] {"shield"}.Contains(seleted.tag))
            {
                rec_list = new(){rec.Down, rec.Right, rec.Up, rec.Left};
            }
            if(rec_list[(int)EditMod.selectbody.transform.localEulerAngles.z / 90])
                {
                    if(new[] {"block", "core"}.Contains(rec_list[(int)EditMod.selectbody.transform.localEulerAngles.z / 90].tag))
                    {
                        install = true;
                    }
                } 
            else
            {
                install = false;
            }
            if(EditMod.selectbody.tag != "booster" && EditMod.selectbody.tag != "shield")
            {
                install = true;
            }
        }
        if(Input.GetMouseButtonDown(0) && EditMod.selected != null && install && !EditMod.selected.CompareTag("Untagged"))
        {
            rec = EditMod.selectbody.transform.GetComponent<Recognition>();
            rec_list = new(){rec.Up, rec.Down, rec.Left, rec.Right};
            seleted = EditMod.selected;
            if
            (
                rec.Center != null &&
                !rec.Center.CompareTag("Untagged") && 
                new[] {"turret", "factory"}.Contains(seleted.tag) && 
                new[] {"block", "core"}.Contains(rec.Center.tag)
                
            )
            {
                fuck = Instantiate(seleted, rec.Center.localPosition ,Quaternion.Euler(0,0,0),rec.Center);
                decided = true;
                if(decided)
                {
                    //연결 애니메이션 작동
                    fuck.GetComponent<Animator>().SetBool("conection",true);
                }
            }
            if (rec_list.Count(obj => obj == null) != 4)
            {
                if(seleted.CompareTag("block") && rec.Center == null)
                {
                    List<Vector3> positions = new(){new Vector2(0,-1),new Vector2(0,1),new Vector2(1,0),new Vector2(-1,0)};
                    for (int i = 0; i < 4; i++)
                    {
                        if (rec_list[i] != null && rec_list[i].tag == "core")
                        {
                            fuck = Instantiate(seleted, Vector3.zero ,Quaternion.Euler(rec_list[i].rotation.eulerAngles),Parent);
                            fuck.localPosition = positions[i];
                            decided = true;
                            break;
                        }
                        else if(rec_list[i] != null && rec_list[i].tag != "shield")
                        {
                            fuck = Instantiate(seleted, Vector3.zero ,Quaternion.Euler(rec_list[i].rotation.eulerAngles),Parent);
                            fuck.localPosition = rec_list[i].transform.localPosition + positions[i];
                            decided = true;
                            break;
                        }
                    }
                    if(decided)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (rec_list[i] != null && (rec_list[i].tag == "core" || rec_list[i].tag == "block"))
                            {
                                //조인트 연결
                                rec_list[i].AddComponent<HingeJoint2D>().connectedBody = fuck.GetComponent<Rigidbody2D>();
                                fuck.AddComponent<HingeJoint2D>().connectedBody = rec_list[i].GetComponent<Rigidbody2D>();

                                //연결 애니메이션 작동
                                fuck.GetChild(i).GetComponent<Animator>().SetBool("conection",true);
                                int j;
                                rec_list[i].GetChild(j = (i % 2 == 0) ? i + 1 : i - 1).GetComponent<Animator>().SetBool("conection",true);
                            }
                        }
                        decided = false;
                    }
                }
                if(!seleted.CompareTag("Untagged") && new[] {"booster", "shield"}.Contains(seleted.tag) && rec.Center == null)
                {
                    
                    List<Vector3> positions = new(){new Vector2(0,-1),new Vector2(0,1),new Vector2(1,0),new Vector2(-1,0)};
                    for (int i = 0; i < 4; i++)
                    {
                        if (rec_list[i] != null && rec_list[i].tag == "core")
                        {
                            fuck = Instantiate(seleted, Vector3.zero ,Quaternion.Euler(0,0,0),Parent);
                            fuck.localPosition = positions[i];
                            fuck.localRotation = EditMod.selectbody.transform.localRotation;
                            decided = true;
                            break;
                        }
                        else if(rec_list[i] != null && !new[] {"booster", "shield"}.Contains(rec_list[i].tag))
                        {
                            fuck = Instantiate(seleted, Vector3.zero ,Quaternion.Euler(0,0,0),Parent);
                            fuck.localPosition = rec_list[i].transform.localPosition + positions[i];
                            fuck.localRotation = EditMod.selectbody.transform.localRotation;
                            decided = true;
                            break;
                        }
                    }
                    if(decided)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (rec_list[i] != null && (rec_list[i].tag == "core" || rec_list[i].tag == "block"))
                            {
                                //조인트 연결
                                rec_list[i].AddComponent<HingeJoint2D>().connectedBody = fuck.GetComponent<Rigidbody2D>();
                                fuck.AddComponent<HingeJoint2D>().connectedBody = rec_list[i].GetComponent<Rigidbody2D>();
                            }
                        }
                        decided = false;
                    }
                }
            }
        }
    }
}