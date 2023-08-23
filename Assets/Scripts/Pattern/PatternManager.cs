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

    void Awake()
    {
        init();
    }

    public void init()
    {
        overlayCanvas = uiCanvas.Find("OverlayCanvas");
        worldSpaceCanvas = uiCanvas.Find("WorldSpaceCanvas");
        Invoke("runPattern", startDelay);
    }

    private void runPattern()
    {
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(transform);
        o.SetActive(true);
    }
}
