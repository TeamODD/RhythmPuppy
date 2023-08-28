using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Option;
    public AudioSource musicsource;

    public void onExitToMenu()
    {
        Option.SetActive(false);
        musicsource.Stop();

        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        SceneManager.LoadScene("SceneMenu_01", LoadSceneMode.Single);
    }
}
