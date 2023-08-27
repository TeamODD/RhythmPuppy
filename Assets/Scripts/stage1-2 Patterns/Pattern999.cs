using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static EventManager;

public class Pattern999 : MonoBehaviour
{
    [SerializeField]
    private GameObject flyingSquirrel;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float squirrelSpeed = 4f;

    EventManager eventManager;
    List<GameObject> objects;

    private void OnEnable()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        objects = new List<GameObject>();
        StartCoroutine(SpawnFlyingSquirrels());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnFlyingSquirrels());
    }

    private void OnDestroy()
    {
        eventManager.deathEvent -= deathEvent;
    }

    private IEnumerator SpawnFlyingSquirrels()
    {
        // ���ٶ��㰡 �����ϴ� ��ġ�� �������� �����մϴ�.
        float xPos;
        float yPos = 4.293f;

        if (Random.Range(0, 2) == 0) // ���� ������ ����
            xPos = -8.16f;
            
        else // ������ ������ ����
            xPos = 8.16f;

        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();

        float scaleX = newWarning.transform.localScale.x;
        float scaleY = newWarning.transform.localScale.y;
        float scaleZ = newWarning.transform.localScale.z;
        if (xPos == -8.16f) // ���� ������ ����
        {
            newWarning.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
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
        objects.Remove(newWarning);
        Destroy(newWarning);

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // �밢�� ������ �����մϴ�.
        

        // ��ֹ��� �����ϰ� �ӵ��� ������ �����մϴ�.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        objects.Add(newSquirrel);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();

        scaleX = newSquirrel.transform.localScale.x;
        scaleY = newSquirrel.transform.localScale.y;
        scaleZ = newSquirrel.transform.localScale.z;

        Vector2 targetPosition = new Vector2(-9f, 0f);
        if (xPos == -8.16f) // ���� ������ ����
        {
            newSquirrel.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            targetPosition = new Vector2(9f, 0f);
        }

        Vector2 direction = (targetPosition - new Vector2(xPos, yPos)).normalized;
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
                objects.Remove(obj);
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
    async UniTask delayRemoval(GameObject o, float t)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(t));
        objects.Remove(o);
    }

    void deathEvent()
    {
        StopAllCoroutines();
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
        objects.Clear();
        Destroy(gameObject);
    }
}
