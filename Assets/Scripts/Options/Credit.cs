using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public void onCredit()
    {
        Time.timeScale = 0f; // �ð� ����� ����ϴ�.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Option_Stage ���� �ε��մϴ�.
        SceneManager.LoadSceneAsync("Credit", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Option_Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // Credit ���� �ε�Ǿ� �ִ��� Ȯ��
            Scene creditScene = SceneManager.GetSceneByName("Credit");

            if (creditScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync("Credit"); // Credit ���� ��ε�
                SceneManager.LoadScene("Option_Menu", LoadSceneMode.Additive); // Option_Menu �� �ε�
            }
        }
    }
}

