using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pattern15 : MonoBehaviour
{
    [SerializeField]
    GameObject flower;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    float ComingOutSpeed;
    [SerializeField]
    float GetDownSpeed;

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
        float Xpos = Random.Range(-4.97f, 7.86f);
        Vector3 warningPosition = new Vector3(-4.56f, -4.49f, 0f);
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

        Vector3 flowerPosition = new Vector3(Xpos, -4.12f, 0f);
        GameObject newflower = Instantiate(flower, flowerPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        

    }

}
