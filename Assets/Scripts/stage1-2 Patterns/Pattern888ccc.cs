using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class Pattern888ccc : MonoBehaviour
{
    [SerializeField]
    private GameObject weasel;
    [SerializeField]
    private GameObject weaselWarning;
    [SerializeField]
    private float weaselSpeed;
    [SerializeField]
    private float[] rhythmTimings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f, 2.2f, 2.3f, 2.7f, 2.9f, 3.2f, 3.5f, 3.9f };

    private Coroutine weaselCoroutine;
    private GameObject currentWarning;
    private float startTime;

    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // ���� 3���� xPos ���� ������ �迭 ����
    int currentIndex = 0; // ���� ������ �ε����� ��Ÿ���� ���� ����

    EventManager eventManager;
    List<GameObject> objects;

    private void OnEnable()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        objects = new List<GameObject>();
        startTime = Time.time;
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
    }

    private void OnDestroy()
    {
        eventManager.deathEvent -= deathEvent;
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
            objects.Remove(currentWarning);
            Destroy(currentWarning);
            currentWarning = null;
        }
    }

    private IEnumerator WeaselRoutine()
    {
        // ��� ������ ���� ���뿡 �ش� ���� ������Ʈ�� �����մϴ�.
        Destroy(gameObject, 9.5f);
        for (int i = 0; i < rhythmTimings.Length; i++)
        {
            float timing = rhythmTimings[i];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }

            if (currentIndex < previousXPositions.Length)
            {
                xPos = Random.Range(-8.14f, 8.14f);
                previousXPositions[currentIndex] = xPos;
            }
            else
            {
                do
                {
                    xPos = Random.Range(-8.14f, 8.14f);
                } while (IsWithinRangeOfPreviousXPositions(xPos));
                previousXPositions[currentIndex % previousXPositions.Length] = xPos;
            }

            currentIndex++;

            //��� ������Ʈ ����

            yPos = -3.7209f;

            StartCoroutine(SpawnWeasel(xPos, yPos));
        }
    }

    private IEnumerator SpawnWeasel(float xPos, float yPos)
    {
        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(weaselWarning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();

        // ��� ������Ʈ�� 0.5�ʿ� ���ļ� ������������ ���İ� ����
        Color originalColor = warningRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float totalTime = 0.5f; // ��ü �ð� (0.5��)
        float fadeInDuration = 0.3f; // 0.3�� ������ ������ �������ϰ� ����

        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            // 0.3�� ������ ������ �������ϰ� ����
            if (elapsedTime <= fadeInDuration)
            {
                warningRenderer.color = originalColor;
            }
            // �� ���� 0.2�� ���ȿ��� ������ ������������ ���İ� ����
            else //0.3�ʰ� ����
            {
                float fadeOutDuration = totalTime - fadeInDuration; // ���������� �ð� (0.2��)
                warningRenderer.color = Color.Lerp(originalColor, targetColor, t);
            }

            yield return null;
        }

        // ��� ������Ʈ ����
        objects.Remove(newWarning);
        Destroy(newWarning);

        Vector3 spawnPosition = new Vector3(xPos, -6f, 0f);

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        objects.Add(newWeasel);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselSpeed;

        while (newWeasel.transform.position.y < -3.76f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.down * 10f;

        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // �� ������ ���� ��� ������Ʈ�� �ı��մϴ�.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                objects.Remove(obj);
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
    async UniTask delayRemoval(GameObject o, float t)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(t));
        objects.Remove(o);
    }

    void deathEvent()
    {
        StopAllCoroutines();
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
        objects.Clear();
        Destroy(gameObject);
    }
}
