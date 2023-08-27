using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOption : MonoBehaviour
{
    //모든 변수는 스스로 찾아야 한다.
    AudioSource StageMusic;

    bool isOptionOpened;

    private void Start()
    {
        StageMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
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
    }

    private void ClosetheOption()
    {
        isOptionOpened = false;
    }
}
