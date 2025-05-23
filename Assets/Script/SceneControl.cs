using UnityEngine;
public class SceneControl : MonoBehaviour
{
    #region 씬
    [SerializeField] private GameObject EditModInstance;
    [SerializeField] private GameObject GameModInstance;
    [SerializeField] private GameObject StoreModInstance;
    [SerializeField] private GameObject SettingModInstance;
    
    public static GameObject EditMod;
    public static GameObject GameMod;
    public static GameObject StoreMod;
    public static GameObject settingMod;
    #endregion
    public static GameObject Curentscene;
    void Start()
    {
        // 인스펙터에서 설정한 값을 static 변수에 할당
        EditMod = EditModInstance;
        GameMod = GameModInstance;
        StoreMod = StoreModInstance;
        settingMod = SettingModInstance;
        // 씬을 지정
        Curentscene = GameMod;
    }
    
    public static void Scenechanger(GameObject selectedscene)
    {
        God.GameState = selectedscene.name;
        if(selectedscene == GameMod)
        {
            GameGod._Core.transform.parent = selectedscene.transform;
        }
        if(selectedscene == EditMod)
        {
            GameGod._Core.transform.parent = selectedscene.transform;
        }
        Curentscene.SetActive(false);
        selectedscene.SetActive(true);
        Curentscene = selectedscene;
    }
}
