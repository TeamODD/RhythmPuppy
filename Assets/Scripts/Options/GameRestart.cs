using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void onRestart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.StartsWith("SceneStage"))
        {
            int stageNumber;
            if (int.TryParse(currentSceneName.Substring("SceneStage".Length), out stageNumber))
            {
                // 스테이지 숫자를 파싱하여 씬을 다시 불러오는 처리
                SceneManager.LoadScene(currentSceneName);
            }
            else
            {
                Debug.LogWarning("Failed to parse stage number.");
            }
        }
        else
        {
            Debug.LogWarning("Not in a stage scene.");
        }
    }
}
