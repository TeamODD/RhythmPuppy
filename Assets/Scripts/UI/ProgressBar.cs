using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventManager;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameObject emptySavePointPrefab;
    [SerializeField] GameObject fullSavePointPrefab;

    AudioSource musicAudioSource;

    EventManager eventManager;
    Image fillImage;
    GameObject playerbudge, puppybudge;
    WaitUntil w;
    Vector3 initialPlayerBudgePosition;
    GameObject[] emptySavePoint, fullSavePoint;
    Coroutine initCoroutine;

    void Awake()
    {
        initCoroutine = StartCoroutine(init());
    }

    private IEnumerator init()
    {
        eventManager = FindObjectOfType<EventManager>();
        fillImage = transform.Find("GameProgressGuage").GetComponent<Image>();
        playerbudge = transform.Find("MovingPlayerBudge").gameObject;
        puppybudge = transform.Find("PuppyBudge").gameObject;
        musicAudioSource = FindObjectOfType<AudioSource>();
        w = new WaitUntil(() => musicAudioSource.clip != null);
        initialPlayerBudgePosition = playerbudge.transform.position;

        eventManager.gameStartEvent += gameStartEvent;
        eventManager.deathEvent += deathEvent;
        eventManager.rewindEvent += rewindEvent;
        eventManager.reviveEvent += gameStartEvent;

        fillImage.fillAmount = 0;
        yield return w;
        emptySavePoint = new GameObject[eventManager.savePointTime.Length];
        fullSavePoint = new GameObject[eventManager.savePointTime.Length];
        for (int i = 0; i < eventManager.savePointTime.Length; i++)
        {
            float normalizedPosition = Mathf.Clamp01(eventManager.savePointTime[i] / musicAudioSource.clip.length);
            float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, puppybudge.transform.position.x, normalizedPosition);
            Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);

            emptySavePoint[i] = Instantiate(emptySavePointPrefab);
            emptySavePoint[i].transform.SetParent(transform);
            emptySavePoint[i].transform.localScale = Vector3.one;
            emptySavePoint[i].SetActive(true);
            emptySavePoint[i].transform.position = newPosition;
            fullSavePoint[i] = Instantiate(fullSavePointPrefab);
            fullSavePoint[i].transform.SetParent(transform);
            fullSavePoint[i].transform.localScale = Vector3.one;
            fullSavePoint[i].SetActive(false);
            fullSavePoint[i].transform.position = newPosition;
        }
        initCoroutine = null;
    }

    void Update()
    {
        if (initCoroutine != null) return;

        if (musicAudioSource.clip.length - 0.7f < musicAudioSource.time)
        {
            MovePlayerBudge(musicAudioSource.clip.length);
            SavePointChecking(1);
            musicAudioSource.Pause();
            musicAudioSource.time = musicAudioSource.clip.length - 0.1f;
            return;
        }

        float currentMusicPosition = musicAudioSource.time;
        float fillAmount = currentMusicPosition / musicAudioSource.clip.length;
        fillImage.fillAmount = fillAmount;

        if (1 <= fillAmount)
        {
            fillAmount = 1;
        }

        MovePlayerBudge(currentMusicPosition);
        SavePointChecking(fillAmount);
    }


    private void MovePlayerBudge(float currentMusicPosition)
    {
        float normalizedPosition = Mathf.Clamp01(currentMusicPosition / musicAudioSource.clip.length);
        float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, puppybudge.transform.position.x, normalizedPosition);
        Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);

        playerbudge.transform.position = newPosition;

        if (1 <= normalizedPosition)
        {
            playerbudge.transform.position = new Vector3(puppybudge.transform.position.x, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);
        }
    }

    private void SavePointChecking(float fillAmount)
    {
        for (int i = 0; i < emptySavePoint.Length; i++)
        {
            if (eventManager.savePointTime[i] <= musicAudioSource.time)
            {
                emptySavePoint[i].SetActive(false);
                fullSavePoint[i].SetActive(true);
            }
            else
            {
                emptySavePoint[i].SetActive(true);
                fullSavePoint[i].SetActive(false);
            }
        }
    }

    private void gameStartEvent()
    {
        playMusic().Forget();
    }

    private async UniTask playMusic()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(1));
        musicAudioSource.Play();
    }

    private void rewindEvent()
    {
        for (int i = 0; i < eventManager.savePointTime.Length; i++)
        {
            if (eventManager.savePointTime[i] <= musicAudioSource.time) continue;

            if (i == 0)
                musicAudioSource.time = 0;
            else
                musicAudioSource.time = eventManager.savePointTime[i-1];
            return;
        }
        musicAudioSource.time = eventManager.savePointTime[eventManager.savePointTime.Length-1];
    }

    private void deathEvent()
    {
        musicAudioSource.Pause();
    }
}