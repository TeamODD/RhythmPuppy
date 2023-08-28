using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void onRestart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName.StartsWith("SceneStage"))
        {
            string stageInfo = sceneName.Substring("SceneStage".Length);
            Debug.Log("스테이지 정보: " + stageInfo);

            int stageNumber;
            if (int.TryParse(stageInfo.Split('_')[0], out stageNumber))
            {
                string reloadSceneName = "SceneStage" + stageInfo;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                SceneManager.LoadSceneAsync(reloadSceneName);
                Debug.Log("씬 재로드: " + reloadSceneName);
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
                Time.fixedDeltaTime = 0.02f;
                SceneManager.LoadSceneAsync(savedSceneName);
                Debug.Log("저장된 씬 불러오기: " + savedSceneName);
            }
            else
            {
                Debug.LogWarning("No suitable scene to reload.");
            }
        }
    }
}
