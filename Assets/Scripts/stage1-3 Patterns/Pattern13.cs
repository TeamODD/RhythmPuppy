using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pattern13 : MonoBehaviour
{
    [SerializeField]
    private GameObject thorstem;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float speed; //���ô����� ���Ƴ��� �ӵ�
    [SerializeField]
    private List<float> patternTimings = new List<float> { 0f, 0.4f, 0.7f, 1.0f };

    private float startTime;
    private float time;

    List<GameObject> ThorstemsList = new List<GameObject>();
    GameObject PlayerCorgi;
    float PlayerCorgi_Xpos;

    private void OnEnable()
    {
        startTime = Time.time;

        PlayerCorgi = GameObject.Find("corgi");
        PlayerCorgi_Xpos = PlayerCorgi.transform.position.x;

        StartCoroutine(pattern());
    }

    private void OnDisable()
    {
        StopCoroutine(pattern());
    }

    private IEnumerator pattern()
    {
        float Xpos;
        if (PlayerCorgi_Xpos < 0f)
        {
            Xpos = -5.14f;
        }
        else
        {
            Xpos = 5.14f;
        }

        // ��� ������Ʈ ���� �� �ı�
        {
            Vector3 warningPosition = new Vector3(Xpos, -0.88f, 0f);
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
        }

        for (int Order = 0; Order < patternTimings.Count; Order++)
        {
            float timing = patternTimings[Order];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            StartCoroutine(ShotThorstem(Order));
        }

        yield return new WaitUntil(() => ThorstemsList.Count == 4 && ThorstemsList[3] != null && ThorstemsList[3].transform.position.y >= 0f);

        foreach (GameObject Thorstem in ThorstemsList)
        {
            StartCoroutine(FadeOutAndDestroy(Thorstem, 255f, 0f));
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private IEnumerator ShotThorstem(int Order)
    {
        float randomZRotation;
        float ThorstemXpos;

        if (Order % 2 == 0) // ¦���� ���(������ ù��°, ����°)
        {
            randomZRotation = Random.Range(5f, 20f);
            ThorstemXpos = (Order == 0) ? -4f : -2f;
        }
        else  // Ȧ���� ���(������ �ι�°, �׹�°)
        {
            randomZRotation = Random.Range(-20f, -5f);
            ThorstemXpos = (Order == 1) ? -8f : -6f;
        }
        ThorstemXpos = (PlayerCorgi_Xpos < 0f) ? -Mathf.Abs(ThorstemXpos) : Mathf.Abs(ThorstemXpos);

        float tanValue = Mathf.Tan(Mathf.Deg2Rad * (90 + randomZRotation));
        float thorstemPosY = -12f;

        Vector3 thorstemPosition = new Vector3(ThorstemXpos, thorstemPosY, 0f);
        GameObject NewThorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);
        ThorstemsList.Add(NewThorstem);

        NewThorstem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);

        Rigidbody2D newthorstemRigidbody = NewThorstem.GetComponent<Rigidbody2D>();
        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, randomZRotation) * Vector2.up;
        Vector2 diagonalVelocity = diagonalDirection.normalized * speed;
        newthorstemRigidbody.velocity = diagonalVelocity;

        yield return new WaitUntil(() => NewThorstem.transform.position.y >= 0f);

        newthorstemRigidbody.velocity = Vector2.zero;
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj, float initialAlpha, float finalAlpha)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //���� �������� �ʱ� �������� �ٲ� �ۼ��� �� ����.

            // 0���� 255 ������ ������ ���� ����
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer renderer in renderers)
            {
                Color color = renderer.color;

                // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
                float normalizedAlpha = currentAlpha / 255.0f;

                color.a = normalizedAlpha; // ���� �� ����
                renderer.color = color; // ����� ���� ����
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        Debug.Log(Time.time);
    }

    private float GetElapsedTime()
    {
        return Time.time - startTime;
    }
}
