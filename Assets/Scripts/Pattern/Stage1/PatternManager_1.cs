using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
    enum Type
    {
        pattern1_a,
        pattern1_b,
        pattern2_a,
        pattern2_b_2,
        pattern2_b_3,
        pattern3,
        pattern4,
    }

    [SerializeField]
    float[] savePointTime;
    [SerializeField] 
    AudioClip music;
    [SerializeField]
    private GameObject Bee_1_a;
    [SerializeField]
    private GameObject Warning_1_a;
    [SerializeField]
    private GameObject Bee_1_b;
    [SerializeField]
    private GameObject Warning_1_b;
    [SerializeField]
    private GameObject Piranha;
    [SerializeField]
    private GameObject Warning_3;
    [SerializeField]
    private GameObject Oak_2_a;
    [SerializeField]
    private GameObject Oak_2_b;
    [SerializeField]
    private GameObject Warning_2_a;
    [SerializeField]
    private GameObject Warning_2_b;
    [SerializeField]
    private GameObject Apple;
    [SerializeField]
    private GameObject Warning_4;


    Dictionary<Type, float> patternCount;
    EventManager eventManager;
    AudioSource audioSource;
    bool isPuppyShown;
    /*private int count_1_a;
    private int count_1_b;
    private int count_3;
    private int count_4;*/


    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        audioSource = FindObjectOfType<AudioSource>();
        patternCount = new Dictionary<Type, float>();
        patternCount[Type.pattern1_a] = 0;
        patternCount[Type.pattern1_b] = 0;
        patternCount[Type.pattern2_a] = 0;
        patternCount[Type.pattern2_b_2] = 0;
        patternCount[Type.pattern2_b_3] = 0;
        patternCount[Type.pattern3] = 0;
        patternCount[Type.pattern4] = 0;
        audioSource.clip = music;
        eventManager.savePointTime = savePointTime;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.stageEvent.gameStartEvent += run;
        eventManager.playerEvent.reviveEvent += run;
        /*
                count_1_a = 0;
                count_1_b = 0;
                count_3 = 0;
                count_4 = 0;*/
        
        eventManager.stageEvent.gameStartEvent();
    }

    void run()
    {
        isPuppyShown = false;
        float startTime = audioSource.time;

        StartCoroutine(Pattern1_a(4.0f, startTime));

        StartCoroutine(Pattern1_b(68f, startTime));

        StartCoroutine(Pattern2_a(4f, startTime));
        StartCoroutine(Pattern2_a(12f, startTime));
        StartCoroutine(Pattern2_a(20f, startTime));
        StartCoroutine(Pattern2_a(28f, startTime));
        StartCoroutine(Pattern2_a(36f, startTime));

        StartCoroutine(Pattern2_b_2(5.7f, startTime));
        StartCoroutine(Pattern2_b_2(21.7f, startTime));
        StartCoroutine(Pattern2_b_2(37.7f, startTime));
        StartCoroutine(Pattern2_b_2(86f, startTime));

        StartCoroutine(Pattern2_b_3(13.7f, startTime));
        StartCoroutine(Pattern2_b_3(29.7f, startTime));
        StartCoroutine(Pattern2_b_3(92f, startTime));
        StartCoroutine(Pattern2_b_3(98f, startTime));
        StartCoroutine(Pattern2_b_3(108f, startTime));
        StartCoroutine(Pattern2_b_3(114f, startTime));

        StartCoroutine(Pattern3(20.0f, startTime));
        StartCoroutine(Pattern3(84.0f, startTime));

        StartCoroutine(Pattern4(36f, startTime));
        StartCoroutine(Pattern4(44f, startTime));
        StartCoroutine(Pattern4(52f, startTime));
        StartCoroutine(Pattern4(60f, startTime));
        StartCoroutine(Pattern4(68f, startTime));
        StartCoroutine(Pattern4(76f, startTime));

        StartCoroutine(Pattern4(84f, startTime));
        StartCoroutine(Pattern4(92f, startTime));
        StartCoroutine(Pattern4(100f, startTime));
        StartCoroutine(Pattern4(108f, startTime));
        StartCoroutine(Pattern4(116f, startTime));
        /*GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc(120f, startTime);*/
    }

    void Update()
    {
        if (!isPuppyShown && audioSource.clip.length - 8f < audioSource.time)
        {
            isPuppyShown = true;
            GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
        }
    }

    IEnumerator Pattern1_a(float t, float startTime)
    {
        if (0 <= t - startTime)
        {
            yield return new WaitForSeconds(t - startTime);
            Instantiate(Bee_1_a);
            Instantiate(Warning_1_a);
        }

        float delayTime = 0.5f;
        WaitForSeconds delay_0_5 = new WaitForSeconds(delayTime);
        for (int i=1; i<32; i++)
        {
            if (0 <= t - startTime + i * delayTime)
            {
                Instantiate(Bee_1_a);
                Instantiate(Warning_1_a);
                yield return delay_0_5;
            }
        }
            /*patternCount[Type.pattern1_a]++;
        count_1_a++;*/
        /*StartCoroutine(Pattern1_a(0.5f));*/
    }
    IEnumerator Pattern1_b(float t, float startTime)
    {
        if (0 <= t - startTime)
        {
            yield return new WaitForSeconds(t - startTime);
            Instantiate(Bee_1_b);
            Instantiate(Warning_1_b);
        }

        float delayTime = 0.5f;
        WaitForSeconds delay_0_5 = new WaitForSeconds(delayTime);
        for (int i = 1; i < 28; i++)
        {
            if (0 <= t - startTime + i * delayTime)
            {
                Instantiate(Bee_1_b);
                Instantiate(Warning_1_b);
                yield return delay_0_5;
            }
            /* patternCount[Type.pattern1_b]++;
             count_1_b++;
             StartCoroutine(Pattern1_b(0.5f));*/
        }
    }
    IEnumerator Pattern2_a(float t, float startTime)
    {
        if (0 <= t - startTime)
        {
            yield return new WaitForSeconds(t - startTime);
            Instantiate(Warning_2_a);
            Instantiate(Oak_2_a);
        }
    }
    IEnumerator Pattern2_b_2(float waitTime, float startTime)
    {
        if (0 <= waitTime - startTime)
        {
            yield return new WaitForSeconds(waitTime - startTime);

            Instantiate(Warning_2_b);
            Instantiate(Oak_2_b);
        }

        if (0 <= waitTime - startTime + 2.0f)
        {
            yield return new WaitForSeconds(2.0f);

            Instantiate(Warning_2_b);
            Instantiate(Oak_2_b);
        }
    }
    IEnumerator Pattern2_b_3(float waitTime, float startTime)
    {
        if (0 <= waitTime - startTime)
        {
            yield return new WaitForSeconds(waitTime - startTime);
        }
        float delayTime = 2;
        WaitForSeconds delay = new WaitForSeconds(delayTime);
        for (int i = 1; i < 4; i++) //통나무가 2회 나오길래 i<3을 고쳤습니다. 
        {
            if (0 <= waitTime - startTime + i * delayTime)
            {
                Instantiate(Warning_2_b);
                Instantiate(Oak_2_b);

                yield return delay; //delay가 if문 첫 시작이라 2초 밀려서 수정했습니다. (나머지 패턴도)
            }
        }
    }
    IEnumerator Pattern3(float t, float startTime)
    {
        if (0 <= t - startTime)
        {
            yield return new WaitForSeconds(t - startTime);
            Instantiate(Warning_3);
            Instantiate(Piranha);
        }

        float delayTime = 1f;
        WaitForSeconds delay = new WaitForSeconds(delayTime);
        for (int i = 1; i < 32; i++)
        {
            if (0 <= t - startTime + i * delayTime)
            {
                Instantiate(Warning_3);
                Instantiate(Piranha);
                yield return delay;
            }
        }

       /* patternCount[Type.pattern3]++;
        count_3++;
        StartCoroutine(Pattern3(1));*/
    }
    IEnumerator Pattern4(float waitTime, float startTime)
    {
        WaitForSeconds delay_0_5 = new WaitForSeconds(0.5f);
        float currentTime = waitTime - startTime;
        if (0 <= currentTime)
        {
            yield return new WaitForSeconds(currentTime);
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5;
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5;
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5;
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.7f;
        if (0 <= currentTime)
        {
            yield return new WaitForSeconds(0.7f);
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 1;
        if (0 <= currentTime)
        {
            yield return new WaitForSeconds(1f); 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.5f;
        if (0 <= currentTime)
        {
            yield return delay_0_5; 
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
        currentTime += 0.1f;
        if (0 <= currentTime)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(Apple);
            Instantiate(Warning_4);
        }
    }

    private void deathEvent()
    {
        StopAllCoroutines();
    }



    /*void FixedUpdate()
    {
        //32회 실행시 호출 중단
        *//*if (count_1_a == 32)*/
        /*if (patternCount[Type.pattern1_a] >= 32)
        {
            *//*CancelInvoke("Pattern1_a");*//*
            StopCoroutine("Pattern1_a");
        }*/
        /*if (count_1_b == 28)*/
        /*if (patternCount[Type.pattern1_b] >= 28)
        {
            *//*CancelInvoke("Pattern1_b");*//*
            StopCoroutine("Pattern1_b");
        }*/
        /*if (count_3 == 32)*//*
        if (patternCount[Type.pattern3] >= 32)
        {
            *//*CancelInvoke("Pattern3");*//*
            StopCoroutine("Pattern3");
            //패턴1_b의 카운트 횟수를 통해 인보크 조절
            *//*if (count_1_b == 0)*//*
            if (patternCount[Type.pattern1_b].Equals(0))
            {
                *//*Invoke("Pattern3", 32f);*//*
                StartCoroutine(Pattern3(32, startTime));
                patternCount[Type.pattern3] = 0;
                count_3 = 0;
            }
        }
        *//*if (count_4 == 16)*//*
        if (patternCount[Type.pattern4] >= 16)
        {
            *//*CancelInvoke("Pattern4");*//*
            StopCoroutine("Pattern4");
            patternCount[Type.pattern4] = 1;
            count_4 = 1;
        }   
    }*/
}
