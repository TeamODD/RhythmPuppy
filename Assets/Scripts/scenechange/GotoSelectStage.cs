using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoSelectStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown) 
        {
            PlaySelectSound.instance.SelectSound();
            PlaySelectSound.instance.audioSourceSelect.Play();

            SceneManager.LoadScene("SceneMenu_01");

        }
    }
}
