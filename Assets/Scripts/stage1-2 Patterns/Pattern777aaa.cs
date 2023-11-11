using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern777aaa : MonoBehaviour
{
    [SerializeField]
    private GameObject redapple;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float redappleSpeed;

    private List<float> patternTimings = new List<float> { 0f, 0.4f, 0.8f, 1.2f, 1.8f, 2f, 2.4f, 2.8f, 3f, 3.4f, 3.8f };

    private float startTime; // 패턴7a가 시작된 시간을 저장하기 위한 변수
    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // 이전 3개의 xPos 값을 저장할 배열 선언
    int currentIndex = 0; // 현재 저장할 인덱스를 나타내는 변수 선언

    private void OnEnable()
    {
        startTime = Time.time; // 패턴7a가 활성화될 때 시작 시간 저장
        StartCoroutine(Startpattern());
    }

    private void OnDisable()
    {
        StopCoroutine(Startpattern());
    }

    private IEnumerator Startpattern()
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

            // 패턴이 모두 실행된 순간에 패턴7a 오브젝트를 삭제합니다. * 복제된 스크립트를 삭제하기 위함
            Destroy(gameObject, 9f);


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

            StartCoroutine(showWarning(xPos));
        }
    }

    private IEnumerator showWarning(float xPos)
    {
        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        // 경고 오브젝트와 자식 오브젝트의 Sprite Renderer 배열 얻기
        SpriteRenderer[] warningRenderers = newWarning.GetComponentsInChildren<SpriteRenderer>();

        Color targetColor = new Color(1f, 0.3f, 0.3f, 0f);
        foreach (SpriteRenderer renderer in warningRenderers)
        {
            renderer.color = targetColor;
        }

        float totalTime = 0.25f;
        float elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(targetColor, Color.red, t);
            }

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(Color.red, targetColor, t);
            }

            yield return null;
        }

        Destroy(newWarning);

        Vector3 RedApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut 오브젝트 생성
        GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
        Rigidbody2D RedAppleRigidbody = newRedApple.GetComponent<Rigidbody2D>();
        RedAppleRigidbody.velocity = Vector2.down * redappleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newRedApple));
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
