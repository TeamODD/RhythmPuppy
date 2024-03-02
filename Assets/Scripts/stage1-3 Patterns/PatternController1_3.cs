using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject pattern15;
    [SerializeField]
    float DelayTime;
    [SerializeField]
    GameObject Boss;
    [SerializeField]
    GameObject BlurEffect;
    [Space(20)]
    [SerializeField]
    GameObject shinyEffect;
    [SerializeField]
    AudioClip shinySoundEffect;
    [SerializeField]
    GameObject soundPlayerPrefab;

    bool isPuppyShown;

    private List<float> pattern11Timings = new List<float>
    {
    0.3f, 0.7f, 2.2f, 3.8f, 5.4f, 7.0f, 8.6f, 10.2f, 11.8f, 13.4f, 15.0f, 16.6f, 18.2f, 19.8f, 21.4f, 23.0f, 24.6f, 26.2f, 27.8f, 29.4f, 31.0f, 32.6f, 34.2f, 35.8f, 37.4f, 39.0f, 40.6f, 42.2f, 43.8f, 45.4f, 47.0f, 48.6f, 50.2f, 51.8f, 53.4f, 55.0f, 56.6f, 58.2f, 59.8f, 61.4f, 63.0f, 64.6f, 66.2f, 67.8f, 69.4f, 71.0f, 72.6f, 74.2f, 75.8f, 77.4f, 79.0f, 80.6f, 82.2f, 83.8f, 85.4f, 87.0f, 88.6f, 90.2f, 91.8f, 93.4f, 95.0f, 96.6f, 98.2f
    };

    private List<float> pattern12Timings = new List<float>
    {
    12.0f, 17.5f, 62.1f, 68.7f
    };

    private List<float> pattern13Timings = new List<float>
    {
    16.3f, 21.7f, 66.5f, 72.9f
    };

    private List<float> pattern15Timings = new List<float>
    {
        24.9f, 26.2f, 27.8f, 28.5f, 29.4f, 30.2f, 31.1f, 32.6f,
        34.2f, 35.0f, 35.8f, 36.7f, 37.6f, 38.6f, 40.2f, 40.9f,
        41.7f, 42.5f, 43.3f, 45.4f, 46.5f, 47.3f, 48.1f, 48.9f,
        75.8f, 77.4f, 79.0f, 79.7f, 80.6f, 81.4f, 82.3f, 83.5f,
        85.4f, 86.2f, 87.0f, 87.9f, 88.8f, 90.3f, 91.9f, 92.6f,
        93.4f, 94.2f, 95.0f, 96.6f, 98.2f, 99.0f, 99.8f
    };


    private List<float> BlurTimings = new List<float>
    {
        9.0f, 59.1f
    };

    private float startTime;
    EventManager eventManager;
    Transform musicManager;

    private void Start()
    {
        isPuppyShown = false;
        audioSource.clip = music;
        audioSource.Stop();
        eventManager = FindObjectOfType<EventManager>();
        musicManager = GameObject.FindWithTag("MusicManager").transform;
        eventManager.savePointTime = savePointTime;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.stageEvent.gameStartEvent += run;
        eventManager.playerEvent.reviveEvent += run;

        eventManager.stageEvent.gameStartEvent();

        pattern11.SetActive(false);
        pattern12.SetActive(false);
        pattern13.SetActive(false);
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
        StartCoroutine(RunPattern15());
        StartCoroutine(GameClear());
        StartCoroutine(RunBlurEffect());
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
        yield return new WaitForSeconds(100f - startTime);
        Rigidbody2D BossRigidbody2D = Boss.GetComponent<Rigidbody2D>();
        BossRigidbody2D.velocity = Vector2.down * 3f;
        yield return new WaitUntil(() => Boss.transform.position.y < -8f);
        BossRigidbody2D.velocity = Vector2.zero;
    }

    private IEnumerator RunBlurEffect()
    {
        BlurEffect.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        for (int i = 0; i < BlurTimings.Count; i++)
        {
            float timing = BlurTimings[i];

            if (timing < startTime) //안개 10초에 시작, 12초 완성, 24.9초에 종료, 26.9초에 사라짐 //24.9초에 첫 세이브
            {
                continue;
            }
            yield return new WaitForSeconds(timing - audioSource.time + DelayTime);

            // 패턴 실행 전, 별 이펙트 및 사운드 출력
            playSoundEffect(shinySoundEffect);
            shinyEffect.SetActive(true);

            // 패턴 실행
            yield return new WaitForSeconds(1f);
            if (i == 0)
                StartCoroutine(Fading(BlurEffect, 0f, 255f, 2f, 12.9f));
            else if (i == 1)
                StartCoroutine(Fading(BlurEffect, 0f, 255f, 2f, 13.7f));
        }
    }

    private IEnumerator Fading(GameObject obj, float initialAlpha, float finalAlpha, float fadeDuration, float holdingtime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //최종 투명도값과 초기 투명도값을 바꿔 작성한 게 맞음.

            // 0에서 255 사이의 값으로 투명도 제한
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            Image image = obj.GetComponent<Image>();

            Color color = image.color;

            // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // 투명도 값 변경
            image.color = color; // 변경된 투명도 설정

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(holdingtime);

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(finalAlpha, initialAlpha, elapsedTime / fadeDuration); //최종 투명도값과 초기 투명도값을 바꿔 작성한 게 맞음.

            // 0에서 255 사이의 값으로 투명도 제한
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            Image image = obj.GetComponent<Image>();

            Color color = image.color;

            // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // 투명도 값 변경
            image.color = color; // 변경된 투명도 설정

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void playSoundEffect(AudioClip sound)
    {
        GameObject o = Instantiate(soundPlayerPrefab);
        AudioSource audioSource = o.GetComponent<AudioSource>();
        o.transform.SetParent(musicManager);
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void stopAllSoundEffects()
    {
        foreach (Transform child in musicManager)
            Destroy(child.gameObject);
    }
}
