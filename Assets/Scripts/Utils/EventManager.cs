using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PlayerHitEvent();
    public delegate void DeathEvent();
    public delegate void ReviveEvent();

    public PlayerHitEvent playerHitEvent;
    public DeathEvent deathEvent;
    public ReviveEvent reviveEvent;

    AudioSource audioSource;

    void Awake()
    {
        init();
    }

    private void init()
    {
        audioSource = FindObjectOfType<AudioSource>();

        reviveEvent += loadCurrentSave;
    }

    private void loadCurrentSave()
    {
        if (audioSource.time < audioSource.clip.length * 0.25f)         // 0%~24.99...%
            audioSource.time = 0;
        else if (audioSource.time < audioSource.clip.length * 0.5f)     // 25%~49.99...% 
            audioSource.time = audioSource.time / audioSource.clip.length * 0.25f;
        else if (audioSource.time < audioSource.clip.length * 0.75f)     // 50%~74.99...% 
            audioSource.time = audioSource.time / audioSource.clip.length * 0.5f;
        else
            audioSource.time = audioSource.time / audioSource.clip.length * 0.75f;
    }

    public void revive()
    {
        loadCurrentSave();

        reviveEvent();
    }
}
