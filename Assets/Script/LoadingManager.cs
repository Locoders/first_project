using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private static string beforeScene;  // 이전 씬 이름 저장
    private static string nextScene;  // 이동할 씬 이름 저장

    // 씬을 로딩 씬으로 이동
    public static void LoadSceneAdditive(string newScene)
    {
        beforeScene = SceneManager.GetActiveScene().name;
        nextScene = newScene;  // 이동할 씬 설정
        
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive); // 로딩 씬으로 이동 (Additive)
    }

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        GameObject[] rootObjects = SceneManager.GetSceneByName(beforeScene).GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);  // 게임 씬 오브젝트들을 비활성화
        }
        // 로딩 씬 비동기 로드
        yield return new WaitForSeconds(2f);  //대기시간
        // 로딩 씬이 비동기 로딩 중
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // 로딩 진행 표시
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");

            if (operation.progress >= 0.9f)
            {
                // 로딩 완료 시 씬 활성화
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        // 새로운 씬 활성화
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
        GameObject[] nextSceneRootObjects = SceneManager.GetSceneByName(nextScene).GetRootGameObjects();
        foreach (GameObject obj in nextSceneRootObjects)
        {
            obj.SetActive(true);  // 새 씬의 오브젝트들을 활성화
        }
    }
}
