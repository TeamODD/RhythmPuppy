using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySelectSound : MonoBehaviour
{
    public AudioSource audioSourceSelect;
    public AudioClip audioClipSelect;


    public static PlaySelectSound instance;

    void Awake()
    {
        if(PlaySelectSound.instance == null)
            PlaySelectSound.instance = this;
    }

    public void SelectSound()
    {
        audioSourceSelect.PlayOneShot(audioClipSelect);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
