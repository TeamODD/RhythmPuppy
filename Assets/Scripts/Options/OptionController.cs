using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        // 현재 Scene에서 Event System 찾기
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        // 첫 번째 이외의 Event System 비활성화
        for (int i = 1; i < eventSystems.Length; i++)
        {
            eventSystems[i].gameObject.SetActive(false);
        }
    }
}
