using System.Collections;
using System.Collections.Generic;
using EventManagement;
using Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    GameObject Option;
    EventManager eventManager;

    void Awake()
    {
        eventManager = GetComponentInParent<EventManager>();
    }

    public void onContinue()
    {
        //GameObject GamePlayManager = GameObject.Find("GamePlayManager");
        //GamePlayManager.GetComponent<GamePause>().ResumeGame();
        eventManager.onResume.Invoke();

        Option.SetActive(false);
    }
}
