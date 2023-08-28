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
    private float weaselUpSpeed;
    [SerializeField]
    private float weaselDownspeed;
    [SerializeField]
    private float[] rhythmTimings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f, 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f };

    private Coroutine weaselCoroutine;
    private GameObject currentWarning;
    private float startTime;

    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // ���� 3���� xPos ���� ������ �迭 ����
    int currentIndex = 0; // ���� ������ �ε����� ��Ÿ���� ���� ����

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
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            // ��� ������ ���� ���뿡 �ش� ���� ������Ʈ�� �����մϴ�.
            Destroy(gameObject, 9.5f);

            if (currentIndex < previousXPositions.Length)
            {
                xPos = Random.Range(-8.3007f, 8.3007f);
                previousXPositions[currentIndex] = xPos;
            }
            else
            {
                do
                {
                    xPos = Random.Range(-8.3007f, 8.3007f);
                } while (IsWithinRangeOfPreviousXPositions(xPos));
                previousXPositions[currentIndex % previousXPositions.Length] = xPos;
            }

            currentIndex++;


            //��� ������Ʈ ����

            yPos = -4.154f;

            StartCoroutine(SpawnWeasel(xPos, yPos));
        }
    }

    private IEnumerator SpawnWeasel(float xPos, float yPos)
    {
        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(weaselWarning, warningPosition, Quaternion.identity);

        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

        // ��� ������Ʈ�� �ڽ� ������Ʈ�� Sprite Renderer �迭 ���
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

        Vector3 spawnPosition = new Vector3(xPos, -6.06f, 0f); //�ĺ� -6f, -5.03f

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselUpSpeed;

        while (newWeasel.transform.position.y < -3.963f)    
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;

        StartCoroutine(WeaselGoDown(weaselRigidbody));
        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
    }

    private IEnumerator WeaselGoDown(Rigidbody2D weaselRigidbody)
    {
        yield return new WaitForSeconds(0.5f);
        weaselRigidbody.velocity = Vector2.down * weaselDownspeed;
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // �� ������ ���� ��� ������Ʈ�� �ı��մϴ�.
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
        float minY = -6.3f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    // ���� �ð� ������ ����Ͽ� ��� �ð��� ����ϴ� �޼���
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
