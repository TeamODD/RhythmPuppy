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
            case 0: Info.text = "Song = Forest\nArtist = NewJudge";
                break;
            case 1: Info.text = "Song = On it\nArtist = SaltMan";
                break;

        }
        
    }
}
