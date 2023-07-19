using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pattern8b : MonoBehaviour
{
    [SerializeField]
    public GameObject weaselwarning; // 경고 오브젝트
    [SerializeField]
    public GameObject weasel; // 족제비 프리팹

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

    public float movementSpeed = 5f; // 족제비 이동 속도
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
        weaselCoroutine = StartCoroutine(SpawnWeasels());
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

    private IEnumerator SpawnWeasels()
    {
        List<GameObject> weaselObjects = new List<GameObject>();

        for (int i = 0; i < 7; i++)
        {
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
                Vector3 startPosition = secondPositions[i];
                Vector3 targetPosition = firstPositions[i];

                weaselObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }

            yield return null;
        }

        // Destroy weasel objects
        foreach (GameObject weaselObject in weaselObjects)
        {
            Destroy(weaselObject);
        }
    }
}
