using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Background2_Alpha : MonoBehaviour
{
    private GameObject me;
    private SpriteRenderer meAlpha;
    private GameObject[] Img;
    private SpriteRenderer[] ImgAlpha;

    private int childCount;

    void Start()
    {
        me = this.gameObject;
        meAlpha = me.GetComponent<SpriteRenderer>();
        childCount = me.transform.childCount;

        Img = new GameObject[childCount];
        ImgAlpha = new SpriteRenderer[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Img[i] = me.transform.GetChild(i).gameObject;
            ImgAlpha[i] = Img[i].GetComponent<SpriteRenderer>();
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < childCount; i++)
        {
            ImgAlpha[i].color = meAlpha.color;
        }
    }
}
