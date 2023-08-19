using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour
{
    public TMP_Text Info;
    [SerializeField]
    private TMP_Text StageInfo;

    public void InfoModify()
    {
        StageInfoFunc();
        switch (Menu_PlayerTransform.currentIndex)
        {
            //Menu_PlayerTransform 스크립트에서 인덱스에 따라 곡 정보 표시
            case 1:
                Info.text = "Music: \nArtist: ";
                break;
            case 2:
                Info.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                break;
            case 4:
                Info.text = "Music: Feel Good\nArtist: MusicbyAden";
                break;
            case 6:
                Info.text = "Music: Boss Battle\nArtist: Alex McCulloch";
                break;
            case 8:
                Info.text = "Music: Start the Engine\nArtist: lemonmusicstudio";
                break;
            case 10:
                Info.text = "Music: Gaming 8bit Music\nArtist: AlexiAction";
                break;
            default:
                Info.text = "";
                break;

        }
    }
    public void StageInfoFunc()
    {
        if (StageInfo == null) return;

        switch (Menu_PlayerTransform.currentIndex)
        {
            case 1:
                StageInfo.text = "Tutorial";
                break;
            case 2:
                StageInfo.text = "Stage 1-1";
                break;
            case 4:
                StageInfo.text = "Stage 1-2";
                break;
            case 6:
                StageInfo.text = "Stage 1-3";
                break;
            case 8:
                StageInfo.text = "Stage 2-1";
                break;
            case 10:
                StageInfo.text = "Stage 2-2";
                break;
            default:
                StageInfo.text = "";
                break;
        }
    }
}
