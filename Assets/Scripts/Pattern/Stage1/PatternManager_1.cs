using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
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

    EventManager eventManager;
    private int count_1_a;
    private int count_1_b;
    private int count_3;
    private int count_4;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.savePointTime = savePointTime;
        count_1_a = 0;
        count_1_b = 0;
        count_3 = 0;
        count_4 = 0;
    }
    void Start()
    {
        //1초 빠르게 함수 호출(경고가 1초 동안 나오므로), 벌이 날아오는데 1초가 걸리도록 설정
        Invoke("Pattern1_a", 3.0f);
        Invoke("Pattern1_b", 68f);

        Invoke("Pattern2_a", 11f);
        Invoke("Pattern2_a", 19f);
        Invoke("Pattern2_a", 27f);
        Invoke("Pattern2_a", 35f);

        Invoke("Pattern3", 19.0f);

        StartCoroutine(Pattern2_b_2(4.7f));
        StartCoroutine(Pattern2_b_2(20.7f));
        StartCoroutine(Pattern2_b_2(36.7f));
        StartCoroutine(Pattern2_b_2(85f));

        StartCoroutine(Pattern2_b_3(12.7f));
        StartCoroutine(Pattern2_b_3(28.7f));
        StartCoroutine(Pattern2_b_3(91f));
        StartCoroutine(Pattern2_b_3(97f));
        StartCoroutine(Pattern2_b_3(107f));
        StartCoroutine(Pattern2_b_3(113f));

        StartCoroutine(Pattern4(35f));
        StartCoroutine(Pattern4(43f));
        StartCoroutine(Pattern4(51f));
        StartCoroutine(Pattern4(59f));
        StartCoroutine(Pattern4(67f));
        StartCoroutine(Pattern4(75f));

        StartCoroutine(Pattern4(83f));
        StartCoroutine(Pattern4(91f));
        StartCoroutine(Pattern4(99f));
        StartCoroutine(Pattern4(107f));
        StartCoroutine(Pattern4(115f));
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
        Invoke("Pattern1_b", 0.5f);
    }
    void Pattern2_a()
    {
        Instantiate(Warning_2_a);
        Instantiate(Oak_2_a);
    }
    IEnumerator Pattern2_b_2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSeconds(2.0f);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);
    }
    IEnumerator Pattern2_b_3(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSeconds(2.0f);

        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);

        yield return new WaitForSeconds(2.0f);

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
        yield return new WaitForSeconds(waitTime);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.8f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.7f);
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //3
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //3.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //4
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //4.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(1f); //6
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //6.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //7
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.5f); //7.5
        Instantiate(Apple);
        Instantiate(Warning_4);
        yield return new WaitForSeconds(0.1f);
        Instantiate(Apple);
        Instantiate(Warning_4);
    }



    void FixedUpdate()
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
                count_3 = 0;
            }
        }
        if (count_4 == 16)
        {
            CancelInvoke("Pattern4");
            count_4 = 1;
        }   
    }
}
