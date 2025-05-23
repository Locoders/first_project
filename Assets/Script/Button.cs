using UnityEngine;
public class Button : MonoBehaviour
{
    public Transform God;
    public Transform Obj;
    EditMod EditMod;
    void Start()
    {
        EditMod = God.GetComponent<EditMod>();
    }
    public void Working()
    {
        EditMod.Selected_Object(Obj);
    }
}
