using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Curtain : MonoBehaviour
{
    public GameObject RawImage;
    public GameObject VideoObject;
    public VideoPlayer Video;
    public VideoClip CloseClip;
    public VideoClip OpenClip;
    
    void Start()
    {
        CurtainClose();
        //Invoke("CurtainOpen", 3f);
    }

    void CurtainClose()
    {
        StartCoroutine(CurtainCloseCoroutine());
    } 

    IEnumerator CurtainCloseCoroutine()
    {
        //RawImage.SetActive(true);
        Video.Play();

        yield return new WaitForSeconds(3f);

        Video.playbackSpeed = 0f;

        Video.Play();
        
        yield break;
    }

    void CurtainOpen()
    {
        StartCoroutine(CurtainOpenCoroutine());
    }

    IEnumerator CurtainOpenCoroutine()
    {


        yield break;
    }
}
