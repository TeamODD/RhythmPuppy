using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] GameObject Option;
    GameObject gameplaymanager;

    public void Start()
    {
        gameplaymanager = GameObject.Find("GamePlayManager");
    }

    public void onContinue()
    {
        Option.SetActive(false);
        gameplaymanager.GetComponent<GamePasue>().ResumeGame();
    }
}
