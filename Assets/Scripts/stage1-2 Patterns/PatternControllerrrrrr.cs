using Cysharp.Threading.Tasks;
using EventManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatternControllerrrrrr : MonoBehaviour
{
    [SerializeField]
    AudioClip music;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    float[] savePointTime;
    [SerializeField]
    private GameObject pattern6;
    [SerializeField]
    private GameObject pattern7a;
    [SerializeField]
    private GameObject pattern7b;
    [SerializeField]
    private GameObject pattern8a;
    [SerializeField]
    private GameObject pattern8b;
    [SerializeField]
    private GameObject pattern8c;
    [SerializeField]
    private GameObject pattern9;
    [SerializeField]
    private GameObject pattern10;
    [SerializeField]
    float DelayTime;
    [SerializeField]
    GameObject MainCamera;

    bool isPuppyShown;
    private float startTime;
    EventManager eventManager;
    CameraShake Camera;

    private List<float> pattern6Timings = new List<float>
    {
        0.4f, 1.4f, 2.4f, 3.4f,
        4.6f, 5.6f, 6.6f, 7.6f,
        8.8f, 9.8f, 10.8f, 11.8f,
        12.9f, 13.9f, 14.9f, 15.9f,
        17.1f, 18.1f, 19.1f, 20.1f,
        21.3f, 22.3f, 23.3f, 24.3f,
        25.4f, 26.4f, 27.5f, 28.5f,
        29.6f, 30.6f,
        68.7f, 69.7f, 70.7f, 71.7f, 72.9f, 73.9f, 74.9f, 75.9f, 77.1f, 78.1f, 79.1f, 80.1f, 81.2f, 82.2f, 83.2f, 84.2f, 85.3f,
        85.9f, 86.9f, 87.9f, 88.9f,
        90.1f, 91.1f, 92.1f, 93.1f,
        94.2f, 95.2f, 96.3f, 97.3f,
        98.4f
    };

    private List<float> pattern7aTimings = new List<float>
    {
        0f, 4.1f, 8.2f,
        68.8f, 72.9f, 77.0f
    };

    private List<float> pattern7bTimings = new List<float>
    {
        12.3f, 81.1f
    };

    private List<float> pattern8aTimings = new List<float>
    {
        16.5f, 20.7f, 24.9f,
        85.3f, 89.5f, 93.7f,
    };

    private List<float> pattern8bTimings = new List<float>
    {
        29.1f, 97.9f
    };

    private List<float> pattern8cTimings = new List<float>
    {
        33.3f, 37.5f, 41.7f, 45.9f, 50.1f, 54.3f, 58.5f, 62.7f,
        102.1f, 106.3f, 110.5f, 114.7f, 118.9f, 123.1f, 127.3f, 131.5f
    };

    private List<float> pattern9Timings = new List<float>
    {
        33.3f, 33.8f, 34.3f, 34.8f, 35.3f, 35.8f, 36.3f, 36.8f,
        37.4f, 37.9f, 38.4f, 38.9f, 39.5f, 40.0f, 40.5f, 41.0f,
        47.3f, 50.5f, 58.8f,
        102.1f, 102.6f, 103.1f, 103.6f, 104.1f, 104.6f, 105.1f, 105.6f, 106.2f, 106.7f, 107.2f, 107.7f, 108.3f, 108.8f, 109.3f, 109.8f, 116.1f, 119.3f, 127.6f
    };

    private List<float> pattern10Timings = new List<float>
    {
    42.1f, 43.1f, 44.1f, 45.3f, 46.3f, 51.0f, 52.0f, 53.0f, 54.0f, 55.2f, 56.2f, 57.2f, 59.3f, 60.3f, 61.4f, 62.4f, 63.4f, 64.4f,
    110.9f, 111.9f, 112.9f, 114.1f, 115.1f, 119.8f, 120.8f, 121.8f, 122.8f, 123.9f, 124.9f, 125.9f, 128.1f, 129.1f, 130.2f, 131.2f, 132.2f, 133.2f
    };

    private List<float> camerashakingTimings = new List<float>
    {
        42.6f, 43.6f, 44.6f, 45.8f, 46.8f, 51.5f, 52.5f, 53.5f, 54.5f, 55.7f, 56.7f, 57.7f, 59.8f, 60.8f, 61.9f, 62.9f, 63.9f, 64.9f,
        111.4f, 112.4f, 113.4f, 114.6f, 115.6f, 120.3f, 121.3f, 122.3f, 123.3f, 124.4f, 125.4f, 126.4f, 128.6f, 129.6f, 130.7f, 131.7f, 132.7f, 133.7f
    };

    private void Start()
    {
        Camera = MainCamera.GetComponent<CameraShake>();
        isPuppyShown = false;
        eventManager = FindObjectOfType<EventManager>();
        audioSource.clip = music;
        eventManager.savePointTime = savePointTime;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.stageEvent.gameStartEvent += run;
        eventManager.playerEvent.reviveEvent += run;

        eventManager.stageEvent.gameStartEvent();

        // 패턴1, 패턴2, 패턴3 스크립트를 비활성화
        pattern6.SetActive(false);
        pattern7a.SetActive(false);
        pattern7b.SetActive(false);
        pattern8a.SetActive(false);
        pattern8b.SetActive(false);
        pattern8c.SetActive(false);
        pattern9.SetActive(false);
        pattern10.SetActive(false);
    }

    void run()
    {
        startTime = audioSource.time; //현재 음악 시간

        // 추가 패턴 GameObject 변수들에 대해도 필요에 따라 비활성화 처리

        StartCoroutine(Pattern6Timing());
        StartCoroutine(Pattern7aTiming());
        StartCoroutine(Pattern7bTiming());
        StartCoroutine(Pattern8aTiming());
        StartCoroutine(Pattern8bTiming());
        StartCoroutine(Pattern8cTiming());
        StartCoroutine(Pattern9Timing());
        StartCoroutine(Pattern10Timing());
        StartCoroutine(CameraShakingTiming());
    }

    void Update()
    {
        if (!isPuppyShown && audioSource.clip.length - 2f < audioSource.time)
        {
            isPuppyShown = true;
            GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
        }
        //Debug.Log(Time.time);
    }

    private IEnumerator Pattern6Timing()
    {
        for (int i = 0; i < pattern6Timings.Count; i++)
        {
            float timing = pattern6Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern6(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern6(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern6 = Instantiate(pattern6, pattern6.transform.position, pattern6.transform.rotation);
        newPattern6.SetActive(true);
    }

    private IEnumerator Pattern7aTiming()
    {
        for (int i = 0; i < pattern7aTimings.Count; i++)
        {
            float timing = pattern7aTimings[i];

            if (timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern7a(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern7a(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern7a = Instantiate(pattern7a, pattern7a.transform.position, pattern7a.transform.rotation);
        newPattern7a.SetActive(true);
    }

    private IEnumerator Pattern7bTiming()
    {
        for (int i = 0; i < pattern7bTimings.Count; i++)
        {
            float timing = pattern7bTimings[i];

            if (timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern7b(timing));
            yield return null;            
        }
    }

    private IEnumerator RunPattern7b(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern7b = Instantiate(pattern7b, pattern7b.transform.position, pattern7b.transform.rotation);
        newPattern7b.SetActive(true);
    }

    private IEnumerator Pattern8aTiming()
    {
        for (int i = 0; i < pattern8aTimings.Count; i++)
        {
            float timing = pattern8aTimings[i];

            if ( timing < startTime)
            {
                continue;
            }

            StartCoroutine (RunPattern8a(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern8a(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern8a = Instantiate(pattern8a, pattern8a.transform.position, pattern8a.transform.rotation);
        newPattern8a.SetActive(true);
    }

    private IEnumerator Pattern8bTiming()
    {
        for (int i = 0; i < pattern8bTimings.Count; i++)
        {
            float timing = pattern8bTimings[i];

            if ( timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern8b(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern8b(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern8b = Instantiate(pattern8b, pattern8b.transform.position, pattern8b.transform.rotation);
        newPattern8b.SetActive(true);
    }

    private IEnumerator Pattern8cTiming()
    {
        for (int i = 0; i < pattern8cTimings.Count; i++)
        {
            float timing = pattern8cTimings[i];

            if (timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern8c(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern8c(float timing) 
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern8c = Instantiate(pattern8c, pattern8c.transform.position, pattern8c.transform.rotation);
        newPattern8c.SetActive(true);
    }

    private IEnumerator Pattern9Timing()
    {
        for (int i = 0; i < pattern9Timings.Count; i++)
        {
            float timing = pattern9Timings[i];

            if (timing < startTime)
            {
                continue;
            }

            StartCoroutine(RunPattern9(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern9(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern9 = Instantiate(pattern9, pattern9.transform.position, pattern9.transform.rotation);
        newPattern9.SetActive(true);
    }

    private IEnumerator Pattern10Timing()
    {
        for (int i = 0; i < pattern10Timings.Count; i++)
        {
            float timing = pattern10Timings[i];

            if (timing < startTime)
            {
                continue;
            }
            
            StartCoroutine(RunPattern10(timing));
            yield return null;
        }
    }

    private IEnumerator RunPattern10(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        // 패턴을 복제하고 활성화
        GameObject newPattern10 = Instantiate(pattern10, pattern10.transform.position, pattern10.transform.rotation);
        newPattern10.SetActive(true);
    }

    private IEnumerator CameraShakingTiming()
    {
        for (int i = 0; i < camerashakingTimings.Count; i++)
        {
            float timing = camerashakingTimings[i];

            if (timing < startTime)
            {
                continue;
            }
            StartCoroutine(CameraShaking(timing));
            yield return null;
        }
    }

    private IEnumerator CameraShaking(float timing)
    {
        yield return new WaitForSeconds(timing - audioSource.time + DelayTime);
        Camera.ShakeAmount = 0.2f;
        Camera.VibrateForTime(0.1f);
    }

    void deathEvent()
    {
        StopAllCoroutines();
    }
}