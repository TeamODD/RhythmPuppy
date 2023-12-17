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
    [SerializeField]
    float DelayTime;
    [SerializeField]
    GameObject Boss;

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
    13.1f, 19.5f, 64.3f, 70.7f
    };

    private List<float> pattern13Timings = new List<float>
    {
    16.3f, 22.7f, 67.5f, 73.9f
    };

    private List<float> pattern14Timings = new List<float>
    {

    };

    private List<float> pattern15Timings = new List<float>
    {
        25.9f, 27.4f, 29f, 29.7f, 30.6f, 31.4f, 32.3f, 33.8f,
        35.4f, 36.2f, 37f, 37.9f, 38.8f, 40.3f, 41.9f, 42.6f, 
        43.4f, 44.2f, 45f, 46.6f, 48.2f, 49f, 49.8f, 50.6f, 
        77f, 78.6f, 80.2f, 80.9f, 81.8f, 82.6f, 83.5f, 85.0f,
        86.6f, 87.4f, 88.2f, 89.1f, 90.0f, 91.5f, 93.1f, 93.8f,
        94.6f, 95.4f, 96.2f, 97.8f, 99.4f, 100.2f, 101.0f, 101.8f
    };

    private float startTime;
    EventManager eventManager;

    private void Start()
    {
        isPuppyShown = false;
        audioSource.clip = music;
        audioSource.Stop();
        eventManager = FindObjectOfType<EventManager>();
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

        StartCoroutine(StartMusicWithDelay());
    }

    private IEnumerator StartMusicWithDelay()
    {
        yield return new WaitForSeconds(DelayTime);
        //audioSource.Play();
        // 여기서 다른 오디오 설정이나 재생을 수행할 수 있습니다.
    }

    void run()
    {
        startTime = audioSource.time - DelayTime;

        // 추가 패턴 GameObject 변수들에 대해도 필요에 따라 비활성화 처리
        StartCoroutine(RunPattern11());
        StartCoroutine(RunPattern12());
        StartCoroutine(RunPattern13());
        StartCoroutine(RunPattern14());
        StartCoroutine(RunPattern15());
        StartCoroutine(GameClear());
    }

    private void Update()
    {
        if (!isPuppyShown && audioSource.clip.length - 2f < audioSource.time)
        {
            isPuppyShown = true;
            GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
        }//2분 38초 보스 사망?
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

            yield return new WaitForSeconds(timing - audioSource.time + DelayTime);

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

            yield return new WaitForSeconds(timing - audioSource.time + DelayTime);

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

            yield return new WaitForSeconds(timing - audioSource.time + DelayTime);

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

            yield return new WaitForSeconds(timing - audioSource.time + 3f);

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

            yield return new WaitForSeconds(timing - audioSource.time + DelayTime);

            // 패턴을 복제하고 활성화
            GameObject newPattern15 = Instantiate(pattern15, pattern15.transform.position, pattern15.transform.rotation);
            newPattern15.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator GameClear()
    {
        yield return new WaitForSeconds(2 * 60 + 38f);
        Rigidbody2D BossRigidbody2D = Boss.GetComponent<Rigidbody2D>();
        BossRigidbody2D.velocity = Vector2.down * 3f;
        yield return new WaitUntil(() => Boss.transform.position.y < -8f);
        BossRigidbody2D.velocity = Vector2.zero;
    }
}
