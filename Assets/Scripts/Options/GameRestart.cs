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
                // �������� ���ڸ� �Ľ��Ͽ� ���� �ٽ� �ҷ����� ó��
                SceneManager.LoadSceneAsync(currentSceneName);
                Time.timeScale = 1f;
                Debug.Log("�� �ҷ�����Խ��ϴ�.");
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
