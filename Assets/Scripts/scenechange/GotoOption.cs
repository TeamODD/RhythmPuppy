using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoOption : MonoBehaviour
{
    private bool isPaused = false;

    public void GoOption()
    {
        isPaused = true;
        SceneManager.LoadScene("Option_Menu", LoadSceneMode.Additive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!isPaused)
            {
                Time.timeScale = 0f;
                isPaused = true;
                SceneManager.LoadScene("Option_Menu", LoadSceneMode.Additive);
            }
            else if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
                SceneManager.UnloadSceneAsync("Option_Menu");
            }
    }
}
