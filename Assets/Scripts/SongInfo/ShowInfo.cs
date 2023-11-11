using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour
{
    public TMP_Text Info;

    public void InfoModify()
    {
        switch (Menu_PlayerTransform.currentIndex)
        {
            //Menu_PlayerTransform ��ũ��Ʈ���� �ε����� ���� �� ���� ǥ��
            case 1:
                Info.text = "Tutorial";
                break;
            case 2:
                Info.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                break;
            case 4:
                Info.text = "Music: Feel Good\nArtist: MusicByAden";
                break;
            case 6:
                Info.text = "Music: Boss Battle\nArtist: Alex McCulloch";
                break;
            case 8:
                Info.text = "Music: Start the Engine\nArtist: LemonMusicStudio";
                break;
            case 10:
                Info.text = "Music: Gaming 8bit Music\nArtist: AlexiAction";
                break;
            default:
                Info.text = "";
                break;

        }
    }
}
