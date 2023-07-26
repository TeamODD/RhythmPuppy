using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern7a : MonoBehaviour
{
    [SerializeField]
    private GameObject redapple;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float redappleSpeed;

    private List<float> patternTimings = new List<float> {0f, 0.4f, 0.8f, 1.2f, 1.8f, 2f, 2.4f, 2.8f, 3f, 3.4f, 3.8f};

    private void OnEnable()
    {
        StartCoroutine(DropRedapple());
    }

    private void OnDisable()
    {
        StopCoroutine(DropRedapple());
    }

    private IEnumerator DropRedapple()
    {
        // ���ϴ� Ÿ�ֿ̹� ���� ������ �����մϴ�.
        for (int i = 0; i < patternTimings.Count; i++)
        {
            float timing = patternTimings[i];

            while (Time.time < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }

            // ��� ������Ʈ ����
            float xPos = UnityEngine.Random.Range(-7f, 7f);
            Vector3 warningPosition = new Vector3(xPos, 4.5f, 0f);
            GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

            SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
            if (warningRenderer != null)
            {
                warningRenderer.sortingOrder = int.MaxValue;
            }

            //yield return new WaitForSeconds(0.5f);
            Destroy(newWarning, 0.5f);

            // ���ϴ� Ÿ�ֿ̹� ������ �����մϴ�.
            Vector3 RedApplePosition = new Vector3(xPos, 7f, 0f);

            // Chestnut ������Ʈ ����
            GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
            Rigidbody2D RedAppleRigidbody = newRedApple.GetComponent<Rigidbody2D>();
            RedAppleRigidbody.velocity = Vector2.down * redappleSpeed;

            StartCoroutine(DestroyIfOutOfBounds(newRedApple));
        }

        // ������ ��� ����Ǹ� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�.
        yield return new WaitForSeconds(4.5f);
        gameObject.SetActive(false);
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        // DestroyIfOutOfBounds �ڷ�ƾ ���� �״�� �����ɴϴ�.
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
        // IsWithinMapBounds �޼��� ���� �״�� �����ɴϴ�.
        float minX = -10f;
        float maxX = 10f;
        float minY = -5f;
        float maxY = 10f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
