using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class Pattern777aaa : MonoBehaviour
{
    [SerializeField]
    private GameObject redapple;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float redappleSpeed;

    private List<float> patternTimings = new List<float> { 0f, 0.4f, 0.8f, 1.2f, 1.8f, 2f, 2.4f, 2.8f, 3f, 3.4f, 3.8f };

    private float startTime; // ����7a�� ���۵� �ð��� �����ϱ� ���� ����
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
        startTime = Time.time; // ����7a�� Ȱ��ȭ�� �� ���� �ð� ����
        StartCoroutine(Startpattern());
    }

    private void OnDisable()
    {
        StopCoroutine(Startpattern());
    }

    private void OnDestroy()
    {
        eventManager.deathEvent -= deathEvent;
    }

    private IEnumerator Startpattern()
    { 
        // ���ϴ� Ÿ�ֿ̹� ���� ������ �����մϴ�.
        for (int i = 0; i < patternTimings.Count; i++)
        {
            float timing = patternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }

            // ������ ��� ����� ������ ����7a ������Ʈ�� �����մϴ�. * ������ ��ũ��Ʈ�� �����ϱ� ����
            StartCoroutine(destroySelf(9f));


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
        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        // ��� ������Ʈ�� 0.5�ʿ� ���ļ� ������������ ���İ� ����
        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
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

        Vector3 RedApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut ������Ʈ ����
        GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
        objects.Add(newRedApple);
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
                objects.Remove(obj);
                Destroy(obj);
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        // IsWithinMapBounds �޼��� ���� �״�� �����ɴϴ�.
        float minX = -10f;
        float maxX = 10f;
        float minY = -5.5f;
        float maxY = 10f;

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
            if (Mathf.Abs(prevX - xPos) < 2f)
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator destroySelf(float t)
    {
        yield return new WaitForSeconds(t);
        StopAllCoroutines();
        Destroy(gameObject);
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
