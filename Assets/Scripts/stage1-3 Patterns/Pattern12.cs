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
        Vector3 warningPosition = new Vector3(-4.56f, -4.49f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        float randomZRotation = Random.Range(-80f, -40f);
        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);


        float distanceFromDiagonal = 14f * Mathf.Sqrt(2f); // �밢�����κ����� �Ÿ�

        // �밢�� ���������� �̵� ���� ���
        Vector3 diagonalDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * randomZRotation), Mathf.Sin(Mathf.Deg2Rad * randomZRotation), 0f);

        // ���� �Ʒ����� �����ϴ� ��ġ ���
        Vector3 thorstemPosition = warningPosition - diagonalDirection * distanceFromDiagonal;

        // ���ο� ������Ʈ ���� �� ��ġ, ���� ����
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.Euler(0f, 0f, randomZRotation));


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

        /*
        Vector3 thorstemPosition = new Vector3(-18.56f, -18.49f, 0f);
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

        while (newthorstem.transform.position.y < 5.79f)
        {
            yield return null;
        }
        */
        Destroy(newthorstem);
        Destroy(gameObject);
    }
}
