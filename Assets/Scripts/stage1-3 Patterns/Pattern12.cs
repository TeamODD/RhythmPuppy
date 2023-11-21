using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern12 : MonoBehaviour
{
    [SerializeField]
    private GameObject thorstem;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float thorwingspeed; //���ô����� ������ �ӵ�
    [SerializeField]
    private GameObject PlayerCorgi;

    private void OnEnable()
    {
        StartCoroutine(pattern());
    }

    private void OnDisable()
    {
        StopCoroutine(pattern());
    }

    private IEnumerator pattern()
    {
        float randomZRotation;
        float PlayerCorgi_Xpos = PlayerCorgi.transform.position.x;

        if (PlayerCorgi_Xpos < 0f)
        {
            randomZRotation = Random.Range(30f, 60f);
        }
        else
        {
            randomZRotation = Random.Range(-30f, -60f);
        }

        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(0f, -5f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);

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

        float tanValue = Mathf.Tan(Mathf.Deg2Rad * (90 + randomZRotation));
        float thorstemPosX;
        if (PlayerCorgi_Xpos < 0f)
        {
            thorstemPosX = 20f;
        }
        else
        {
            thorstemPosX = -20f;
        }
        float thorstemPosY = 0f + (thorstemPosX + 5f) * tanValue;

        //Debug.Log(Mathf.Tan(Mathf.Deg2Rad * -randomZRotation));
        //Debug.Log(thorstemPosY);

        Vector3 thorstemPosition = new Vector3(thorstemPosX, thorstemPosY, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        newthorstem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation); // ȸ�� ���� ����

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();

        // ȸ���� ������ ����Ͽ� ���� ȸ��
        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, randomZRotation) * Vector2.up;

        // �밢�� �̵� �ӵ� ���
        Vector2 diagonalVelocity = diagonalDirection.normalized * thorwingspeed;

        newthorstemRigidbody.velocity = diagonalVelocity;

        // �ڽ� ������Ʈ�� �Բ� �����̵��� ����
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = diagonalVelocity;
            }
        }

        while (newthorstem.transform.position.y < -5f)
        {
            yield return null;
        }

        newthorstemRigidbody.velocity = Vector2.zero;
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = Vector2.zero;
            }
        }

        yield return FadeOut(newthorstem, 255f, 0f);

        Destroy(newthorstem);
        Destroy(gameObject);
    }

    private IEnumerator FadeOut(GameObject obj, float initialAlpha, float finalAlpha)
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
    }
}
