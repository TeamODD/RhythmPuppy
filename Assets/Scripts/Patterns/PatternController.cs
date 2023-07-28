using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternController : MonoBehaviour
{
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

    private List<GameObject> patterns = new List<GameObject>();

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
        67.2f, 68.2f, 69.2f, 70.2f,
        71.4f, 72.4f, 73.4f, 74.4f,
        75.6f, 76.6f, 77.6f, 78.6f,
        79.7f, 80.7f, 81.7f, 82.7f,
        83.9f, 84.9f, 85.9f, 86.9f,
        88.1f, 89.1f, 90.1f, 91.1f,
        92.2f, 93.2f, 94.3f, 95.3f,
        96.4f, 97.4f,
        134f, 135f, 136f, 137f,
        138.2f, 139.2f, 140.2f, 141.2f,
        142.4f, 143.4f, 144.4f, 145.4f,
        146.5f, 147.5f, 148.5f, 149.5f,
        150.7f, 151.7f, 152.7f, 153.7f,
        154.9f, 155.9f, 156.9f, 157.9f,
        159f, 160f, 161.1f, 162.1f,
        163.2f, 164.2f
    };

    private List<float> pattern7aTimings = new List<float>
    {
        0f, 4.1f, 8.2f,
        66.8f, 70.9f, 75f,
        133.6f, 137.7f, 141.8f
    };

    private List<float> pattern7bTimings = new List<float>
    {
        12.3f, 79.1f, 145.9f

    };

    private List<float> pattern8aTimings = new List<float>
    {
        16.5f, 20.7f, 24.9f,
        83.3f, 87.5f, 91.7f,
        150.1f, 154.3f, 158.5f
    };

    private List<float> pattern8bTimings = new List<float>
    {
        29.1f, 95.9f, 162.7f
    };

    private List<float> pattern8cTimings = new List<float>
    {
        33.3f, 100.1f, 166.9f
    };

    private List<float> pattern9Timings = new List<float>
    {
        37.4f, 39.5f,
        47.3f, 50.5f,
        58.8f,
        104.2f, 106,3f,
        114.1f, 117,3f,
        125.6f,
        171f, 173.1f,
        180.9f, 184.1f,
        192.4f
    };

    private List<float> pattern10Timings = new List<float>
    {
        42.1f, 45.3f,
        51f, 55.2f, 
        59.3f, 61.4f,
        108.9f, 112.1f,
        117.8f, 122f, 
        126.1f, 128.2f,
        175.7f, 178.9f,
        184.6f, 188.8f, 
        192.9f, 195f
    };

    private void Update()
    {
        //Debug.Log("게임의 시간"+Time.time);
    }

    private void Start()
    {
        // 패턴1, 패턴2, 패턴3 스크립트를 비활성화
        pattern6.SetActive(false);
        pattern7a.SetActive(false);
        pattern7b.SetActive(false);
        pattern8a.SetActive(false);
        // 추가 패턴 GameObject 변수들에 대해도 필요에 따라 비활성화 처리

        //StartCoroutine(RunPattern6());
        //StartCoroutine(RunPattern7a());
        //StartCoroutine(RunPattern7b());
        StartCoroutine(RunPattern8a());
        // 추가 패턴 실행 메서드들도 필요에 따라 추가
    }

    private IEnumerator RunPattern6()
    {
        for (int i = 0; i < pattern6Timings.Count; i++)
        {
            float timing = pattern6Timings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }
            // 패턴을 복제하고 활성화
            GameObject newPattern6 = Instantiate(pattern6, pattern6.transform.position, pattern6.transform.rotation);
            newPattern6.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern7a()
    {
        for (int i = 0; i < pattern7aTimings.Count; i++)
        {
            float timing = pattern7aTimings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }
            // 패턴을 복제하고 활성화
            GameObject newPattern7a = Instantiate(pattern7a, pattern7a.transform.position, pattern7a.transform.rotation);
            newPattern7a.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern7b()
    {
        for (int i = 0; i < pattern7bTimings.Count; i++)
        {
            float timing = pattern7bTimings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }
            // 패턴을 복제하고 활성화
            GameObject newPattern7b = Instantiate(pattern7b, pattern7b.transform.position, pattern7b.transform.rotation);
            newPattern7b.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern8a()
    {
        for (int i = 0; i < pattern8aTimings.Count; i++)
        {
            float timing = pattern8aTimings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }
            // 패턴을 복제하고 활성화
            GameObject newPattern8a = Instantiate(pattern8a, pattern8a.transform.position, pattern8a.transform.rotation);
            newPattern8a.SetActive(true);
        }
        yield return null;
    }
}
