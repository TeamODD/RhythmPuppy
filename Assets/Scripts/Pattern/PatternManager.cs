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
    private enum StageType
    {
        Tutorial,
        Stage_1_1,
        Stage_1_2,
        Stage_2_1,
        Stage_2_2,
    }

    [SerializeField] StageType stageType;
    [SerializeField] Stage_2_1 stage_2_1;
    [SerializeField] float startDelayTime;
    [SerializeField] GameObject puppyPrefab;

    [HideInInspector] 
    public EventManager eventManager;
    PatternManager patternManager;
    AudioSource audioSource;
    List<Coroutine> coroutineList;
    GameObject puppy;

    void Awake()
    {
        init().Forget();
    }

    private async UniTask init()
    {
        eventManager = FindObjectOfType<EventManager>();
        patternManager = GetComponent<PatternManager>();
        audioSource = FindObjectOfType<AudioSource>();
        coroutineList = new List<Coroutine>();
        puppy = null;
        setStageInfo();


        eventManager.stageEvent.gameStartEvent += gameStartEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += gameStartEvent;

        await UniTask.Delay(System.TimeSpan.FromSeconds(startDelayTime));
        eventManager.stageEvent.gameStartEvent();
    }

    void Update()
    {
        if (puppy != null) return;
        if (audioSource.clip.length - 5f < audioSource.time)
        {
            puppy = Instantiate(puppyPrefab);
            puppy.SetActive(true);
        }
    }

    private void setStageInfo()
    {
        AudioClip clip = null;
        switch(stageType)
        {
            case StageType.Tutorial:
                break;
            case StageType.Stage_1_1:
                break;
            case StageType.Stage_1_2:
                break;
            case StageType.Stage_2_1:
                clip = stage_2_1.music;
                stage_2_1.init(patternManager, eventManager);
                eventManager.savePointTime = stage_2_1.savePointTime;
                break;
            case StageType.Stage_2_2:
                break;
        }
        audioSource.clip = clip;
    }

    private void gameStartEvent()
    {
        switch (stageType)
        {
            case StageType.Tutorial:
                break;
            case StageType.Stage_1_1:
                break;
            case StageType.Stage_1_2:
                break;
            case StageType.Stage_2_1:
                stage_2_1.Run(audioSource.time, out coroutineList);
                break;
            case StageType.Stage_2_2:
                break;
        }
    }

    private void deathEvent()
    {
        int i;
        for (i = 0; i < coroutineList.Count; i++)
        {
            StopCoroutine(coroutineList[i]);
        }
        coroutineList.Clear();
    }
}