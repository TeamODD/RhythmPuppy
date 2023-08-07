using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using World_2;

public abstract class PatternBase : MonoBehaviour
{
    [SerializeField] AudioClip BGM;

    [SerializeField] 
    protected GameObject[] patternList;

    [PatternArrayElementTitle()]
    [SerializeField]
    protected Playlist[] playlist;

    AudioSource musicManager;

    [ContextMenu("Sort Timeline Arrays by Time")]
    public void SortTimelineArraysByTime()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].sortTimeline();
        }
    }

    public abstract void definePatternAction(Playlist p, Timeline t);

    void OnEnable()
    {
        setPatternAction();
    }

    void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        musicManager.clip = BGM;
        runPlaylist();
    }

    public void setPatternAction()
    {
        foreach (Playlist p in playlist)
        {
            foreach (Timeline t in p.timeline)
            {
                definePatternAction(p, t);
                /*t.defineAction(getPatternAction(p, t));*/
            }
        }
    }

    private void runPlaylist()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            StartCoroutine(playlist[i].Run());
        }
        musicManager.Play();
        Destroy(gameObject);
    }
}
