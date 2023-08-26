using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    GameObject Option;

    public void onContinue()
    {
        GameObject GamePlayManager = GameObject.Find("GamePlayManager");
        GamePlayManager.GetComponent<GamePasue>().ResumeGame();

        Option.SetActive(false);
    }
}
