using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float time;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < 0.5f)
            spriteRenderer.color = new Color(1, 0.3f, 0.3f, time / 1f);
        else
            spriteRenderer.color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);

        //1초후 gameObject 삭제
        Destroy(gameObject, 1f);
    }
}
