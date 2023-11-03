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
        RawImage.SetActive(true);
        CloseObject.SetActive(true);
        Close.time = 0f;
        Close.Play();
        
        yield return new WaitForSeconds(3f);
        Open.time = 0f;
        Open.Play();
        CloseObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        RawImage.SetActive(false);
        yield break;

    }
}
