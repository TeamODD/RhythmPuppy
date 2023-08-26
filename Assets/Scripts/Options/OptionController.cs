using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    [SerializeField] private GameObject mainCameraToturnOff;
    [SerializeField] private GameObject eventSystemToturnOff;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject[] eventSystemObjects = GameObject.FindGameObjectsWithTag("EventSystem");
        foreach (GameObject eventSystemObject in eventSystemObjects)
        {
            if (eventSystemObject != eventSystemToturnOff)
                eventSystemToturnOff.SetActive(false);
        }

        GameObject[] mainCameraObjects = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject mainCameraObject in mainCameraObjects)
        {
            if (mainCameraObject != mainCameraToturnOff)
                mainCameraToturnOff.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC 키를 누르면 일시정지/해제
        {
            if (SceneManager.GetSceneByName("Option_Stage").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Option_Stage");
            }
            if (SceneManager.GetSceneByName("Option_Menu").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Option_Menu");
            }
        }
    }

}