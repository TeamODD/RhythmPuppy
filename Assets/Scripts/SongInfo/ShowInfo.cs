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
            
        } //Info Ʈ����
        switch (Menu_PlayerTransform.currentIndex)
        {
            //Menu_PlayerTransform ��ũ��Ʈ���� �ε����� ���� �� ���� ǥ��
            case 0: Info.text = "Music: Minimal Inspiring Ambient\nArtist: ComaStudio";
                break;
            case 1: Info.text = "Music: Feel Good\nArtist: MusicbyAden";
                break;

        }
        
    }
}
