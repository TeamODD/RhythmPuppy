using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public Transform uiCanvas, obstacleManager;
    public GameObject warningBox, warningArrow;
    [SerializeField] GameObject pattern;
    [SerializeField] float startDelay;

    [HideInInspector]
    public Transform overlayCanvas, worldSpaceCanvas;
    [HideInInspector] public EventManager eventManager;

    void Awake()
    {
        init();
    }

    public void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        overlayCanvas = uiCanvas.Find("OverlayCanvas");
        worldSpaceCanvas = uiCanvas.Find("WorldSpaceCanvas");

        eventManager.deathEvent += deathEvent;
        eventManager.reviveEvent += reviveEvent;

        Invoke("run", startDelay);
    }

    public void run()
    {
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(transform);
        o.SetActive(true);
    }

    private void deathEvent()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < obstacleManager.childCount; i++)
        {
            Destroy(obstacleManager.GetChild(i).gameObject);
        }
    }

    private void reviveEvent()
    {
        run();
    }
}