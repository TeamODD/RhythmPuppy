using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Menu_PlayerTransform;

public class PlayerWalkingSound : MonoBehaviour
{
    private AudioSource theAudio;
    private AudioClip EmptyAudio;
    public AudioClip[] Walking;
    [SerializeField]
    private AudioClip[] Music_Stage;

    public static PlayerWalkingSound instance;

    private void Awake()
    {
        EmptyAudio = null;

        if (PlayerWalkingSound.instance == null)
            PlayerWalkingSound.instance = this;

        theAudio = GetComponent<AudioSource>();
    }

    public void World1_Walking()
    {
        theAudio.clip = Walking[0];
        theAudio.Play();
    }
    public void World2_Walking()
    {
        theAudio.clip = Walking[1];
        theAudio.Play();
    }
    public void ChangeMusic()
    {
        if (Music_Stage[currentIndex] == null) //���� �������� ����Ʈ���� ������ ���� ���, �� �ɾ�ٴ� �κ��� ��� �Ҹ��� ���ְ� ���
        {
            theAudio.clip = EmptyAudio;
            theAudio.Play();
        }
    }
}
