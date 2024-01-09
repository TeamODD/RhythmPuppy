using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Pattern8_WarningBox : MonoBehaviour
{
    [HideInInspector]
    public float time;
    ObjectPoolManager PoolingManager;
    [HideInInspector]
    public bool IsPooled = false;

    private Image gameObjectAlpha = null;

    void Start()
    {
        PoolingManager = FindObjectOfType<ObjectPoolManager>();

        gameObjectAlpha = gameObject.GetComponent<Image>();

        time = 0;

    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time < 0.5f)
        {
            gameObjectAlpha.color = new Color(1, 0.3f, 0.3f, time / 1f);
        }
        else
        {
            gameObjectAlpha.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
        }


        if (time > 1f)
        {
            DestroyObject();
        }

    }

    public void DestroyObject()
    {
        if (IsPooled)
            PoolingManager.ReleaseObject();
        else
            Destroy(gameObject);
    }

}