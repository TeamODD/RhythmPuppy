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
    [SerializeField]
    private TMP_Text Tip;

    public void LoadingInfoModify()
    {
        switch (currentIndex)
        {
            case 1:
                SongInfo.text = "Music: 8 bit ice cave lofi\nArtist: Tad Miller";
                break;
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
    public void TipInfoFunc()
    {
        int randomX = Random.Range(1, 8);

        switch (randomX)
        {
            case 1:
                Tip.text = "Tip: Retrieving a thrown bone by right-clicking restores\r\nthe Corgi's stamina slightly.";
                break;
            case 2:
                Tip.text = "Tip: The reason Corgi always carries a bone in his mouth\r\nis to give it to Pomerania.";
                break;
            case 3:
                Tip.text = "Tip: Corgi can double jump by pressing the space bar\r\nonce more.";
                break;
            case 4:
                Tip.text = "Tip: Corgi's dash and bone throwing consume stamina.\r\nBe careful of stamina depletion!";
                break;
            case 5:
                Tip.text = "Tip: In hard mode, your rank is recorded. If you are\r\nconfident in your skills, try hard mode!\r\n";
                break;
            case 6:
                Tip.text = "Tip: The Corgi's stamina automatically recovers little by little.\r\n";
                break;
            case 7:
                Tip.text = "Tip: Clear ranks range from S to C ranks.\r\nChallenge yourself to a high rank!\r\n";
                break;
        }
    }
}
