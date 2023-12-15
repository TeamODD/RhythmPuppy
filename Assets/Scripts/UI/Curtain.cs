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
        //Invoke("CurtainOpen", 3f);
    }

    void CurtainClose()
    {
        StartCoroutine(CurtainCloseCoroutine());
    } 

    IEnumerator CurtainCloseCoroutine()
    {
        Video.clip = CloseClip;
        //RawImage.SetActive(true);
        Video.Prepare();
        if (Video.isPrepared)
            Video.Play();

        yield return new WaitForSeconds(3f);

        yield break;
    }

    void CurtainOpen()
    {
        StartCoroutine(CurtainOpenCoroutine());
    }

    IEnumerator CurtainOpenCoroutine()
    {
        Video.clip = OpenClip;

        Video.Prepare();
        yield return Video.isPrepared;
        if (Video.isPrepared)
            Video.Play();

        yield return new WaitForSeconds(3f);

        yield break;
    }
}
