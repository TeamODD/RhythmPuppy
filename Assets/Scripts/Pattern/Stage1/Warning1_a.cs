using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pattern1_a;
using System;
using UnityEngine.UI;

public class Warning1_a : MonoBehaviour
{
    [HideInInspector]
    public float time;
    ObjectPoolManager PoolingManager;
    [HideInInspector]
    public bool IsPooled = false;

    private SpriteRenderer GameObjectAlpha = null;
    private SpriteRenderer ArrowAlpha = null;

    private Image gameObjectAlpha = null;
    private Image arrowAlpha = null;

    void Start()
    {
        PoolingManager = FindObjectOfType<ObjectPoolManager>();

        GameObject Arrow;
        try
        {
            GameObjectAlpha = gameObject.GetComponent<SpriteRenderer>();
            Arrow = gameObject.transform.GetChild(0).gameObject;
            ArrowAlpha = Arrow.GetComponent<SpriteRenderer>();
            GameObjectAlpha.color = new Color(1, 0.3f, 0.3f, 0);
        }
        catch(MissingComponentException)
        {
            gameObjectAlpha = gameObject.GetComponent<Image>();
            Arrow = gameObjectAlpha.transform.GetChild(0).gameObject;
            arrowAlpha = Arrow.GetComponent<Image>();
        }

        time = 0;
        if(!IsPooled)
            gameObject.transform.position = new Vector3(9f, yPosition, 0);

    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (gameObjectAlpha != null && GameObjectAlpha == null)
        {
            if (time < 0.5f)
            {
                gameObjectAlpha.color = new Color(1, 0.3f, 0.3f, time / 1f);
                arrowAlpha.color = new Color(1, 0.3f, 0.3f, time / 1f);
            }
            else
            {
                gameObjectAlpha.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
                arrowAlpha.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
            }
            
        }

        if (gameObjectAlpha == null && GameObjectAlpha != null)
        {

            if (time < 0.5f)
            {
                GameObjectAlpha.color = new Color(1, 0.3f, 0.3f, time / 1f);
                ArrowAlpha.color = new Color(1, 0.3f, 0.3f, time / 1f);
            }
            else
            {
                GameObjectAlpha.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
                ArrowAlpha.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
            }
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
