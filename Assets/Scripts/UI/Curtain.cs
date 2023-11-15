using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Curtain : MonoBehaviour
{
    public GameObject RawImage;
    public GameObject CloseObject;
    public VideoPlayer Open;
    public VideoPlayer Close;

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
        RawImage.SetActive(true);
        CloseObject.SetActive(true);
        Close.time = 0f;
        Close.Play();
        
        yield break;
    }

    IEnumerator CurtainOpenCoroutine()
    {
        Open.time = 0f;
        Open.Play();

        RawImage.SetActive(false);
        yield break;
    }
}
