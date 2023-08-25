using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToNextPage : MonoBehaviour
{
    [SerializeField]
    GameObject HowToPlay;
    [SerializeField]
    GameObject UI1;
    [SerializeField]
    GameObject UI2;
    // Start is called before the first frame update
    void Start()
    {
        HowToPlay.SetActive(true);
        UI1.SetActive(false);
        UI2.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (HowToPlay.activeSelf)
        {
            HowToPlay.SetActive(false);
            UI1.SetActive(true);
            UI2.SetActive(false);
        }

        else if (UI1.activeSelf)
        {
            HowToPlay.SetActive(false);
            UI1.SetActive(false);
            UI2.SetActive(true);
        }

        else if (UI2.activeSelf)
        {
            HowToPlay.SetActive(false);
            UI1.SetActive(false);
            UI2.SetActive(false);
            Debug.Log("게임화면이 열렸습니다?");
        }
    }
}
