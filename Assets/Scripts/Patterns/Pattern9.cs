using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern9 : MonoBehaviour
{
    [SerializeField]
    private GameObject flyingSquirrel;
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
        float yPos = 4.4f;

        if (Random.Range(0, 2) == 0) // ���� ������ ����
            xPos = -10f;
        else // ������ ������ ����
            xPos = 10f;

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // �밢�� ������ �����մϴ�.
        Vector2 direction = (xPos < 0f ? Vector2.right : Vector2.left) + Vector2.down;

        // ��ֹ��� �����ϰ� �ӵ��� ������ �����մϴ�.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();
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