using UnityEngine;
public class Keysetting : MonoBehaviour
{
    public bool keyN;
    public static Transform KeysettingSelectbody;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(keyN)
            {
                if(EditMod.selected)
                {
                    EditMod.selected = null;
                }
                if(EditMod.selectbody)
                {
                    Destroy(EditMod.selectbody.gameObject);
                }
                if(KeysettingSelectbody)
                {
                    Destroy(KeysettingSelectbody.gameObject);
                }
                SceneControl.Scenechanger("GameMod");
                God.GameState = "GameMod";
                keyN = !keyN;
            }
            else if (!keyN)
            {
                SceneControl.Scenechanger("EditMod");
                God.GameState = "EditMod";
                keyN = !keyN;
            }
        }
    }
}