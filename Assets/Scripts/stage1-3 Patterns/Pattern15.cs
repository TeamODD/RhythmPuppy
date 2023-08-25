using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pattern15 : MonoBehaviour
{
    [SerializeField]
    GameObject flower;
    [SerializeField]
    Sprite changedflower;
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
        float Xpos = Random.Range(-5.05f, 7.868f);
        Vector3 warningPosition = new Vector3(Xpos, -3.416f, 0f);
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

        Vector3 flowerPosition = new Vector3(Xpos, -5.198f, 0f);
        GameObject newflower = Instantiate(flower, flowerPosition, Quaternion.identity);
        Rigidbody2D flowerRigidBody2D = newflower.GetComponent<Rigidbody2D>();

        //���� ����
        flowerRigidBody2D.velocity = Vector2.up * 5;

        while (newflower.transform.position.y < -4.12f)
        {
            yield return null;
        }

        flowerRigidBody2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);
        ChangeImg(newflower);

        //�� ��� ����
        flowerRigidBody2D.velocity = Vector2.up * ComingOutSpeed;

        while (newflower.transform.position.y < -2.13)
        {
            yield return null;
        }

        flowerRigidBody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);

        flowerRigidBody2D.velocity = Vector2.down * GetDownSpeed;
        StartCoroutine(DestroyIfOutOfBounds(newflower));
    }

    private void ChangeImg(GameObject newflower)
    {
        new WaitForSeconds(0.3f);
        SpriteRenderer flowerSpriteRenderer = newflower.GetComponent<SpriteRenderer>();
        flowerSpriteRenderer.sprite = changedflower;
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // �� ������ ���� ��� ������Ʈ�� �ı��մϴ�.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                Destroy(obj);
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        float minX = -10f;
        float maxX = 10f;
        float minY = -7f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
