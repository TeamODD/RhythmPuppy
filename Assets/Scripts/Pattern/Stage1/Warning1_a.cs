using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pattern1_a;
using System;

public class Warning1_a : MonoBehaviour
{
    [HideInInspector]
    public float time;

    private SpriteRenderer GameObjectAlpha = null;
    private SpriteRenderer ArrowAlpha = null;

    void Start()
    {
        GameObject Arrow;
        GameObjectAlpha = gameObject.GetComponent<SpriteRenderer>();
        Arrow = gameObject.transform.GetChild(0).gameObject;
        ArrowAlpha = Arrow.GetComponent<SpriteRenderer>();
        GameObjectAlpha.color = new Color(1, 0.3f, 0.3f, 0);

        time = 0;

        gameObject.transform.position = new Vector3(9f, yPosition, 0);

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
            Destroy(gameObject);
        }

    }

}
