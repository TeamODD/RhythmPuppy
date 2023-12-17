using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        float xPos = 0;
        float yPos = 4.293f;

        if (Random.Range(0, 2) == 0) // ���� ������ ����
            xPos = -8.16f;
            
        else // ������ ������ ����
            xPos = 8.16f;

        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        float scaleX = newWarning.transform.localScale.x;
        float scaleY = newWarning.transform.localScale.y;
        float scaleZ = newWarning.transform.localScale.z;
        if (xPos == -8.16f) // ���� ������ ����
        {
            newWarning.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }

        // ��� ������Ʈ�� �ڽ� ������Ʈ�� Sprite Renderer �迭 ���
        SpriteRenderer[] warningRenderers = newWarning.GetComponentsInChildren<SpriteRenderer>();

        Color targetColor = new Color(1f, 0.3f, 0.3f, 0f);
        foreach (SpriteRenderer renderer in warningRenderers)
        {
            renderer.color = targetColor;
        }

        float totalTime = 0.25f;
        float elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(targetColor, Color.red, t);
            }

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(Color.red, targetColor, t);
            }

            yield return null;
        }
        //��������Ʈ �ı�
        Destroy(newWarning);

        //�������� ���� ����
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // �밢�� ������ �����մϴ�.
        

        // ��ֹ��� �����ϰ� �ӵ��� ������ �����մϴ�.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();

        scaleX = newSquirrel.transform.localScale.x;
        scaleY = newSquirrel.transform.localScale.y;
        scaleZ = newSquirrel.transform.localScale.z;

        float RandomZRoation = Random.Range(-225f, -250f); 
        if (xPos == -8.16f) // ���� ������ ����
        {
            newSquirrel.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            RandomZRoation = Random.Range(225f, 250f);
        }
        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, RandomZRoation) * Vector2.up;
        squirrelRigidbody.velocity = diagonalDirection.normalized * squirrelSpeed;

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
