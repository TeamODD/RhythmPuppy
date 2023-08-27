using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

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
        eventManager.deathEvent += deathEvent;
        eventManager.gameStartEvent += run;
        eventManager.reviveEvent += run;
/*
        count_1_a = 0;
        count_1_b = 0;
        count_3 = 0;
        count_4 = 0;*/

        eventManager.gameStartEvent();
    }

    void run()
    {
        //1초 빠르게 함수 호출(경고가 1초 동안 나오므로), 벌이 날아오는데 1초가 걸리도록 설정
        float startTime = audioSource.time;

        StartCoroutine(Pattern1_a(3.0f, startTime));

        StartCoroutine(Pattern1_b(67f, startTime));

        StartCoroutine(Pattern2_a(11f, startTime));
        StartCoroutine(Pattern2_a(19f, startTime));
        StartCoroutine(Pattern2_a(27f, startTime));
        StartCoroutine(Pattern2_a(35f, startTime));

        StartCoroutine(Pattern2_b_2(4.7f, startTime));
        StartCoroutine(Pattern2_b_2(20.7f, startTime));
        StartCoroutine(Pattern2_b_2(36.7f, startTime));
        StartCoroutine(Pattern2_b_2(85f, startTime));

        StartCoroutine(Pattern2_b_3(12.7f, startTime));
        StartCoroutine(Pattern2_b_3(28.7f, startTime));
        StartCoroutine(Pattern2_b_3(91f, startTime));
        StartCoroutine(Pattern2_b_3(97f, startTime));
        StartCoroutine(Pattern2_b_3(107f, startTime));
        StartCoroutine(Pattern2_b_3(113f, startTime));

        StartCoroutine(Pattern3(19.0f, startTime));
        StartCoroutine(Pattern3(84.0f, startTime));

        StartCoroutine(Pattern4(35f, startTime));
        StartCoroutine(Pattern4(43f, startTime));
        StartCoroutine(Pattern4(51f, startTime));
        StartCoroutine(Pattern4(59f, startTime));
        StartCoroutine(Pattern4(67f, startTime));
        StartCoroutine(Pattern4(75f, startTime));

        StartCoroutine(Pattern4(83f, startTime));
        StartCoroutine(Pattern4(91f, startTime));
        StartCoroutine(Pattern4(99f, startTime));
        StartCoroutine(Pattern4(107f, startTime));
        StartCoroutine(Pattern4(115f, startTime));
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
                yield return delay_0_5;
                Instantiate(Bee_1_a);
                Instantiate(Warning_1_a);
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
                yield return delay_0_5;
                Instantiate(Bee_1_b);
                Instantiate(Warning_1_b);
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
        for (int i = 1; i < 3; i++)
        {
            if (0 <= waitTime - startTime + i * delayTime)
            {
                yield return delay;

                Instantiate(Warning_2_b);
                Instantiate(Oak_2_b);
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
                yield return delay;
                Instantiate(Warning_3);
                Instantiate(Piranha);
            }
        }

       /* patternCount[Type.pattern3]++;
        count_3++;
        StartCoroutine(Pattern3(1));*/
    }
    IEnumerator Pattern4(float waitTime, float startTime)
    {
        yield return new WaitForSeconds(waitTime);
        WaitForSeconds delay_0_5 = new WaitForSeconds(0.5f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5;
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5;
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5;
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.7f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //3
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //3.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //4
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //4.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(1f); //6
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //6.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //7
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return delay_0_5; //7.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.1f);
        Instantiate(Apple);
        Instantiate(Warning_4);
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
