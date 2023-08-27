using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class Pattern888bbb : MonoBehaviour
{
    [SerializeField]
    private GameObject weasel; // ������ ������
    [SerializeField]
    private GameObject weaselwarning; // ��� ������Ʈ
    [SerializeField]
    private float weaselspeed;

    //private float[] firstweaselTimings = { 0f, 0.6f, 0.8f, 1.1f, 1.5f, 1.8f };
    [SerializeField]
    private float[] secondweaselTimings = { 2.1f, 2.2f, 2.6f, 2.8f, 3.1f, 3.4f, 3.8f };
        
    private Coroutine weaselCoroutine;
    private GameObject currentWarning;

    private List<Vector3> firstPositions = new List<Vector3>
    {
        new Vector3(-9f, -6f, 0f),
        new Vector3(-6f, -6f, 0f),
        new Vector3(-3f, -6f, 0f),
        new Vector3(0f, -6f, 0f),
        new Vector3(3f, -6f, 0f),
        new Vector3(6f, -6f, 0f),
        new Vector3(9f, -6f, 0f)
    };

    private List<Vector3> warningPositions = new List<Vector3>
    {
        new Vector3(-9f, -4.55f, 0f),
        new Vector3(-6f, -4.55f, 0f),
        new Vector3(-3f, -4.55f, 0f),
        new Vector3(0f, -4.55f, 0f),
        new Vector3(3f, -4.55f, 0f),
        new Vector3(6f, -4.55f, 0f),
        new Vector3(9f, -4.55f, 0f)
    };

    private float startTime;
    float xPos;
    float yPos;
    float[] previousXPositions = new float[3]; // ���� 3���� xPos ���� ������ �迭 ����
    int currentIndex = 0; // ���� ������ �ε����� ��Ÿ���� ���� ����
    private List<GameObject> weaselObjects;
    private bool ReadyToJump = false;

    EventManager eventManager;
    List<GameObject> objects;

    private void OnEnable()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        objects = new List<GameObject>();
        startTime = Time.time;
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
    }

    private void StartPattern()
    {
        weaselCoroutine = StartCoroutine(TimingDivider());
    }

    private void StopPattern()
    {
        if (weaselCoroutine != null)
        {
            StopCoroutine(weaselCoroutine);
            weaselCoroutine = null;
        }

        if (currentWarning != null)
        {
            objects.Remove(currentWarning);
            Destroy(currentWarning);
            currentWarning = null;
        }
    }

    private IEnumerator TimingDivider()
    {
        Destroy(gameObject, 10f);
        weaselObjects = new List<GameObject>(); // �ʱ�ȭ�� ���⼭ ����
        //StartCoroutine(Pattern8bFirst());
        StartCoroutine(Pattern8bSecond());
        StartCoroutine(Pattern8bFly());
        yield return null;
    }

    /*
    private IEnumerator Pattern8bFirst()
    {
        for (int i = 0; i < firstweaselTimings.Length; i++)
        {
            float timing = firstweaselTimings[i];

            while (GetElapsedTime() < timing)
            {
                // ���� ��� �ð��� ������ Ÿ�ֿ̹� ������ ������ ��ٸ��ϴ�.
                yield return null;
            }
            if (currentIndex < previousXPositions.Length)
            {
                xPos = Random.Range(-8.3007f, 8.3007f);
                previousXPositions[currentIndex] = xPos;
            }
            else
            {
                do
                {
                    xPos = Random.Range(-8.3007f, 8.3007f);
                } while (IsWithinRangeOfPreviousXPositions(xPos));
                previousXPositions[currentIndex % previousXPositions.Length] = xPos;
            }

            currentIndex++;

            //��� ������Ʈ ����

            yPos = -4.5f;
            StartCoroutine(SpawnWeasel(xPos, yPos));
        }
    }
    */

    private IEnumerator Pattern8bSecond()
    {
        for (int i = 0; i < secondweaselTimings.Length; i++)
        {
            float timing = secondweaselTimings[i];

            while (GetElapsedTime() < timing)
            {
                yield return null;
            }
            StartCoroutine(SpawnWeasels(i));
        }
    }

    private IEnumerator Pattern8bFly()
    {
        while (weaselObjects.Count != 7)
        {
            yield return null;
        }
        StartCoroutine(FlyingWeasels(weaselObjects));
    }

    //2023.08.27 ���� ��� ����
    private IEnumerator SpawnWeasel(float xPos, float yPos)
    {
        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(weaselwarning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

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

        Vector3 spawnPosition = new Vector3(xPos, -8f, 0f);

        GameObject newWeasel = Instantiate(weasel, spawnPosition, Quaternion.identity);
        objects.Add(newWeasel);
        Rigidbody2D weaselRigidbody = newWeasel.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * weaselspeed;

        while (newWeasel.transform.position.y < -3.963f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;
        newWeasel.transform.position = new Vector3(newWeasel.transform.position.x, -4.6f, newWeasel.transform.position.z);

        yield return new WaitForSeconds(0.5f);

        weaselRigidbody.velocity = Vector2.down * weaselspeed;
        StartCoroutine(DestroyIfOutOfBounds(newWeasel));
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator SpawnWeasels(int i)
    {
        Vector3 warningPosition = warningPositions[i];
        GameObject newWarning = Instantiate(weaselwarning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
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

        GameObject weaselObject = Instantiate(weasel, firstPositions[i], Quaternion.identity);
        objects.Add(weaselObject);
        weaselObjects.Add(weaselObject);

        Rigidbody2D weaselRigidbody = weaselObject.GetComponent<Rigidbody2D>();
        weaselRigidbody.velocity = Vector2.up * 5f;

        while (weaselObject.transform.position.y < -4.5f)
        {
            yield return null;
        }

        weaselRigidbody.velocity = Vector2.zero;

        if (weaselObjects.Count == 7)
        {
            ReadyToJump = true;
        }
    }

    private IEnumerator FlyingWeasels(List<GameObject> weaselObjects)
    {
        while (!ReadyToJump)
        {
            yield return null;
        }

        for (int i = 0; i < weaselObjects.Count; i++)
        {
            GameObject weaselObject = weaselObjects[i];

            weaselObject.transform.GetChild(0).gameObject.SetActive(false);
            weaselObject.transform.GetChild(1).gameObject.SetActive(true);

            Rigidbody2D weaselObjectRigidbody = weaselObject.GetComponent<Rigidbody2D>();
            weaselObjectRigidbody.velocity = Vector2.up * 10f;
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < weaselObjects.Count; i++)
        {
            GameObject weaselObject = weaselObjects[i];

            Rigidbody2D weaselObjectRigidbody = weaselObject.GetComponent<Rigidbody2D>();
            weaselObjectRigidbody.velocity = Vector2.down * 10f;
        }

        yield return null;

        // Destroy weasel objects
        foreach (GameObject weaselObject in weaselObjects)
        {
            StartCoroutine(DestroyIfOutOfBounds(weaselObject));
        }
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
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        float minX = -11f;
        float maxX = 11f;
        float minY = -7f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    private float GetElapsedTime()
    {
        return Time.time - startTime;
    }

    private bool IsWithinRangeOfPreviousXPositions(float xPos)
    {
        foreach (float prevX in previousXPositions)
        {
            if (Mathf.Abs(prevX - xPos) < 1.5f)
            {
                return true;
            }
        }
        return false;
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
        eventManager.deathEvent -= deathEvent;
        Destroy(gameObject);
    }
}
