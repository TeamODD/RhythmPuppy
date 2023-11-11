using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Patterns;
using Cysharp.Threading.Tasks;
using Stage_2;
using UnityEngine.SceneManagement;
using EventManagement;

public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject stagePrefab;
    public float[] savePointTime;
    [SerializeField] float startDelayTime;
    [SerializeField] AudioClip music;
    [SerializeField] AudioSource audioSource;

    [HideInInspector] 
    public EventManager eventManager;
    GameObject stage;
    List<Coroutine> coroutineList;
    bool isPuppyShown;

    void Awake()
    {
        StartCoroutine(init());
    }

    private IEnumerator init()
    {
        isPuppyShown = false;
        eventManager = FindObjectOfType<EventManager>();
        audioSource = FindObjectOfType<AudioSource>();
        if (stage != null) Destroy(stage);
        stage = null;
        audioSource.clip = music;

        eventManager.stageEvent.gameStartEvent += gameStartEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += gameStartEvent;
        eventManager.savePointTime = savePointTime;

        yield return new WaitForSeconds(startDelayTime);
        eventManager.stageEvent.gameStartEvent();
    }

    void Update()
    {
        if (!isPuppyShown && audioSource.clip.length - 3f < audioSource.time)
        {
            isPuppyShown = true;
            GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
        }
    }

    private void gameStartEvent()
    {
        stage = Instantiate(stagePrefab);
        stage.transform.SetParent(transform);
        stage.SetActive(true);
    }

    private void deathEvent()
    {
        stage = null;
    }
}