using System.Collections.Generic;
using UnityEngine;

public class Keysetting : MonoBehaviour
{
    public bool keyN;
    public static bool K_Delete, K_Rotate;
    public Transform core;
    public Transform editmod;
    List<string> keylist = new() { "X", "Z"};
    void Update()
    {
        // 예: "X" 키를 KeyCode로 변환하여 입력 처리
        K_Delete = InputKey(StringToKeyCode(keylist[0]));

        // 예: 키패드의 숫자 1 처리
        K_Rotate = InputKey(StringToKeyCode(keylist[1]));
    }

    bool InputKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            return true;
        }
        return false;
    }

    public static KeyCode StringToKeyCode(string keyString)
    {
        try
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString, true); // 대소문자 구분 없이 변환
        }
        catch
        {
            Debug.LogError($"'{keyString}'는 유효한 KeyCode가 아닙니다.");
            return KeyCode.None; // 변환 실패 시 기본값 반환
        }
    }
}