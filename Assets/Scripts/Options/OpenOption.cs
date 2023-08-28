using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenOptionStage : MonoBehaviour
{
    //��� ������ ������ ã�ƾ� �Ѵ�.

    bool isOptionOpened;
    string PlayingScene;

    private void Start()
    {
        PlayingScene = SceneManager.GetActiveScene().name;
    }

    private void SearchPlayingScene()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(isOptionOpened)
            {
                ClosetheOption();
            }
            else if (!isOptionOpened)
            {
                OpentheOption();
            }
        }
    }

    private void OpentheOption()
    {
        isOptionOpened = true;
        SceneManager.LoadScene("Option_stage",LoadSceneMode.Additive);
    }

    private void ClosetheOption()
    {
        isOptionOpened = false;
        SceneManager.UnloadSceneAsync("Option_stage");
    }
}
