using UnityEngine;
public class SceneControl : MonoBehaviour
{
    static string before = "GameMod";
    static GameObject beforescene;
    static GameObject afterscene;
    public static void Scenechanger(string after)
    {
        GameObject Scenes = GameObject.Find("Scenes");
        foreach (Transform child in Scenes.transform)
        {
            if(child.name == before)
            {
                beforescene = child.gameObject;
            }
        }
        foreach (Transform child in Scenes.transform)
        {
            if(child.name == after)
            {
                afterscene = child.gameObject;
            }
        }
        GameObject.FindWithTag("core").transform.parent = afterscene.transform;
        beforescene.gameObject.SetActive(false);
        afterscene.gameObject.SetActive(true);
        before = after;
    }
}
