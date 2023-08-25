using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [HideInInspector]
    public float[] savePointTime;
    public delegate void PlayerHitEvent();
    public delegate void DeathEvent();
    public delegate void RewindEvent();
    public delegate void ReviveEvent();

    public PlayerHitEvent playerHitEvent;
    public DeathEvent deathEvent;
    public RewindEvent rewindEvent;
    public ReviveEvent reviveEvent;

    AudioSource audioSource;

    void Awake()
    {
        init();
    }

    private void init()
    {
        audioSource = FindObjectOfType<AudioSource>();

    }
}
