using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Btn_master : MonoBehaviour
{
    #region 설정 버튼
    //언어 설정
    List<string> LanguageList = new()
    {
        "English", "Korean", "Chinese", "Japanese"
    };
    int LanguageIndex = 0;
    //디스플레이 모드 설정
    List<string> DisplayModList = new()
    {
        "Windowed", "Fullscreen", "Borderless"
    };
    public int DisplayModIndex = 1;
    //해상도 설정
    List<string> ResolutionList = new()
    {
        "800X600", "1280X720", "1920X1080"
    };
    public int ResolutionIndex = 0;
    //프레임 제한
    List<string> FlameRateList = new()
    {
        "30", "60", "120", "144"
    };
    public int FlameRateIndex = 1;
    public int VSyncIndex = 0;

    #region 사운드 볼륨 설정
    public float volumeMaster = 1f;
    public float volumeBGM = 1f;
    public float volumeSFX = 1f;
    public float volumeUi = 1f;
    // 마스터 볼륨
    public void SliderMasterVolume(Transform i)
    {
        volumeMaster = float.Parse(ChangeInputField(i));
        i.parent.Find("InputField").GetComponent<InputField>().text = (float.Parse(volumeMaster.ToString("0.00")) * 100).ToString();
        AudioListener.volume = volumeMaster;
    }
    public void InputFieldMasterVolume(Transform i)
    {
        volumeMaster = float.Parse(ChangeInputField(i));
        i.parent.Find("Slider").GetComponent<Slider>().value = volumeMaster/100;
        AudioListener.volume = volumeMaster;
    }
    // BGM 볼륨
    public void SliderBGMVolume(Transform i)
    {
        volumeBGM = float.Parse(ChangeInputField(i));
        i.parent.Find("InputField").GetComponent<InputField>().text = (float.Parse(volumeBGM.ToString("0.00"))*100).ToString();
    }
    public void InputFieldBGMVolume(Transform i)
    {
        volumeBGM = float.Parse(ChangeInputField(i));
        i.parent.Find("Slider").GetComponent<Slider>().value = volumeBGM;
    }
    // SFX 볼륨
    public void SliderSFXVolume(Transform i)
    {
        volumeSFX = float.Parse(ChangeInputField(i));
        i.parent.Find("InputField").GetComponent<InputField>().text = (float.Parse(volumeSFX.ToString("0.00"))*100).ToString();
    }
    public void InputFieldSFXVolume(Transform i)
    {
        volumeSFX = float.Parse(ChangeInputField(i));
        i.parent.Find("Slider").GetComponent<Slider>().value = volumeSFX;
    }
    // UI 볼륨
    public void SliderUiVolume(Transform i)
    {
        volumeUi = float.Parse(ChangeInputField(i));
        i.parent.Find("InputField").GetComponent<InputField>().text = (float.Parse(volumeUi.ToString("0.00"))*100).ToString();
    }
    public void InputFieldUiVolume(Transform i)
    {
        volumeUi = float.Parse(ChangeInputField(i));
        i.parent.Find("Slider").GetComponent<Slider>().value = volumeUi;
    }
    #endregion
    ////////////////////
    /////Functions//////
    ////////////////////
    public void BtnChevron(Transform i)
    {
        Transform parent_ = i.parent.parent.parent;
        List<string> itemList;
        int index;
        if (parent_.name == "Language")
        {
            itemList = LanguageList;
            index = LanguageIndex;
            LanguageIndex = CheckLR(i, itemList, index);
        }
        else if (parent_.parent.name == "DisplayMod")
        {
            itemList = DisplayModList;
            index = DisplayModIndex;
            DisplayModIndex = CheckLR(i, itemList, index);
            switch (DisplayModList[DisplayModIndex])
            {
                case "Windowed":
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case "Fullscreen":
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case "Borderless":
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
        else if (parent_.name == "Resolution")
        {
            itemList = ResolutionList;
            index = ResolutionIndex;
            ResolutionIndex = CheckLR(i, itemList, index);
            string[] resolution = ResolutionList[ResolutionIndex].Split('X');
            int width = int.Parse(resolution[0]);
            int height = int.Parse(resolution[1]);
            Screen.SetResolution(width, height, Screen.fullScreenMode);
        }
        else if (parent_.name == "FlameRate")
        {
            itemList = FlameRateList;
            index = FlameRateIndex;
            FlameRateIndex = CheckLR(i, itemList, index);
            Application.targetFrameRate = int.Parse(FlameRateList[FlameRateIndex]);
        }
        
    }
    int CheckLR(Transform i, List<string> itemList = null, int index = 0)
    {
        // 왼쪽 버튼 클릭
        if (i.name[^1] == 'L')
        {
            if (0 < index) index--;
            else index = itemList.Count - 1;
        }
        // 오른쪽 버튼 클릭
        else if (i.name[^1] == 'R')
        {
            if (index < itemList.Count - 1) index++;
            else index = 0;
        }
        // 텍스트 업데이트
        i.parent.parent.Find("Text").GetComponent<Text>().text = itemList[index];
        return index;
    }
    public void BtnBool(Transform i)
    {
        Transform parent_ = i.parent.parent.parent;
        if (parent_.name == "DisplayVSync")
        {
            QualitySettings.vSyncCount = CheckOnOff(i) ? 1 : 0;
            VSyncIndex = CheckOnOff(i) ? 1 : 0;
        }
        else if(parent_.name == "test1")
        {

        }
    }
    bool CheckOnOff(Transform i)
    {
        if (i.name == "On")
        {
            return true;
        }
        else if (i.name == "off")
        {
            return false;
        }
        return false;
    }
    public string ChangeInputField(Transform i)
    {
        return i.GetComponent<InputField>().text;
    }
    #endregion
    #region 상점 버튼
    public Transform Strore;
    public void Select_module(Transform i)
    {
       Strore.GetComponent<Store>().selected = i;
       Strore.GetComponent<Store>().isMoving = true;
    }
    public void Next_round()
    {
        SceneControl.Scenechanger(SceneControl.GameMod);
    }
    #endregion  
    #region 에디트 모드 버튼
    public void EM_Next_Round()
    {
        SceneControl.Scenechanger(SceneControl.GameMod);
    }
    public void EM_Store()
    {
        SceneControl.Scenechanger(SceneControl.StoreMod);
    }
    public Transform EditMod;
    public void EM_select_item(Transform i)
    {
        EditMod.GetComponent<EditMod>().Selected_Object(i);
    }
    public void EM_select_list_item(Transform i)
    {
        i.GetChild(0).gameObject.SetActive(false);
        i.GetChild(1).gameObject.SetActive(true);
        foreach (Transform item in i.parent)
        {
            if(item != i)
            {
                item.GetChild(0).gameObject.SetActive(true);
                item.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    public void EM_select_category(Transform i)
    {
        i.gameObject.SetActive(true);
        foreach (Transform item in i.parent)
        {
            if(item != i)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
    public void EM_Chevron(Transform i)
    {

    }
    #endregion
    Color ChangeColor(float a,float b, float c, float d)
    {
        return new Color(a/255f, b/255f, c/255f, d/255f);
    }
}
