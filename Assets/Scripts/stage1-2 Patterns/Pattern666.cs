using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class Pattern666 : MonoBehaviour
{
    [SerializeField]
    private GameObject thornStem;
    [SerializeField]
    private GameObject thornStemWarning;
    [SerializeField]
    private float stemSpeed;

    private bool isPatternRunning = false;
    private GameObject currentStem;
    EventManager eventManager;
    List<GameObject> objects;

    private void OnEnable()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += StopPattern;
        objects = new List<GameObject>();
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
    }

    private void OnDestroy()
    {
        eventManager.deathEvent -= StopPattern;
    }

    private void StartPattern()
    {
        if (!isPatternRunning)
        {
            isPatternRunning = true;
            StartCoroutine(RunPattern());
        }
    }

    private void StopPattern()
    {
        isPatternRunning = false;
        if (currentStem != null)
        {
            objects.Remove(currentStem);
            Destroy(currentStem);
            currentStem = null;
        }
        StopAllCoroutines();
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
        objects.Clear();
        Destroy(gameObject);
    }

    private IEnumerator RunPattern()
    {
        //������ ��ġ������ ����

        // ��� ������Ʈ ����
        Vector3 warningPosition = new Vector3(8.297f, 2.28f, 0f);
        GameObject warning = Instantiate(thornStemWarning, warningPosition, Quaternion.identity);
        objects.Add(warning);

        // ��� ������Ʈ�� 0.5�ʿ� ���ļ� ������������ ���İ� ����
        SpriteRenderer warningRenderer = warning.GetComponent<SpriteRenderer>();
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
        objects.Remove(warning);
        Destroy(warning);

        // ���� �ٱ� ����
        float startX = 8.38f; //9.44f, 8.38f
        float startY = 2.58f;
        Vector3 startPos = new Vector3(startX, startY, 0f);

        currentStem = Instantiate(thornStem, startPos, Quaternion.identity);
        objects.Add(currentStem);
        Rigidbody2D stemRigidbody = currentStem.GetComponent<Rigidbody2D>();

        // ���������� �̵�
        if (startX < 0f)
            stemRigidbody.velocity = Vector2.right * stemSpeed;
        // �������� �̵�
        else
            stemRigidbody.velocity = Vector2.left * stemSpeed;

        StartCoroutine(DestroyIfOutOfBounds(currentStem));
    }
    

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (isPatternRunning)
        {
            // �� ������ ���� ��� ������Ʈ�� �ı��մϴ�.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                objects.Remove(obj);
                Destroy(obj);
                currentStem = null;
                StopPattern();
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        float minX = -10f;
        float maxX = 12f;
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
