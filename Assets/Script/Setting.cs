using System.Collections.Generic;
using UnityEngine;
public class Setting : MonoBehaviour
{
    public Transform Seleted;
    public List<Transform> SettingCategoryList;
    void Start()
    {
        foreach(Transform i in gameObject.transform.GetChild(0).GetChild(0))
        {
            SettingCategoryList.Add(i.Find("Item"));
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Settingclose();
        }
    }
    void Settingclose()
    {
        gameObject.SetActive(false);
    }
    public void ChangeCategory(Transform i)
    {
        Seleted = i;
        foreach(Transform j in SettingCategoryList)
        {
            if (j == i.Find("Item")) continue;
            j.gameObject.SetActive(false);
        }
        i.Find("Item").gameObject.SetActive(true);

    }
    /*
    1. 일반 (General)
        Language: 언어 설정.
        Subtitles: 자막 설정.
        Tutorial: 튜토리얼 켜기/끄기.
    2. 그래픽 또는 디스플레이 (Graphics or Display)
        Screen Shake: 화면 흔들림.
        Display Mode: 전체화면, 창 모드, 테두리 없는 창 모드.
        Resolution: 해상도.
        Frame Rate Limit: 프레임 제한.
        V-Sync: 수직 동기화.
        Motion Blur: 모션 블러.
    3. 소리 (Audio)
        Master Volume: 마스터 볼륨.
        Music Volume: 배경음악 볼륨.
        SFX Volume: 효과음 볼륨.
        UI Volume: UI 소리 볼륨.
        Dialogue Volume: 대사 볼륨.
        Ambient Volume: 환경음 볼륨.
    4. 조작 (Controls)
        Key Bindings: 키 설정.
        Move Forward: 앞으로 이동.
        Move Backward: 뒤로 이동.
        Turn Left: 왼쪽으로 회전.
        Turn Right: 오른쪽으로 회전.
        Rotate Part Left: 파츠를 왼쪽으로 회전.
        Rotate Part Right: 파츠를 오른쪽으로 회전.
        Toggle Mode: 모드 전환 (토글).
        Hold Mode: 유지 모드.
    5. 크레딧 (Credits)
        Developers: 제작자.
        Publisher: 제작사.
        Copyright: 저작권.
        Contact: 연락처.
        Website: 홈페이지.
        Social Media: SNS.
    */
}