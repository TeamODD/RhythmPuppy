using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pattern8 : MonoBehaviour
{
    private ObjectPoolManager PoolingManager;
    private Image ObjectImage = null;
    [HideInInspector]
    public bool IsPooled = false;
    [HideInInspector]
    public float time = 0;
    
    void Update()
    {
        time += Time.fixedDeltaTime;

        if (time > 4f) DestroyObject();
        if (ObjectImage == null) return;

        if (time < 0.8f)
        {
            ObjectImage.color = new Color(1, 1, 1, time * (5 / 4));
        }
        else if (time > 0.8f && time < 2.5f)
        {
            ObjectImage.color = new Color(1, 1, 1, 1);
        }
        else if (time > 2.5f && time < 4f)
        {
            ObjectImage.color = new Color(1, 1, 1, 1 - ((time-2.5f) * (2/3)));
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
        ObjectImage = null;
        PoolingManager = FindObjectOfType<ObjectPoolManager>();

        try
        {
            ObjectImage = gameObject.GetComponent<Image>();
        }
        catch 
        {
            //UI 프리팹과 호환되도록 장애물 프리팹을 위해 제작하였음. 
        }

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
