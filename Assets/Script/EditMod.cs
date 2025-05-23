using UnityEngine;
public class EditMod : MonoBehaviour
{
    public static Transform selected;
    public static Transform selectbody;
    public Transform God;
    public static Transform core;
    public Transform mouse;
    public Camera maincamera;
    public void Selected_Object(Transform A)
    {
        selected = A;
        
        if(!selectbody)
        {
            selectbody = Instantiate(selected);
            selectbody.name = selected.name;
            Keysetting.KeysettingSelectbody = selectbody;
            MonoBehaviour[] scripts = selectbody.GetComponents<MonoBehaviour>();
        
            // 모든 스크립트를 비활성화
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
            selectbody.gameObject.AddComponent<Recognition>();
            Destroy(selectbody.GetComponent<Rigidbody2D>());
            Destroy(selectbody.GetComponent<BoxCollider2D>());
            Destroy(selectbody.GetComponent<PolygonCollider2D>());
            for (int i = 0; i < selectbody.childCount; i++)
            {
                if (selectbody.GetChild(i))
                {
                    selectbody.gameObject.AddComponent<Recognition>();
                    Destroy(selectbody.GetComponent<Rigidbody2D>());
                    Destroy(selectbody.GetComponent<BoxCollider2D>());
                    Destroy(selectbody.GetComponent<PolygonCollider2D>());
                }
            }
        }
        else if(selected.name != selectbody.name)
        {
            Destroy(selectbody.gameObject);
            selectbody = Instantiate(selected);
            selectbody.name = selected.name;
            Keysetting.KeysettingSelectbody = selectbody;
            MonoBehaviour[] scripts = selectbody.GetComponents<MonoBehaviour>();
        
            // 모든 스크립트를 비활성화
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
            selectbody.gameObject.AddComponent<Recognition>();
            Destroy(selectbody.GetComponent<Rigidbody2D>());
            Destroy(selectbody.GetComponent<BoxCollider2D>());
            Destroy(selectbody.GetComponent<PolygonCollider2D>());
            for (int i = 0; i < selectbody.childCount; i++)
            {
                if (selectbody.GetChild(i))
                {
                    selectbody.gameObject.AddComponent<Recognition>();
                    Destroy(selectbody.GetComponent<Rigidbody2D>());
                    Destroy(selectbody.GetComponent<BoxCollider2D>());
                    Destroy(selectbody.GetComponent<PolygonCollider2D>());
                }
            }
        }
    }
}