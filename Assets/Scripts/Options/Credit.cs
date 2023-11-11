using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public void onCredit()
    {
        Time.timeScale = 0f; // 시간 경과를 멈춥니다.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Option_Stage 씬을 로드합니다.
        SceneManager.LoadSceneAsync("Credit", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Option_Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // Credit 씬이 로드되어 있는지 확인
            Scene creditScene = SceneManager.GetSceneByName("Credit");

            if (creditScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync("Credit"); // Credit 씬을 언로드
                SceneManager.LoadScene("Option_Menu", LoadSceneMode.Additive); // Option_Menu 씬 로드
            }
        }
    }
}

