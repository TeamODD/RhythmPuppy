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

    private float startTime; // ����7b�� ���۵� �ð��� �����ϱ� ���� ����
    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // ���� 3���� xPos ���� ������ �迭 ����
    int currentIndex = 0; // ���� ������ �ε����� ��Ÿ���� ���� ����

    private void OnEnable()
    {
        startTime = Time.time; // ����7b�� Ȱ��ȭ�� �� ���� �ð� ����
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
        // ���ϴ� Ÿ�ֿ̹� ���� ������ �����մϴ�.
        for (int i = 0; i < RedApplepatternTimings.Count; i++)
        {
            float timing = RedApplepatternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
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
        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        
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
        Destroy(newWarning);

        // ���ϴ� Ÿ�ֿ̹� ������ �����մϴ�.
        Vector3 RedApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut ������Ʈ ����
        GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
        Rigidbody2D RedAppleRigidbody = newRedApple.GetComponent<Rigidbody2D>();
        RedAppleRigidbody.velocity = Vector2.down * redappleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newRedApple));
    }
    private IEnumerator Startpattern2()
    {
        // ���ϴ� Ÿ�ֿ̹� ���� ������ �����մϴ�.
        for (int i = 0; i < GreenApplepatternTimings.Count; i++)
        {
            float timing = GreenApplepatternTimings[i];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }

            // ������ ��� ����Ǹ� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�.
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
        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(xPos, -0.8f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

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
        Destroy(newWarning);

        // ���ϴ� Ÿ�ֿ̹� ������ �����մϴ�.
        Vector3 GreenApplePosition = new Vector3(xPos, 4.5f, 0f);

        // Chestnut ������Ʈ ����
        GameObject newGreenApple = Instantiate(greenapple, GreenApplePosition, Quaternion.identity);
        Rigidbody2D GreenAppleRigidbody = newGreenApple.GetComponent<Rigidbody2D>();
        GreenAppleRigidbody.velocity = Vector2.down * greenappleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(newGreenApple));
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
}
