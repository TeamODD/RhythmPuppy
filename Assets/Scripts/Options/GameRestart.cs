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

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //커튼
        GameObject CurtainObject = GameObject.FindGameObjectWithTag("Curtain");
        CurtainObject.transform.SetParent(null, false); //worldpositionstays bool 인자 false로
        Curtain curtain = GameObject.FindGameObjectWithTag("Curtain").GetComponent<Curtain>();
        DontDestroyOnLoad(curtain);
        curtain.CurtainClose();

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
                /*StartCoroutine(loadScene(reloadSceneName));*/
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("Failed to parse stage number.");
            }
        }
        else
        {
            string savedSceneName = PlayerPrefs.GetString("PlayingSceneName");

            if (!string.IsNullOrEmpty(savedSceneName))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                /*StartCoroutine(loadScene(savedSceneName));*/
                SceneManager.LoadScene(savedSceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("No suitable scene to reload.");
            }
        }
    }

    IEnumerator loadScene(string sceneName)
    {
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
