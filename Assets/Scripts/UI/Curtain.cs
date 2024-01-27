using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using UnityEngine.Video;

public class Curtain : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    public GameObject RawImage;
    public GameObject VideoObject;
    public VideoPlayer Video;
    public VideoClip CurtainClose;
    public VideoClip CurtainOpen;
    public VideoClip CurtainCloseOpen;


    void Start()
    {
        init();
    }

    void init()
    {
        DontDestroyOnLoad(Canvas);

        //Video.Pause();
        Video.time = 0;
        //Video.playOnAwake = false;
    }

    public void CurtainEffect(string Status, float time)
    {
        StartCoroutine(CurtainCoroutine(Status, time));
    }

    public IEnumerator CurtainCoroutine(string Status, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        VideoObject.SetActive(true);
        switch (Status)
        {
            case "Close":
                Video.clip = CurtainClose;
                break;
            case "Open":
                Video.clip = CurtainOpen;
                break;
            case "CloseOpen":
                Video.clip = CurtainCloseOpen;
                break;
        }
        //RawImage.SetActive(true);
        //Video.time = 0;
        //Video.Prepare();
        /*if (Video.isPrepared)
            Video.Play();*/
        //Video.Play();
        float offset = 1f;
        Debug.Log("Curtain Action");
        //Destroy(Canvas, 5f);
        yield return new WaitForSeconds((float)Video.length + offset);
        VideoObject.SetActive(false);

        yield return Video;
    }

    public void Pause()
    {
        Video.Pause();
    }
}

