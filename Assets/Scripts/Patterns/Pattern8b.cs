using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pattern8b : MonoBehaviour
{
    [SerializeField]
    private GameObject weaselwarning; // 경고 오브젝트
    [SerializeField]
    private GameObject weasel; // 족제비 프리팹
    [SerializeField]
    private float weaselspeed = 10f;

    private float[] firstweaselTimings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f };
    private float[] secondweaselTimings = { 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f };

    private Coroutine weaselCoroutine;
    private GameObject currentWarning;

    public List<Vector3> firstPositions = new List<Vector3>
    {
        new Vector3(-7.5f, -5f, 0f),
        new Vector3(-5f, -5f, 0f),
        new Vector3(-2.5f, -5f, 0f),
        new Vector3(0f, -5f, 0f),
        new Vector3(2.5f, -5f, 0f),
        new Vector3(5f, -5f, 0f),
        new Vector3(7.5f, -5f, 0f)
    };

    public List<Vector3> secondPositions = new List<Vector3>
    {
        new Vector3(-7.5f, -3f, 0f),
        new Vector3(-5f, -3f, 0f),
        new Vector3(-2.5f, -3f, 0f),
        new Vector3(0f, -3f, 0f),
        new Vector3(2.5f, -3f, 0f),
        new Vector3(5f, -3f, 0f),
        new Vector3(7.5f, -3f, 0f)
    };

    public List<Vector3> warningPositions = new List<Vector3>
    {
        new Vector3(-7.5f, -4.55f, 0f),
        new Vector3(-5f, -4.55f, 0f),
        new Vector3(-2.5f, -4.55f, 0f),
        new Vector3(0f, -4.55f, 0f),
        new Vector3(2.5f, -4.55f, 0f),
        new Vector3(5f, -4.55f, 0f),
        new Vector3(7.5f, -4.55f, 0f)
    };

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

        for (int i = 0; i < firstweaselTimings.Length; i++)
        {
            float targetTime = startTime + firstweaselTimings[i];

            while (Time.time < targetTime)
            {
                yield return null;
            }

            if (i < firstweaselTimings.Length - 1) // 마지막 족제비 타이밍 이전에만 경고 표시
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
        StartCoroutine(SpawnWeasels());
    }

    private Vector3 ShowWeaselWarning()
    {
        float xPos = Random.Range(-9f, 9f);
        Vector3 warningPosition = new Vector3(xPos, -4.6f, 0f);
        currentWarning = Instantiate(weaselwarning, warningPosition, Quaternion.identity);
        return new Vector3(xPos, -5f, 0f);
    }

    private IEnumerator SpawnWeasel(Vector3 spawnPosition)
    {
        spawnPosition.y = -7f;

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselspeed;

        while (newWeasel.transform.position.y < -4.6f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;
        newWeasel.transform.position = new Vector3(newWeasel.transform.position.x, -4.6f, newWeasel.transform.position.z);

        yield return new WaitForSeconds(0.5f);

        weaselRigidbody.velocity = Vector2.down * weaselspeed;
        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator SpawnWeasels()
    {
        List<GameObject> weaselObjects = new List<GameObject>();

        for (int i = 0; i < secondweaselTimings.Length; i++)
        {
            float timing = secondweaselTimings[i];

            while (Time.time < timing)
            {
                yield return null;
            }

            ShowWarningObject(warningPositions[i]);

            yield return new WaitForSeconds(0.5f);

            HideWarningObject();

            GameObject weaselObject = Instantiate(weasel, new Vector3(firstPositions[i].x, -6.12f, firstPositions[i].z), Quaternion.identity);
            weaselObjects.Add(weaselObject);

            Rigidbody2D weaselRigidbody = weaselObject.GetComponent<Rigidbody2D>();
            weaselRigidbody.velocity = Vector2.up * 5f;
            while (weaselObject.transform.position.y < -5f)
            {
                yield return null;
            }
            weaselRigidbody.velocity = Vector2.zero;

            yield return new WaitForSeconds(0.1f);
        }

        yield return StartCoroutine(FlyingWeasels(weaselObjects));
    }

    private void ShowWarningObject(Vector3 position)
    {
        weaselwarning.SetActive(true);
        weaselwarning.transform.position = position;
    }

    private void HideWarningObject()
    {
        weaselwarning.SetActive(false);
    }

    private IEnumerator FlyingWeasels(List<GameObject> weaselObjects)
    {
        float duration = 0.5f;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            for (int i = 0; i < weaselObjects.Count; i++)
            {
                GameObject weaselObject = weaselObjects[i];
                Vector3 startPosition = firstPositions[i];
                Vector3 targetPosition = secondPositions[i];

                weaselObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }

            yield return null;
        }

        // Move down
        duration = 0.5f;
        startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            for (int i = 0; i < weaselObjects.Count; i++)
            {
                GameObject weaselObject = weaselObjects[i];

                Rigidbody2D weaselObjectRigidbody = weaselObject.GetComponent<Rigidbody2D>();
                weaselObjectRigidbody.velocity = Vector2.down * 5f;
            }

            yield return null;
        }

        // Destroy weasel objects
        foreach (GameObject weaselObject in weaselObjects)
        {
            StartCoroutine(DestroyIfOutOfBounds(weaselObject));
        }
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
