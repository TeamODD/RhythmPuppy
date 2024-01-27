using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning7_a : MonoBehaviour
{
    [HideInInspector]
    public float time;
    ObjectPoolManager PoolingManager;
    [HideInInspector]
    public bool IsPooled = false;

    private Image gameObjectAlpha;
    private Image arrowAlpha;

    void Start()
    {
        PoolingManager = FindObjectOfType<ObjectPoolManager>();

        GameObject Arrow;
        gameObjectAlpha = gameObject.GetComponent<Image>();
        Arrow = gameObjectAlpha.transform.GetChild(0).gameObject;
        arrowAlpha = Arrow.GetComponent<Image>();
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
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
