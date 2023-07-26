using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern8a : MonoBehaviour
{
    [SerializeField]
    private GameObject weasel;
    [SerializeField]
    private GameObject weaselWarning;
    [SerializeField]
    private float weaselSpeed = 10f;
    [SerializeField]
    private float[] rhythmTimings = {0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f, 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f};

    private Coroutine weaselCoroutine;
    private GameObject currentWarning;

    private void OnEnable()
    {
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
        float startTime = Time.time;

        for (int i = 0; i < rhythmTimings.Length; i++)
        {
            float targetTime = startTime + rhythmTimings[i];

            while (Time.time < targetTime)
            {
                yield return null;
            }

            if (i < rhythmTimings.Length - 1) // 마지막 족제비 타이밍 이전에만 경고 표시
            {
                if (currentWarning != null)
                {
                    Destroy(currentWarning);
                    currentWarning = null;
                }

                Vector3 spawnPosition = ShowWeaselWarning();
                yield return new WaitForSeconds(0.5f);

                Destroy(currentWarning);
                yield return StartCoroutine(SpawnWeasel(spawnPosition)); // 변경된 부분
            }
            else // 마지막 족제비 타이밍에 모든 경고 표시 제거
            {
                if (currentWarning != null)
                {
                    Destroy(currentWarning);
                    currentWarning = null;
                }
            }
        }
    }

    private Vector3 ShowWeaselWarning()
    {
        float xPos = Random.Range(-9f, 9f);
        Vector3 warningPosition = new Vector3(xPos, -4.6f, 0f);
        currentWarning = Instantiate(weaselWarning, warningPosition, Quaternion.identity);
        return new Vector3(xPos, -5f, 0f);
    }

    private IEnumerator SpawnWeasel(Vector3 spawnPosition)
    {
        spawnPosition.y = -6f;

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselSpeed;

        while (newWeasel.transform.position.y < -4.6f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;
        newWeasel.transform.position = new Vector3(newWeasel.transform.position.x, -4.6f, newWeasel.transform.position.z);

        yield return new WaitForSeconds(0.5f);

        weaselRigidbody.velocity = Vector2.down * weaselSpeed;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
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
        float minY = -5f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
