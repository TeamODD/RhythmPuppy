using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern888aaa : MonoBehaviour
{
    [SerializeField]
    private GameObject weasel;
    [SerializeField]
    private GameObject weaselWarning;
    [SerializeField]
    private float weaselSpeed = 10f;
    [SerializeField]
    private float[] rhythmTimings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f, 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f };

    private Coroutine weaselCoroutine;
    private GameObject currentWarning;
    private float startTime;

    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // 이전 3개의 xPos 값을 저장할 배열 선언
    int currentIndex = 0; // 현재 저장할 인덱스를 나타내는 변수 선언

    private void OnEnable()
    {
        startTime = Time.time;
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
    }

    private void StartPattern()
    {
        weaselCoroutine = StartCoroutine(WeaselRoutine());
    }

    private void StopPattern()
    {
        if (weaselCoroutine != null)
        {
            StopCoroutine(weaselCoroutine);
            weaselCoroutine = null;
        }

        if (currentWarning != null)
        {
            Destroy(currentWarning);
            currentWarning = null;
        }
    }

    private IEnumerator WeaselRoutine()
    {
        for (int i = 0; i < rhythmTimings.Length; i++)
        {
            float timing = rhythmTimings[i];

            while (GetElapsedTime() < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }
            // 모든 패턴이 끝날 때쯤에 해당 게임 오브젝트를 삭제합니다.
            Destroy(gameObject, 9.5f);

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


            //경고 오브젝트 생성

            yPos = -4.5f;

            StartCoroutine(SpawnWeasel());
        }
    }

    private IEnumerator SpawnWeasel()
    {
        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject currentWarning = Instantiate(weaselWarning, warningPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Destroy(currentWarning);

        yPos = -8f;
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselSpeed;

        while (newWeasel.transform.position.y < -4.6f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;
        newWeasel.transform.position = new Vector3(newWeasel.transform.position.x, -4.6f, newWeasel.transform.position.z);

        StartCoroutine(WeaselGoDown(weaselRigidbody));
        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
    }

    private IEnumerator WeaselGoDown(Rigidbody2D weaselRigidbody)
    {
        yield return new WaitForSeconds(0.5f);
        weaselRigidbody.velocity = Vector2.down * 10f;
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                Debug.Log("패턴 종료 : " + Time.time);
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
        float minY = -6.3f;
        float maxY = 5f;

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
            if (Mathf.Abs(prevX - xPos) < 1.5f)
            {
                return true;
            }
        }
        return false;
    }
}
