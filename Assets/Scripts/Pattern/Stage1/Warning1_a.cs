using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pattern1_a;

public class Warning1_a : MonoBehaviour
{
    [HideInInspector]
    public float time;
    ObjectPoolManager PoolingManager;
    [HideInInspector]
    public bool IsPooled = false;

    private SpriteRenderer GameObjectAlpha;
    private SpriteRenderer ArrowAlpha;

    void Start()
    {
        PoolingManager = FindObjectOfType<ObjectPoolManager>();

        GameObject Arrow;
        GameObjectAlpha = gameObject.GetComponent<SpriteRenderer>();
        Arrow = gameObject.transform.GetChild(0).gameObject;
        ArrowAlpha = Arrow.GetComponent<SpriteRenderer>();

        time = 0;
        if(!IsPooled)
            gameObject.transform.position = new Vector3(9f, yPosition, 0);
        GameObjectAlpha.color    = new Color(1, 0.3f, 0.3f, 0);
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

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
