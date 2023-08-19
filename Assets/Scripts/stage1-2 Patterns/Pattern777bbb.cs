using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern777bbb : MonoBehaviour
{
    [SerializeField]
    private GameObject redapple;
    [SerializeField]
    private GameObject greenapple;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float redappleSpeed;
    [SerializeField]
    private float greenappleSpeed;

    private List<float> RedApplepatternTimings = new List<float> { 0f, 0.4f, 0.8f, 1.2f, 1.8f };
    private List<float> GreenApplepatternTimings = new List<float> { 2f, 2.4f, 2.8f, 3f, 3.4f, 3.8f };

    private float startTime; // 패턴7b가 시작된 시간을 저장하기 위한 변수
    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // 이전 3개의 xPos 값을 저장할 배열 선언
    int currentIndex = 0; // 현재 저장할 인덱스를 나타내는 변수 선언

    private void OnEnable()
    {
        startTime = Time.time; // 패턴7b가 활성화될 때 시작 시간 저장
        StartCoroutine(Startpattern1());
        StartCoroutine(Startpattern2());
    }

    private void OnDisable()
    {
        StopCoroutine(Startpattern1());
        StopCoroutine(Startpattern2());
    }

    private IEnumerator Startpattern1()
    {
        // 원하는 타이밍에 따라 패턴을 실행합니다.
        for (int i = 0; i < RedApplepatternTimings.Count; i++)
        {
            float timing = RedApplepatternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }

            if (currentIndex < previousXPositions.Length)
            {
                xPos = Random.Range(-8.33f, 8.33f);
                previousXPositions[currentIndex] = xPos;
            }
            else
            {
                do
                {
                    xPos = Random.Range(-8.33f, 8.33f);
                } while (IsWithinRangeOfPreviousXPositions(xPos));
                previousXPositions[currentIndex % previousXPositions.Length] = xPos;
            }

            currentIndex++;

            StartCoroutine(showwarning1(xPos));
        }
    }

    private IEnumerator showwarning1(float xPos)
    {
        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        
        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();

        // 경고 오브젝트가 0.5초에 걸쳐서 투명해지도록 알파값 조정
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

        // 원하는 타이밍에 패턴을 실행합니다.
        Vector3 RedApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut 오브젝트 생성
        GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
        Rigidbody2D RedAppleRigidbody = newRedApple.GetComponent<Rigidbody2D>();
        RedAppleRigidbody.velocity = Vector2.down * redappleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newRedApple));
    }
    private IEnumerator Startpattern2()
    {
        // 원하는 타이밍에 따라 패턴을 실행합니다.
        for (int i = 0; i < GreenApplepatternTimings.Count; i++)
        {
            float timing = GreenApplepatternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }

            // 패턴이 모두 실행되면 스크립트를 비활성화합니다.
            Destroy(gameObject, 6.5f);

            if (currentIndex < previousXPositions.Length)
            {
                xPos = Random.Range(-8.33f, 8.33f);
                previousXPositions[currentIndex] = xPos;
            }
            else
            {
                do
                {
                    xPos = Random.Range(-8.33f, 8.33f);
                } while (IsWithinRangeOfPreviousXPositions(xPos));
                previousXPositions[currentIndex % previousXPositions.Length] = xPos;
            }

            currentIndex++;

            StartCoroutine(showwarning2(xPos));
        }
    }

    private IEnumerator showwarning2(float xPos)
    {
        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();

        // 경고 오브젝트가 0.5초에 걸쳐서 투명해지도록 알파값 조정
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

        // 원하는 타이밍에 패턴을 실행합니다.
        Vector3 GreenApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut 오브젝트 생성
        GameObject newGreenApple = Instantiate(greenapple, GreenApplePosition, Quaternion.identity);
        Rigidbody2D GreenAppleRigidbody = newGreenApple.GetComponent<Rigidbody2D>();
        GreenAppleRigidbody.velocity = Vector2.down * greenappleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newGreenApple));
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
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
    private bool IsWithinRangeOfPreviousXPositions(float xPos)
    {
        foreach (float prevX in previousXPositions)
        {
            if (Mathf.Abs(prevX - xPos) < 2f)
            {
                return true;
            }
        }
        return false;
    }
}
