using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField]
    GameObject BackGround;
    void Start()
    {
        for (int i = 0; i < BackGround.transform.childCount; i++)
        {
            GameObject backgroundImg = transform.GetChild(i).gameObject;
            SpriteRenderer sprite = backgroundImg.GetComponent<SpriteRenderer>();
            Debug.Log(i+":"+sprite.sortingOrder);
        }   
    }

    IEnumerator ImgRouting(GameObject Img, float MovingSpeed)
    {
        yield return null;
    }
}
