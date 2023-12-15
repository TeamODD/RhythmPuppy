using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern11 : MonoBehaviour
{
    [SerializeField]
    GameObject RedApple;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    float RedAppleSpeed;

    private List<float> patternTimings = new List<float> {0f, 0.4f, 0.7f, 1.0f};
    private float startTime;
    private float time;

    private void OnEnable()
    {
        startTime = Time.time; // ������ Ȱ��ȭ�� �� ���� �ð� ����
        StartCoroutine(patterntiming());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator patterntiming()
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

            StartCoroutine(pattern());
        }
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator pattern()
    {
        float Xpos;
        if (Random.Range(-1f, 1f) < 0f){
            Xpos = Random.Range(-2f, -8f);
        }
        else
        {
            Xpos = Random.Range(2f, 8f);
        }

        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(Xpos, 5.1f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

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
        Destroy(newWarning);

        Vector3 RedApplePosition = new Vector3(Xpos, 5.5f, 0f);
        GameObject NewRedApple = Instantiate(RedApple, RedApplePosition, Quaternion.identity);
        Rigidbody2D RedAppleRigidbody = NewRedApple.GetComponent<Rigidbody2D>();
        RedAppleRigidbody.velocity = Vector2.down * RedAppleSpeed;

        StartCoroutine(DestroyIfOutOfBounds(NewRedApple));
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
}
