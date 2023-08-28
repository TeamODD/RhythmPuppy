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
        GameObject.Find("SoundManager").GetComponent<AudioSource>().Pause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!isPaused)
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0;
                GameObject.Find("SoundManager").GetComponent<AudioSource>().Pause();
                isPaused = true;
                SceneManager.LoadSceneAsync("Option_Menu", LoadSceneMode.Additive);
            }
            else if (isPaused)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                GameObject.Find("SoundManager").GetComponent<AudioSource>().Play();
                isPaused = false;
                SceneManager.UnloadSceneAsync("Option_Menu");
            }
    }
}
