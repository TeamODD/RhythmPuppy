using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerEvent;

public class PatternController1_3 : MonoBehaviour
{
    [SerializeField]
    AudioClip music;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    float[] savePointTime;
    [SerializeField]
    GameObject pattern11;
    [SerializeField]
    GameObject pattern12;
    [SerializeField]
    GameObject pattern13;
    [SerializeField]
    GameObject pattern14;
    [SerializeField]
    GameObject pattern15;
    bool isPuppyShown;

    private List<float> pattern11Timings = new List<float>
    {
    0.3f, 1.9f, 3.5f, 5.1f, 6.7f, 8.3f, 9.9f, 11.5f, 13.1f, 
    14.7f, 16.3f, 17.9f, 19.5f, 21.1f, 22.7f, 24.3f, 25.9f,
    27.5f, 29.1f, 30.7f, 32.3f, 33.9f, 35.5f, 37.1f, 38.7f,
    40.3f, 41.9f, 43.5f, 45.1f, 46.7f, 48.3f, 49.9f, 51.5f,
    53.1f, 54.7f, 56.3f, 57.9f, 59.5f, 61.1f, 62.7f, 64.3f,
    65.9f, 67.5f, 69.1f, 70.7f, 72.3f, 73.9f, 75.5f, 77.1f,
    78.7f, 80.3f, 81.9f, 83.5f, 85.1f, 86.7f, 88.3f, 89.9f,
    91.5f, 93.1f, 94.7f, 96.3f, 97.9f, 99.5f, 101.1f
    };

    private List<float> pattern12Timings = new List<float>
    {
    0.3f, 6.7f, 13.1f, 19.5f, 25.9f, 32.3f, 38.7f, 45.1f, 
    51.5f, 57.9f, 64.3f, 70.7f, 77.1f, 83.5f, 89.9f, 96.3f
    };

    private List<float> pattern13Timings = new List<float>
    {
    3.5f, 9.9f, 16.3f, 22.7f, 29.1f, 35.5f, 41.9f, 48.3f,
    54.7f, 61.1f, 67.5f, 73.9f, 80.3f, 86.7f, 93.1f, 99.5f
    };

    private List<float> pattern14Timings = new List<float>
    {

    };

    private List<float> pattern15Timings = new List<float>
    {

    };

    private float startTime;
    EventManager eventManager;

    private void Start()
    {
        isPuppyShown = false;
        eventManager = FindObjectOfType<EventManager>();
        audioSource.clip = music;
        eventManager.savePointTime = savePointTime;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.stageEvent.gameStartEvent += run;
        eventManager.playerEvent.reviveEvent += run;

        eventManager.stageEvent.gameStartEvent();

        pattern11.SetActive(false);
        pattern12.SetActive(false);
        pattern13.SetActive(false);
        pattern14.SetActive(false);
        pattern15.SetActive(false);
    }

    void run()
    {
        startTime = audioSource.time;

        // 추가 패턴 GameObject 변수들에 대해도 필요에 따라 비활성화 처리
        StartCoroutine(RunPattern11());
        StartCoroutine(RunPattern12());
        StartCoroutine(RunPattern13());
        StartCoroutine(RunPattern14());
        StartCoroutine(RunPattern15());
    }

    private void Update()
    {
        if (!isPuppyShown && audioSource.clip.length - 2f < audioSource.time)
        {
            isPuppyShown = true;
            GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
        }
    }

    void deathEvent()
    {
        StopAllCoroutines();
    }

    private IEnumerator RunPattern11()
    {
        for (int i = 0; i < pattern11Timings.Count; i++)
        {
            float timing = pattern11Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            yield return new WaitForSeconds(timing - audioSource.time);

            // 패턴을 복제하고 활성화
            GameObject newPattern11 = Instantiate(pattern11, pattern11.transform.position, pattern11.transform.rotation);
            newPattern11.SetActive(true);
        }
    }

    private IEnumerator RunPattern12()
    {
        for (int i = 0; i < pattern12Timings.Count; i++)
        {
            float timing = pattern12Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            yield return new WaitForSeconds(timing - audioSource.time);

            // 패턴을 복제하고 활성화
            GameObject newPattern12 = Instantiate(pattern12, pattern12.transform.position, pattern12.transform.rotation);
            newPattern12.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern13()
    {
        for (int i = 0; i < pattern13Timings.Count; i++)
        {
            float timing = pattern13Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            yield return new WaitForSeconds(timing - audioSource.time);

            // 패턴을 복제하고 활성화
            GameObject newPattern13 = Instantiate(pattern13, pattern13.transform.position, pattern13.transform.rotation);
            newPattern13.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern14()
    {
        for (int i = 0; i < pattern14Timings.Count; i++)
        {
            float timing = pattern14Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            yield return new WaitForSeconds(timing - audioSource.time);

            // 패턴을 복제하고 활성화
            GameObject newPattern14 = Instantiate(pattern14, pattern14.transform.position, pattern14.transform.rotation);
            newPattern14.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern15()
    {
        for (int i = 0; i < pattern15Timings.Count; i++)
        {
            float timing = pattern15Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            yield return new WaitForSeconds(timing - audioSource.time);

            // 패턴을 복제하고 활성화
            GameObject newPattern15 = Instantiate(pattern15, pattern15.transform.position, pattern15.transform.rotation);
            newPattern15.SetActive(true);
        }
        yield return null;
    }
}
