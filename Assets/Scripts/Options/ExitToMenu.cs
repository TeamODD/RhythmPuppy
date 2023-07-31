using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Option;
    [SerializeField] GameObject SceneMenu;
    public AudioSource musicsource;

    public void onExitToMenu()
    {
        Option.SetActive(false);
        musicsource.Stop();

        SceneManager.LoadScene("SceneMenu_01");
    }
}
