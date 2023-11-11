using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Menu_PlayerTransform;

public class LoadingInfo : MonoBehaviour
{
    [SerializeField]
    private TMP_Text SongInfo;
    [SerializeField]
    private TMP_Text StageInfo;

    public void LoadingInfoModify()
    {
        switch (currentIndex)
        {
            case 2:
                SongInfo.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                break;
            case 4:
                SongInfo.text = "Music: Feel Good\nArtist: MusicbyAden";
                break;
            case 6:
                SongInfo.text = "Music: Boss Battle\nArtist: Alex McCulloch";
                break;
            case 8:
                SongInfo.text = "Music: Start the Engine\nArtist: lemonmusicstudio";
                break;
            case 10:
                SongInfo.text = "Music: Gaming 8bit Music\nArtist: AlexiAction";
                break;
            default:
                SongInfo.text = "";
                break;
        }
    }
    public void StageInfoFunc()
    {
        switch (currentIndex)
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
