using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Curtain
{
    public class Curtain : MonoBehaviour
    {
        public GameObject RawImage;
        public GameObject VideoObject;
        public VideoPlayer Video;
        public VideoClip CurtainClose;
        public VideoClip CurtainOpen;
        public VideoClip CurtainCloseOpen;


        void Start()
        {
        }

        void CurtainEffect(string Status)
        {
            StartCoroutine(CurtainCoroutine(Status));
        }

        IEnumerator CurtainCoroutine(string Status)
        {
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
            Video.time = 0;
            /*Video.Prepare();
            if (Video.isPrepared)
                Video.Play();*/
            Video.Play();

            yield return Video;
        }
    }
}
