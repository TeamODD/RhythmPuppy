using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMenu : MonoBehaviour
{   

    // Start is called before the first frame update
    void Start()
    {
        Invoke("scenechange", 3);

    }
    void scenechange()
    {
        SceneManager.LoadScene("SceneGameplay");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
