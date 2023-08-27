using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    EventManager eventManager;
    List<GameObject> objects;

    private void OnEnable()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        objects = new List<GameObject>();
        StartCoroutine(DropChestnuts());
    }

    private void OnDisable()
    {
        StopCoroutine(DropChestnuts());
    }

    private void OnDestroy()
    {
        eventManager.deathEvent -= deathEvent;
    }

    private IEnumerator DropChestnuts()
    {

        float xPos = Random.Range(-8.124f, 8.124f);
        Vector3 chestnutPosition = new Vector3(xPos, 4.201f, 0f);

        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(xPos, 4.327f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);
        objects.Add(newWarning);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();

        // 경고 오브젝트가 0.5초에 걸쳐서 투명해지도록 알파값 조정
        Color originalColor = warningRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float totalTime = 0.5f; // 전체 시간 (0.5초)
        float fadeInDuration = 0.3f; // 0.3초 동안은 완전히 불투명하게 유지

        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            // 0.3초 동안은 완전히 불투명하게 유지
            if (elapsedTime <= fadeInDuration)
            {
                warningRenderer.color = originalColor;
            }
            // 그 이후 0.2초 동안에는 빠르게 투명해지도록 알파값 조정
            else //0.3초가 지남
            {
                float fadeOutDuration = totalTime - fadeInDuration; // 투명해지는 시간 (0.2초)
                warningRenderer.color = Color.Lerp(originalColor, targetColor, t);
            }

            yield return null;
        }

        // 경고 오브젝트 제거
        objects.Remove(newWarning);
        Destroy(newWarning);

        // ChestNut 오브젝트 생성
        GameObject newChestnut = Instantiate(chestnut, chestnutPosition, Quaternion.identity);
        objects.Add(newChestnut);
        Rigidbody2D chestnutRigidbody = newChestnut.GetComponent<Rigidbody2D>();
        chestnutRigidbody.velocity = Vector2.down * chestnutSpeed;

        yield return new WaitForSeconds(0.5f);

        ExplodeChestnut(newChestnut.transform.position);
        objects.Remove(newChestnut);
        Destroy(newChestnut);
        yield return null;
    }

    private void ExplodeChestnut(Vector3 position)
    {
        GameObject ChestNutBomb = Instantiate(chestnutbomb, position, Quaternion.identity);
        delayRemoval(ChestNutBomb, 0.2f).Forget();
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
            newSplinter.transform.rotation = Quaternion.Euler(0f, 0f, angleInDegrees - 90f); // -90도 회전

            StartCoroutine(DestroyIfOutOfBounds(newSplinter));

            Destroy(gameObject, 6f);
        }
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
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
