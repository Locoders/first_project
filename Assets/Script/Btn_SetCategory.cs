using UnityEngine;
using UnityEngine.UI;
public class Btn_SetCategory : MonoBehaviour
{
    public Transform SettingMod;
    public GameObject Design_seleted;
    public GameObject Design_hover;
    public Text Text;
    void Start()
    {
        Design_seleted = transform.Find("Design_A").gameObject;
        Design_hover = transform.Find("Design_B").gameObject;
        Text = transform.Find("Text").GetComponent<Text>();
    }
    void OnMouseUp()
    {
        if(SettingMod.GetComponent<Setting>().Seleted != null)
        {
            SettingMod.GetComponent<Setting>().Seleted.Find("Design_A").gameObject.SetActive(false);
            SettingMod.GetComponent<Setting>().Seleted.Find("Text").GetComponent<Text>().color = ChangeColor(255, 255, 255, 255);
        }
        Design_hover.SetActive(false);
        Design_seleted.SetActive(true);
        Text.color = ChangeColor(229, 166, 21, 255);
        SettingMod.GetComponent<Setting>().ChangeCategory(transform);
    }
    void OnMouseOver()
    {
        if(!Design_seleted.activeSelf)
        {
            Design_hover.SetActive(true);
        }
    }
    void OnMouseExit()
    {
        if(Design_hover.activeSelf)
        {
            Design_hover.SetActive(false);
        }
    }
    Color ChangeColor(float a,float b, float c, float d)
    {
        return new Color(a/255f, b/255f, c/255f, d/255f);
    }
}