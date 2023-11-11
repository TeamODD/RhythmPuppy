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
        if (Music_Stage[currentIndex] == null) //음악 스테이지 리스트에서 음악이 없을 경우, 즉 걸어다는 부분일 경우 소리를 없애고 재생
        {
            theAudio.clip = EmptyAudio;
            theAudio.Play();
        }
    }
}
