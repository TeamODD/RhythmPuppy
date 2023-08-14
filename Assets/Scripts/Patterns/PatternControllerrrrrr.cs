using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternControllerrrrrr : MonoBehaviour
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
        69.2f, 70.2f, 71.2f, 72.2f,
        73.4f, 74.4f, 75.4f, 76.4f, 
        77.6f, 78.6f, 79.6f, 80.6f,
        81.7f, 82.7f, 83.7f, 84.7f,
        85.9f, 86.9f, 87.9f, 88.9f, 
        90.1f, 91.1f, 92.1f, 93.1f,
        94.2f, 95.2f, 96.3f, 97.3f,
        98.4f, 99.4f
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
110.9f, 111.9f, 112.9f, 114.1f, 115.1f, 119.8f, 120.8f, 121.8f, 122.8f, 123.2f, 124.2f, 125.2f, 128.1f, 129.1f, 130.2f, 131.2f, 132.2f, 133.2f
    };


    private float startTime;
    private float savePointTime;

    public void Start()
    {
        Checkingsavepoint();
        gameprogress.GetComponent<GameProgress>().SettingCheckPoint();
    }

    private void Start()
    {
        // ����1, ����2, ����3 ��ũ��Ʈ�� ��Ȱ��ȭ
        pattern6.SetActive(false);
        pattern7a.SetActive(false);
        pattern7b.SetActive(false);
        pattern8a.SetActive(false);
        pattern8b.SetActive(false);
        pattern8c.SetActive(false);
        pattern9.SetActive(false);
        pattern10.SetActive(false);
        // �߰� ���� GameObject �����鿡 ���ص� �ʿ信 ���� ��Ȱ��ȭ ó��

        StartCoroutine(RunPattern6());
        StartCoroutine(RunPattern7a());
        StartCoroutine(RunPattern7b());
        StartCoroutine(RunPattern8a());
        StartCoroutine(RunPattern8b());
        StartCoroutine(RunPattern8c());
        StartCoroutine(RunPattern9());
        StartCoroutine(RunPattern10());
        // �߰� ���� ���� �޼���鵵 �ʿ信 ���� �߰�
    }


    /*
    private void OnEnable()
    {
        startTime = Time.time;
        Checkingsavepoint();

        // ����1, ����2, ����3 ��ũ��Ʈ�� ��Ȱ��ȭ
        pattern6.SetActive(false);
        pattern7a.SetActive(false);
        pattern7b.SetActive(false);
        pattern8a.SetActive(false);
        pattern8b.SetActive(false);
        pattern8c.SetActive(false);
        pattern9.SetActive(false);
        pattern10.SetActive(false);
        // �߰� ���� GameObject �����鿡 ���ص� �ʿ信 ���� ��Ȱ��ȭ ó��

        StartCoroutine(RunPattern6());
        StartCoroutine(RunPattern7a());
        StartCoroutine(RunPattern7b());
        StartCoroutine(RunPattern8a());
        StartCoroutine(RunPattern8b());
        StartCoroutine(RunPattern8c());
        StartCoroutine(RunPattern9());
        StartCoroutine(RunPattern10());
        // �߰� ���� ���� �޼���鵵 �ʿ信 ���� �߰�
    }
    */

    private void OnDisable()
    {
        StopCoroutine(RunPattern6());
        StopCoroutine(RunPattern7a());
        StopCoroutine(RunPattern7b());
        StopCoroutine(RunPattern8a());
        StopCoroutine(RunPattern8b());
        StopCoroutine(RunPattern8c());
        StopCoroutine(RunPattern9());
        StopCoroutine(RunPattern10());
    }

    private void Checkingsavepoint() //���� GameProgress���� ���� ������ ���൵ �ٴ� �������ִ� ��Ȳ
    {
        float checkpointTime = PlayerPrefs.GetFloat("checkpointTime");

        if (checkpointTime == 0)
        {
            startTime = 0f;
        }
        else if (checkpointTime == 39.6669f) 
        {
            startTime = 39.6669f;
        }
        else if (checkpointTime == 79.3338f)
        {
            startTime = 79.3338f;
        }
        else if (checkpointTime == 119.0008f)
        {
            startTime = 119.0008f;
        }
    }

    private float GetElapsedTime()
    {
        float elapsedTime = Time.time + startTime;
        float roundedElapsedTime = Mathf.Round(elapsedTime * 10f) / 10f; // �Ҽ� ù° �ڸ����� �ݿø�
        return roundedElapsedTime;
    }

    private void Update()
    {
        Debug.Log("GetElapsedTime : " + GetElapsedTime());
        Debug.Log("Time.time : " + Mathf.Round(Time.time * 10f) / 10f);
    }

    private IEnumerator RunPattern6()
    {
        for (int i = 0; i < pattern6Timings.Count; i++)
        {
            float timing = pattern6Timings[i];

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {    // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
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

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
             {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
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

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            while (Time.time < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
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

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
            GameObject newPattern8a = Instantiate(pattern8a, pattern8a.transform.position, pattern8a.transform.rotation);
            newPattern8a.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern8b()
    {
        for (int i = 0; i < pattern8bTimings.Count; i++)
        {
            float timing = pattern8bTimings[i];

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
            GameObject newPattern8b = Instantiate(pattern8b, pattern8b.transform.position, pattern8b.transform.rotation);
            newPattern8b.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern8c()
    {
        for (int i = 0; i < pattern8cTimings.Count; i++)
        {
            float timing = pattern8cTimings[i];

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
            GameObject newPattern8c = Instantiate(pattern8c, pattern8c.transform.position, pattern8c.transform.rotation);
            newPattern8c.SetActive(true);
        }
        yield return null;
    }

    private IEnumerator RunPattern9()
    {
        for (int i = 0; i < pattern9Timings.Count; i++)
        {
            float timing = pattern9Timings[i];

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
            GameObject newPattern9 = Instantiate(pattern9, pattern9.transform.position, pattern9.transform.rotation);
            newPattern9.SetActive(true);
        }
        yield return null;
    }
        
    private IEnumerator RunPattern10()
    {
        for (int i = 0; i < pattern10Timings.Count; i++)
        {
            float timing = pattern10Timings[i];

            if (timing < GetElapsedTime())
            {
                continue;
            }

            while (GetElapsedTime() != timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ������ �����ϰ� Ȱ��ȭ
            GameObject newPattern10 = Instantiate(pattern10, pattern10.transform.position, pattern10.transform.rotation);
            newPattern10.SetActive(true);
        }
        yield return null;
    }
}
