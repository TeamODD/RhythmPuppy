using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    GameObject Option;

    public void onContinue()
    {
        GameObject GamePlayManager = GameObject.Find("GamePlayManager");
        GamePlayManager.GetComponent<GamePause>().ResumeGame();

        Option.SetActive(false);
    }
}
