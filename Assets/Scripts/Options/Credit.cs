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
        Time.fixedDeltaTime = 0;

        // Option_Stage 씬을 로드합니다.
        SceneManager.LoadScene("Credit");
    }

    private void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.StartsWith("Credit"))
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("Option_Menu");
            }
        }
    }
}
