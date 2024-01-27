using Cysharp.Threading.Tasks;
using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManagement.StageEvent;

public class GameRestart : MonoBehaviour
{
    EventManager eventManager;

    public void onRestart()
    {
        eventManager = FindObjectOfType<EventManager>();
        if (!(GameObject.FindWithTag("CurtainObject") == null))
            GameObject.FindWithTag("CurtainObject").GetComponent<Curtain>().CurtainEffect("CloseOpen", 0);
        else
            Debug.Log("커튼 오브젝트가 없음");

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //게임 진행 중 리스타트를 위한 부분
        if (sceneName.StartsWith("SceneStage"))
        {
            string stageInfo = sceneName.Substring("SceneStage".Length);
            Debug.Log("스테이지 정보: " + stageInfo);

            int stageNumber;
            if (int.TryParse(stageInfo.Split('_')[0], out stageNumber))
            {
                string reloadSceneName = "SceneStage" + stageInfo;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                //StartCoroutine(loadScene(reloadSceneName));
                StartCoroutine(LoadingScene(sceneName));
                //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("Failed to parse stage number.");
            }
        }
        else if (sceneName == "Tutorials2")
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
        else //게임오버씬에서 되돌아오기 위한 부분
        {
            string savedSceneName = PlayerPrefs.GetString("PlayingSceneName");

            if (!string.IsNullOrEmpty(savedSceneName))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                //StartCoroutine(loadScene(savedSceneName));
                StartCoroutine(LoadingScene(savedSceneName));
                //SceneManager.LoadScene(savedSceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("No suitable scene to reload.");
            }
        }

        IEnumerator LoadingScene(string SceneName)
        {
            yield return new WaitForSeconds(2.2f);
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            yield return null;
        }
    }

    IEnumerator loadScene(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = asyncOperation.progress * 100f;
            Debug.Log(string.Format("[{0}] reloading : {1}%", Time.time, Mathf.RoundToInt(progress)));
            yield return null;
        }
        Debug.Log(string.Format("[{0}] done", Time.time));
    }
}
