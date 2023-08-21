using System;
using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using World_2;

public abstract class PatternBase : MonoBehaviour
{
    [SerializeField] AudioClip BGM;
    [SerializeField] float startDelay;
    [PlaylistElementName()]
    [SerializeField]
    protected Playlist[] playlist;
    protected Dictionary<string, Action<Playlist, Timeline>> bind = new Dictionary<string, Action<Playlist, Timeline>>();

    AudioSource musicManager;
    Coroutine[] coroutineArray;

    [ContextMenu("Sort Timeline Arrays by Time")]
    public void SortTimelineArraysByTime()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].sortTimeline();
        }
    }

    public abstract void bindPatternAction();

    void Awake()
    {
        init();
    }

    void OnEnable()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        musicManager.clip = BGM;
        Invoke("runPlaylist", startDelay);
    }

    void Update()
    {
        if (musicManager.isPlaying) 
        {
            int i;
            for (i = 0; i < coroutineArray.Length; i++)
            {
                if (coroutineArray[i] != null)
                    break;
            }
            if (i == coroutineArray.Length)
                Destroy(gameObject);
        }
        
    }

    public void init()
    {
        SortTimelineArraysByTime();
        bindPatternAction();
        setPatternAction();
    }

    public void setPatternAction()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].defineAction(bind[playlist[i].ToString()]);
        }
    }

    private void runPlaylist()
    {
        coroutineArray = new Coroutine[playlist.Length];

        for (int i = 0; i < playlist.Length; i++)
        {
            coroutineArray[i] = StartCoroutine(playlist[i].Run());
        }
        musicManager.Play();
    }
}
