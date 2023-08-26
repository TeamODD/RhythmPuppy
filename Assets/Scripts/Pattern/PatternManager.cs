using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;
using Cysharp.Threading.Tasks;

public class PatternManager : MonoBehaviour
{
    [SerializeField] AudioClip BGM;
    [SerializeField] float[] savePointTime;
    [SerializeField] PatternPlaylist[] playlist;
    [SerializeField] float startDelayTime;

    [HideInInspector] 
    public EventManager eventManager;
    [HideInInspector]
    public Transform obstacleManager;
    AudioSource audioSource;
    Coroutine[] playlistCoroutine;

    void Awake()
    {
        init().Forget();
    }

    private async UniTask init()
    {
        eventManager = FindObjectOfType<EventManager>();
        audioSource = FindObjectOfType<AudioSource>();
        obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager").transform;

        if (audioSource.clip == null)
            audioSource.clip = BGM;
        eventManager.savePointTime = savePointTime;

        eventManager.deathEvent += deathEvent;
        eventManager.reviveEvent += reviveEvent;

        SortTimelineArraysByTime();

        await UniTask.Delay(System.TimeSpan.FromSeconds(startDelayTime));
        run();
    }

    private void deathEvent()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < obstacleManager.childCount; i++)
        {
            Destroy(obstacleManager.GetChild(i).gameObject);
        }
    }

    private void reviveEvent()
    {
        run();
    }

    [ContextMenu("Sort Timeline Arrays by Time")]
    public void SortTimelineArraysByTime()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            playlist[i].sortTimeline();
        }
    }

    private void run()
    {
        playlistCoroutine = new Coroutine[playlist.Length];
        float t = audioSource.time;

        for (int i = 0; i < playlist.Length; i++)
        {
            playlistCoroutine[i] = StartCoroutine(playlist[i].Run(t));
        }
        playMusic().Forget();
    }

    private async UniTask playMusic()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(startDelayTime));
        audioSource.Play();
    }
}