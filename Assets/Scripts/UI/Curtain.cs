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

    public void CurtainClose()
    {
        StartCoroutine(CurtainCloseCoroutine());
    } 

    public void CurtainOpen()
    {
        StartCoroutine(CurtainOpenCoroutine());
    }

    IEnumerator CurtainCloseCoroutine()
    {
        Video.clip = CurtainClip;
        //RawImage.SetActive(true);
        Video.Prepare();
        if (Video.isPrepared)
            Video.Play();

        yield break;
    }
}
