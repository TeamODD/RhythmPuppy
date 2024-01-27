using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_corgi : MonoBehaviour
{
    SpriteRenderer[] childSprites;
    SpriteRenderer thisSprite;
    [HideInInspector]
    public bool IsLoading = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        childSprites = new SpriteRenderer[gameObject.transform.childCount -2];
        thisSprite = gameObject.GetComponent<SpriteRenderer>();

        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            if (childObject.GetComponent<SpriteRenderer>() == null) return;
            childSprites[i] = childObject.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        for (int i = 0; i < childSprites.Length; i++)
        {
            childSprites[i].color = thisSprite.color;
        }
        if (!IsLoading) return;

        time += Time.fixedDeltaTime;
        thisSprite.color = new Color(1, 1, 1, time);

    }
}
