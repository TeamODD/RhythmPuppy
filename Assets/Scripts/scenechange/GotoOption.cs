using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GotoOption : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused = false;
    
    public void GoOption()
    {
        if (!isPaused && !SceneManager.GetSceneByName("Option_Menu").isLoaded) //정지 중이지 않을 때 중지
        {
            GameObject.Find("GotoOption").GetComponent<Button>().interactable = false;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            GameObject.Find("MusicSoundManager").GetComponent<AudioSource>().Pause();
            GameObject.Find("SFXSoundManager").GetComponent<AudioSource>().Pause();
            isPaused = true;
            Menu_PlayerTransform.IsPaused = true;
            SceneManager.LoadSceneAsync("Option_Menu", LoadSceneMode.Additive);
        }
    }

    void Update()
    {
        if (Menu_PlayerTransform.ReadyToGoStage) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !SceneManager.GetSceneByName("Option_Menu").isLoaded) //정지 중이지 않을 때 중지
            {
                GameObject.Find("GotoOption").GetComponent<Button>().interactable = false;

                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                GameObject.Find("MusicSoundManager").GetComponent<AudioSource>().Pause();
                GameObject.Find("SFXSoundManager").GetComponent<AudioSource>().Pause();
                isPaused = true;
                Menu_PlayerTransform.IsPaused = true;
                SceneManager.LoadSceneAsync("Option_Menu", LoadSceneMode.Additive);
            }
            else if (isPaused && SceneManager.GetSceneByName("Option_Menu").isLoaded) //정지 중일 때 중지 중단
            {
                GameObject.Find("GotoOption").GetComponent<Button>().interactable = true;

                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                GameObject.Find("MusicSoundManager").GetComponent<AudioSource>().Play();
                GameObject.Find("SFXSoundManager").GetComponent<AudioSource>().Play();
                isPaused = false;
                Menu_PlayerTransform.IsPaused = false;
                SceneManager.UnloadSceneAsync("Option_Menu");
            }
        }
    }
}
