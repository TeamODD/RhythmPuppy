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

    [SerializeField]
    private List<float> pattern6Timings = new List<float>
    {
        0.4f, 1.4f, 2.4f, 3.4f,
        4.6f, 5.6f, 6.6f, 7.6f,
        8.8f, 9.8f, 10.8f, 11.8f,
        12.9f, 13.9f, 14.9f, 15.9f,
        17.1f, 18.1f, 19.1f, 20.1f,
        21.3f, 22.3f, 23.3f, 24.3f,
        25.4f, 26.4f,
        27.5f, 28.5f,
        29.6f, 30.6f
    };
    [SerializeField]
    private List<float> pattern1Timings = new List<float> { 0.4f, 1.4f, 2.4f }; // 패턴1의 발동 타이밍
    [SerializeField]
    private List<float> pattern2Timings = new List<float> { 0.4f, 0.8f, 1.2f }; // 패턴2의 발동 타이밍
    [SerializeField]
    private List<float> pattern3Timings = new List<float> { 0.6f, 0.8f, 1.2f }; // 패턴3의 발동 타이밍

    private void Start()
    {
        // 패턴1, 패턴2, 패턴3 스크립트를 비활성화
        pattern6.SetActive(false);
        pattern7a.SetActive(false);
        pattern7b.SetActive(false);
        // 추가 패턴 GameObject 변수들에 대해도 필요에 따라 비활성화 처리

        StartCoroutine(RunPattern6());
        // 추가 패턴 실행 메서드들도 필요에 따라 추가
    }

    private IEnumerator RunPattern6()
    {
        foreach (float timing in pattern6Timings)
        {
            // 패턴을 복제하고 활성화
            GameObject newPattern6 = Instantiate(pattern6, pattern6.transform.position, pattern6.transform.rotation);
            newPattern6.SetActive(true);
        }
        yield return null;
    }
}
