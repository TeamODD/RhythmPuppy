using System;
using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;

public abstract class PatternBase : MonoBehaviour
{
    [SerializeField] float[] savePointTime;
    [SerializeField] AudioClip BGM;
    [SerializeField] float startDelay;
    [PlaylistElementName()]
    [SerializeField]
    protected Playlist[] playlist;
    protected Dictionary<string, Action<Playlist, Timeline>> bind = new Dictionary<string, Action<Playlist, Timeline>>();

    EventManager eventManager;
    AudioSource audioSource;
    Coroutine[] coroutineArray;


    public abstract void bindPatternAction();

    void Awake()
    {
        init();
    }

    public void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource.clip == null)
            audioSource.clip = BGM;
        eventManager.savePointTime = savePointTime;

        SortTimelineArraysByTime();
        bindPatternAction();
        setPatternAction();

        runPlaylist();
    }

    void Update()
    {
        if (!audioSource.isPlaying) return;

        int i;
        for (i = 0; i < coroutineArray.Length; i++)
        {
            if (coroutineArray[i] != null)
                break;
        }
        if (i == coroutineArray.Length)
            Destroy(gameObject);
    }

    public void setPatternAction()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].defineAction(bind[playlist[i].ToString()]);
        }
    }

    [ContextMenu("Sort Timeline Arrays by Time")]
    public void SortTimelineArraysByTime()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].sortTimeline();
        }
    }
    private void runPlaylist()
    {
        coroutineArray = new Coroutine[playlist.Length];
        float t = audioSource.time;

        for (int i = 0; i < playlist.Length; i++)
        {
            coroutineArray[i] = StartCoroutine(playlist[i].Run(t));
        }
        StartCoroutine(playMusic());
    }

    private IEnumerator playMusic()
    {
        yield return new WaitForSeconds(1f);
        audioSource.Play();
    }
}
