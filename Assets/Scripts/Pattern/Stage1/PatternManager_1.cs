using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
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

    private int count_1_a;
    private int count_1_b;
    private int count_3;
    private int count_4;
    private float callTime;

    void Awake()
    {
        count_1_a = 0;
        count_1_b = 0;
        count_3 = 0;
        count_4 = 0;
        callTime = 0.5f;
    }
    void Start()
    {
        //1초 빠르게 함수 호출(경고가 1초 동안 나오므로), 벌이 날아오는데 1초가 걸리도록 설정
        Invoke("Pattern1_a", 4.0f);
        Invoke("Pattern1_b", 68f);

        Invoke("Pattern2_a", 12f);
        Invoke("Pattern2_a", 20f);
        Invoke("Pattern2_a", 28f);
        Invoke("Pattern2_a", 36f);

        Invoke("Pattern3", 19.0f);

        StartCoroutine(Pattern2_b_2(5.7f));
        StartCoroutine(Pattern2_b_2(21.7f));
        StartCoroutine(Pattern2_b_2(37.7f));
        StartCoroutine(Pattern2_b_2(86f));

        StartCoroutine(Pattern2_b_3(13.7f));
        StartCoroutine(Pattern2_b_3(29.7f));
        StartCoroutine(Pattern2_b_3(92f));
        StartCoroutine(Pattern2_b_3(98f));
        StartCoroutine(Pattern2_b_3(108f));
        StartCoroutine(Pattern2_b_3(114f));

        StartCoroutine(Pattern4(36f));
        StartCoroutine(Pattern4(44f));
        StartCoroutine(Pattern4(52f));
        StartCoroutine(Pattern4(60f));
        StartCoroutine(Pattern4(68f));
        StartCoroutine(Pattern4(76f));

        StartCoroutine(Pattern4(84f));
        StartCoroutine(Pattern4(92f));
        StartCoroutine(Pattern4(100f));
        StartCoroutine(Pattern4(108f));
        StartCoroutine(Pattern4(116f));
    }
    void Pattern1_a()
    {
        Instantiate(Bee_1_a);
        Instantiate(Warning_1_a);

        count_1_a++;
        Invoke("Pattern1_a", 0.5f);
    }
    void Pattern1_b()
    {
        Instantiate(Bee_1_b);
        Instantiate(Warning_1_b);

        count_1_b++;
        Invoke("Pattern1_b", 2f);
    }
    void Pattern2_a()
    {
        Instantiate(Warning_2_a);
        Instantiate(Oak_2_a);
    }
    IEnumerator Pattern2_b_2(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSecondsRealtime(2.0f);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);
    }
    IEnumerator Pattern2_b_3(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSecondsRealtime(2.0f);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSecondsRealtime(2.0f);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);
    }
    void Pattern3()
    {
        Instantiate(Warning_3);
        Instantiate(Piranha);
        
        count_3++;
        Invoke("Pattern3", 1f);
    }
    IEnumerator Pattern4(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Instantiate(Apple);
        Instantiate(Warning_4);

        //시간 조작
        if (count_4 == 3)
            callTime += 0.3f;
        if (count_4 == 4)
            callTime -= 0.1f;
        if (count_4 == 5)
            callTime -= 0.2f;
        if (count_4 == 10)
            callTime += 0.5f;
        if (count_4 == 11)
            callTime -= 0.5f;
        if (count_4 == 14)
            callTime -= 0.4f;

        count_4++;

        if (count_4 == 15)
        {
            callTime = 0.5f;
            yield break;
        }
        yield return new WaitForSecondsRealtime(callTime);
            StartCoroutine(Pattern4(0));
    }

    void Update()
    {
        //32회 실행시 호출 중단
        if (count_1_a == 32)
        {
            CancelInvoke("Pattern1_a");
        }
        if (count_1_b == 28)
        {
            CancelInvoke("Pattern1_b");
        }
        if (count_3 == 32)
        {
            CancelInvoke("Pattern3");
            //패턴1_b의 카운트 횟수를 통해 인보크 조절
            if (count_1_b == 0)
            {
                Invoke("Pattern3", 32f);
            }
            count_3 = 0;
        }
        if (count_4 == 16)
        {
            CancelInvoke("Pattern4");
            count_4 = 1;
            callTime = 0.5f;
        }   
    }
}
