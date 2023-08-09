using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static Menu_PlayerTransform;

public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        Invoke("GotoStage", 3f); 
    }

    void GotoStage()
    {
        SceneManager.LoadScene("SceneStage" + (currentIndex + 1));
    }
}
