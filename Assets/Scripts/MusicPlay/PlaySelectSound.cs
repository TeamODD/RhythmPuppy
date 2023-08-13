using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySelectSound : MonoBehaviour
{
    private AudioSource theAudio;
    [SerializeField]
    private AudioClip[] Music_Stage;

    public AudioSource audioSourceSelect;
    public AudioClip audioClipSelect;


    public static PlaySelectSound instance;

    void Awake()
    {
        if(PlaySelectSound.instance == null)
            PlaySelectSound.instance = this;

        theAudio = GetComponent<AudioSource>();
    }

    public void SelectSound()
    {
        audioSourceSelect.PlayOneShot(audioClipSelect);
    }
    public void ChangeMusic(int Index)
    {
        if (Music_Stage[Index] == null) return;
        theAudio.clip = Music_Stage[Index];
        theAudio.Play();
    }
}
