using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    private Text INFO;
    INFO = GameObject.Find("INFO").GetComponent<Text>();
    public void showInfo(int num)
    {
        switch (num)
        {
            case 1: INFO.text = "Song : asda\nArtist : sda";
                break;
            case 2: INFO.text = "Song : uiyuiy\nArtist : ityuty";
                break;
        }
    }
}
