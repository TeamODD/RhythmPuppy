using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour
{
    public TMP_Text Info;
    [SerializeField]
    private GameObject Enter;

    public void InfoModify()
    {
        switch (Menu_PlayerTransform.currentIndex)
        {
            //Menu_PlayerTransform 스크립트에서 인덱스에 따라 곡 정보 표시
            case 1:
                Info.text = "Tutorial";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            case 2:
                Info.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            case 4:
                Info.text = "Music: Feel Good\nArtist: MusicByAden";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            case 6:
                Info.text = "Music: Boss Battle\nArtist: Alex McCulloch";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            case 8:
                Info.text = "Music: Start the Engine\nArtist: LemonMusicStudio";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            case 10:
                Info.text = "Music: Gaming 8bit Music\nArtist: AlexiAction";
                Enter.GetComponent<Menu_Enter>().Enter("appear");
                break;
            default:
                Info.text = "";
                break;

        }
    }
    public void EmptyInfo()
    {
        Info.text = "";
        Enter.GetComponent<Menu_Enter>().Enter("disappear");
    }
}
