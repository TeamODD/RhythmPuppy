using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern8 : MonoBehaviour
{
    private ObjectPoolManager PoolingManager;
    private SpriteRenderer ObjectSprite;
    [HideInInspector]
    public bool IsPooled = false;
    [HideInInspector]
    public float time = 0;
    
    void Update()
    {
        time += Time.fixedDeltaTime;
        if (time < 0.8f)
        {
            ObjectSprite.color = new Color(1, 1, 1, time * (5 / 4));
        }
        else if (time > 0.8f && time < 1f)
        {
            ObjectSprite.color = new Color(1, 1, 1, 1);
        }
        else if (time > 1f && time < 2.5f)
        {
            ObjectSprite.color = new Color(1, 1, 1, 1 - ((time-1) * (2/3)));
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
        ObjectSprite = gameObject.GetComponent<SpriteRenderer>();
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
