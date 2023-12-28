using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Curtain : MonoBehaviour
{
    public GameObject RawImage;
    public GameObject VideoObject;
    public VideoPlayer Video;
    public VideoClip CurtainClip;

    void Start()
    {
        CurtainEffect();
    }

    void CurtainEffect()
    {
        StartCoroutine(CurtainCoroutine());
    } 

    IEnumerator CurtainCoroutine()
    {
        Video.clip = CurtainClip;
        //RawImage.SetActive(true);
        Video.Prepare();
        if (Video.isPrepared)
            Video.Play();

        yield break;
    }
}
