using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pattern999 : MonoBehaviour
{
    [SerializeField]
    private GameObject flyingSquirrel;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float squirrelSpeed = 4f;

    private void OnEnable()
    {
        StartCoroutine(SpawnFlyingSquirrels());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnFlyingSquirrels());
    }

    private IEnumerator SpawnFlyingSquirrels()
    {
        // ���ٶ��㰡 �����ϴ� ��ġ�� �������� �����մϴ�.
        float xPos;
        float yPos = 4.5f;

        if (Random.Range(0, 2) == 0) // ���� ������ ����
            xPos = -8.385f;
            
        else // ������ ������ ����
            xPos = 8.385f;

        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        if (warningRenderer != null)
        {
            warningRenderer.sortingOrder = int.MaxValue;
        }

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

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // �밢�� ������ �����մϴ�.
        Vector2 direction = (xPos < 0f ? Vector2.right : Vector2.left) + Vector2.down;

        // ��ֹ��� �����ϰ� �ӵ��� ������ �����մϴ�.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();

        float scaleX = newSquirrel.transform.localScale.x;
        float scaleY = newSquirrel.transform.localScale.y;
        float scaleZ = newSquirrel.transform.localScale.z;
        if (xPos == -8.385f) // ���� ������ ����
        {
            newSquirrel.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
        squirrelRigidbody.velocity = direction.normalized * squirrelSpeed;

        yield return StartCoroutine(DestroyIfOutOfBounds(newSquirrel));
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
        float minY = -5f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
