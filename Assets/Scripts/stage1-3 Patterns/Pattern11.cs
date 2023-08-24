using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern11 : MonoBehaviour
{
    [SerializeField]
    GameObject Oak;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    float OakSpeed;

    private List<float> patternTimings = new List<float> { 0f, 0.4f, 0.7f, 1.0f };
    private float startTime;
    private float time;

    private void OnEnable()
    {
        startTime = Time.time; // 패턴이 활성화될 때 시작 시간 저장
        StartCoroutine(pattern());
    }

    private void OnDisable()
    {
        StopCoroutine(pattern());
    }

    private IEnumerator patterntiming()
    {
        // 원하는 타이밍에 따라 패턴을 실행합니다.
        for (int i = 0; i < patternTimings.Count; i++)
        {
            float timing = patternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }

            StartCoroutine(pattern());
        }
    }

    private IEnumerator pattern()
    {
        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(8.02f, -3.15f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        // 경고 오브젝트가 0.5초에 걸쳐서 투명해지도록 알파값 조정
        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        Color originalColor = warningRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float totalTime = 0.5f; // 전체 시간 (0.5초)
        float fadeInDuration = 0.3f; // 0.3초 동안은 완전히 불투명하게 유지

        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            // 0.3초 동안은 완전히 불투명하게 유지
            if (elapsedTime <= fadeInDuration)
            {
                warningRenderer.color = originalColor;
            }
            // 그 이후 0.2초 동안에는 빠르게 투명해지도록 알파값 조정
            else //0.3초가 지남
            {
                float fadeOutDuration = totalTime - fadeInDuration; // 투명해지는 시간 (0.2초)
                warningRenderer.color = Color.Lerp(originalColor, targetColor, t);
            }

            yield return null;
        }
        // 경고 오브젝트 제거
        Destroy(newWarning);

        Vector3 OakPosition = new Vector3(8.02f, -3.45f, 0f);
        GameObject newOak = Instantiate(Oak, OakPosition, Quaternion.identity);
        Rigidbody2D RedAppleRigidbody = newOak.GetComponent<Rigidbody2D>();
        RedAppleRigidbody.velocity = Vector2.left * OakSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newOak));
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            if (!IsWithinMapBounds(obj.transform.position))
            {
                Destroy(obj);
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        // IsWithinMapBounds 메서드 내용 그대로 가져옵니다.
        float minX = -10f;
        float maxX = 10f;
        float minY = -5.5f;
        float maxY = 10f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    // 고유 시간 변수를 사용하여 경과 시간을 계산하는 메서드
    private float GetElapsedTime()
    {
        return Time.time - startTime;
    }
}
