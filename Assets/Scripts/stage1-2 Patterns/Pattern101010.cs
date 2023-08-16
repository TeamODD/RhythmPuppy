using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern101010 : MonoBehaviour
{
    [SerializeField]
    private GameObject chestnut;
    [SerializeField]
    private GameObject chestnutProjectile;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private GameObject chestnutbomb;
    [SerializeField]
    private float chestnutSpeed;
    [SerializeField]
    private float splinterSpeed;
    [SerializeField]
    private float splinterInterval;

    private void OnEnable()
    {
        StartCoroutine(DropChestnuts());
    }

    private void OnDisable()
    {
        StopCoroutine(DropChestnuts());
    }

    private IEnumerator DropChestnuts()
    {

        float xPos = Random.Range(-8.124f, 8.124f);
        Vector3 chestnutPosition = new Vector3(xPos, 4.201f, 0f);

        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(xPos, 4.327f, 0f);
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

        // ChestNut ������Ʈ ����
        GameObject newChestnut = Instantiate(chestnut, chestnutPosition, Quaternion.identity);
        Rigidbody2D chestnutRigidbody = newChestnut.GetComponent<Rigidbody2D>();
        chestnutRigidbody.velocity = Vector2.down * chestnutSpeed;

        yield return new WaitForSeconds(0.5f);

        ExplodeChestnut(newChestnut.transform.position);
        Destroy(newChestnut);
        yield return null;
    }

    private void ExplodeChestnut(Vector3 position)
    {
        GameObject ChestNutBomb = Instantiate(chestnutbomb, position, Quaternion.identity);
        Destroy(ChestNutBomb, 0.2f);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * splinterInterval;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 splinterDirection = new Vector3(x, y, 0f).normalized;
            GameObject newSplinter = Instantiate(chestnutProjectile, position, Quaternion.identity);

            MovementTransform2D movementComponent = newSplinter.GetComponent<MovementTransform2D>();
            movementComponent.MoveTo(splinterDirection * splinterSpeed);

            float angleInDegrees = Mathf.Atan2(splinterDirection.y, splinterDirection.x) * Mathf.Rad2Deg;
            newSplinter.transform.rotation = Quaternion.Euler(0f, 0f, angleInDegrees - 90f); // -90�� ȸ��

            StartCoroutine(DestroyIfOutOfBounds(newSplinter));

            Destroy(gameObject, 6f);
        }
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
        float minY = -5f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
