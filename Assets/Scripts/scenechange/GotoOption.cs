using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoOption : MonoBehaviour
{   
    public void GoOption()
    {
        SceneManager.LoadScene("Option_Menu");
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Option_Menu");
    }
}
