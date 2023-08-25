using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern13 : MonoBehaviour
{
    [SerializeField]
    private GameObject thorstem;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float reachspeed; //���ô����� ���Ƴ��� �ӵ�
    [SerializeField]
    private float swingspeed; //���ô����� �ֵѷ����� �ӵ�
    [SerializeField]
    private float rotationDuration; //���ô����� �����Ǵ� �ð�

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
        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(-4.86f, -0.84f, 0f);
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

        Vector3 thorstemPosition = new Vector3(-4.86f, -15.3f, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();
        newthorstemRigidbody.velocity = Vector2.up * reachspeed;

        // �ڽ� ������Ʈ�� �Բ� �����̵��� ����
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = newthorstemRigidbody.velocity;
            }
        }

        while (newthorstem.transform.position.y < -4.60f)
        {
            yield return null;
        }

        // �θ� ������Ʈ�� �ڽ� ������Ʈ�� �ӵ� �ʱ�ȭ
        newthorstemRigidbody.velocity = Vector2.zero;
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = Vector2.zero;
            }
        }

        newthorstem.transform.Rotate(Vector3.back * swingspeed * Time.deltaTime);

        while (newthorstem.transform.rotation.eulerAngles.z > 250f)
        {
            newthorstem.transform.Rotate(Vector3.back * swingspeed * Time.deltaTime);
            yield return null;
        }

        Destroy(newthorstem);
        Destroy(gameObject);
    }
}
