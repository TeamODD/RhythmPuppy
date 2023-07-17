using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern8b : MonoBehaviour
{
    [SerializeField]
    private GameObject weasel;
    [SerializeField]
    private GameObject weaselwarning;
    [SerializeField]
    private float weaselSpeed = 10f;
    [SerializeField]
    private float[] timings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f, 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f, 4.2f };

    private List<GameObject> weasels = new List<GameObject>();
    private List<GameObject> warnings = new List<GameObject>();
    private Coroutine performActionsCoroutine;

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
        performActionsCoroutine = StartCoroutine(PerformActions());
    }

    private void StopPattern()
    {
        if (performActionsCoroutine != null)
        {
            StopCoroutine(performActionsCoroutine);
            performActionsCoroutine = null;
        }

        foreach (GameObject weaselObj in weasels)
        {
            Destroy(weaselObj);
        }
        weasels.Clear();

        foreach (GameObject warningObj in warnings)
        {
            Destroy(warningObj);
        }
        warnings.Clear();
    }

    private IEnumerator PerformActions()
    {
        yield return new WaitForSeconds(timings[6]);

        // Step 1: Display warning object
        GameObject warning = Instantiate(weaselwarning, new Vector3(GetRandomXPosition(-10f, -2f), -4.6f, 0f), Quaternion.identity);
        warnings.Add(warning);

        yield return new WaitForSeconds(0.5f);

        // Step 2: Destroy warning object and spawn weasel object
        Destroy(warning);

        GameObject newWeasel = Instantiate(weasel, new Vector3(warning.transform.position.x, -4.6f, 0f), Quaternion.identity);
        weasels.Add(newWeasel);

        // Step 3: Move weasel objects up and down
        yield return new WaitForSeconds(timings[12] - timings[6]);

        foreach (GameObject weaselObj in weasels)
        {
            StartCoroutine(MoveWeaselUpAndDown(weaselObj));
        }
    }

    private IEnumerator MoveWeaselUpAndDown(GameObject weaselObj)
    {
        Vector3 startPosition = weaselObj.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 4.6f, 0f);

        float upSpeed = 5f;  // Speed of moving up
        float downSpeed = 10f;  // Speed of moving down

        // Move up
        while (weaselObj.transform.position != targetPosition)
        {
            weaselObj.transform.position = Vector3.MoveTowards(weaselObj.transform.position, targetPosition, upSpeed * 0.5f);
            yield return null;
        }

        // Move down
        while (weaselObj.transform.position != startPosition)
        {
            weaselObj.transform.position = Vector3.MoveTowards(weaselObj.transform.position, startPosition, downSpeed * 0.5f);
            yield return null;
        }

        Destroy(weaselObj);
    }

    private float GetRandomXPosition(float minX, float maxX)
    {
        return Random.Range(minX, maxX);
    }
}
