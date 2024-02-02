using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Option;
    public AudioSource musicsource;

    public void onExitToMenu()
    {
        /*
        Option.SetActive(false);
        musicsource.Stop();

        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        SceneManager.LoadScene("SceneMenu_01", LoadSceneMode.Single);
        */

        if (SceneManager.GetSceneByName("Option_Menu").isLoaded) //정지 중일 때 중지 중단
        {
            GameObject.Find("GotoOption").GetComponent<Button>().interactable = true;

            GameObject.Find("SceneManager").GetComponent<GotoOption>().isPaused = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            GameObject.Find("MusicSoundManager").GetComponent<AudioSource>().Play();
            GameObject.Find("SFXSoundManager").GetComponent<AudioSource>().Play();
            Menu_PlayerTransform.IsPaused = false;
            SceneManager.UnloadSceneAsync("Option_Menu");
        }
        else if (SceneManager.GetSceneByName("Option_Stage").isLoaded)
        {
            GameObject.Find("GamePlayManager").GetComponent<GamePause>().isPaused = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Menu_PlayerTransform.IsPaused = false;
            SceneManager.LoadScene("SceneMenu_01", LoadSceneMode.Single);
        }
        else if (SceneManager.GetSceneByName("GameOver").isLoaded)
        {
            SceneManager.LoadScene("SceneMenu_01", LoadSceneMode.Single);
        }
    }
}
