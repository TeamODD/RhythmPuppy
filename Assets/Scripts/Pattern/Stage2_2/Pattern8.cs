using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pattern8 : MonoBehaviour
{
    private ObjectPoolManager PoolingManager;
    private Image ObjectImage;
    [HideInInspector]
    public bool IsPooled = false;
    [HideInInspector]
    public float time = 0;
    
    void Update()
    {
        time += Time.fixedDeltaTime;
        if (time < 0.8f)
        {
            ObjectImage.color = new Color(1, 1, 1, time * (5 / 4));
        }
        else if (time > 0.8f && time < 1f)
        {
            ObjectImage.color = new Color(1, 1, 1, 1);
        }
        else if (time > 1f && time < 2.5f)
        {
            ObjectImage.color = new Color(1, 1, 1, 1 - ((time-1) * (2/3)));
        }
        else
            DestroyObject();
    }

    void Awake()
    {
        init();
    }

    void init()
    {
        ObjectImage = gameObject.GetComponent<Image>();
        PoolingManager = FindObjectOfType<ObjectPoolManager>();
    }

    void DestroyObject()
    {
        if (IsPooled)
        {
            PoolingManager.ReleaseObject();
        }
        else
            Destroy(this);
    }
}
