using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour
{
    public TMP_Text Info;

    void Update()
    {
        if (Info != null)
        {
            
        } //Info 트라이
        switch (Menu_PlayerTransform.currentIndex)
        {
            //Menu_PlayerTransform 스크립트에서 인덱스에 따라 곡 정보 표시
            case 0: Info.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                break;
            case 1: Info.text = "Music: Feel Good\nArtist: MusicbyAden";
                break;

        }
        
    }
}
