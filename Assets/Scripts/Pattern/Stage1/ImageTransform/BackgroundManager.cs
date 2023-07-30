using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private Sprite bgSprite;
    [SerializeField]
    private GameObject Img1;
    [SerializeField]
    private GameObject tempImg1;

    void Start()
    {
        img1Rout();
    }
    
    void FixedUpdate()
    {
        if (Img1 == null || tempImg1 == null)
            img1Rout();
    }

    void img1Rout()
    {
        if (Img1 == null)
        {
            Img1 = Instantiate(tempImg1);
            Img1.transform.position = Img1.transform.position;
            Img1.transform.position += new Vector3(bgSprite.bounds.size.x - 0.04f, 0, 0);
        }
        else
        {
            tempImg1 = Instantiate(Img1);
            tempImg1.transform.position = Img1.transform.position;
            tempImg1.transform.position += new Vector3(bgSprite.bounds.size.x - 0.04f, 0, 0);
        }
    }
}
