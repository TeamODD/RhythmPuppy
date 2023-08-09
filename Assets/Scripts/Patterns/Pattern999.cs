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
        // 날다람쥐가 시작하는 위치를 랜덤으로 선택합니다.
        float xPos;
        float yPos = 4.5f;

        if (Random.Range(0, 2) == 0) // 왼쪽 위에서 시작
            xPos = -8.385f;
            
        else // 오른쪽 위에서 시작
            xPos = 8.385f;

        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        if (warningRenderer != null)
        {
            warningRenderer.sortingOrder = int.MaxValue;
        }

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
        Destroy(newWarning);

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // 대각선 방향을 설정합니다.
        Vector2 direction = (xPos < 0f ? Vector2.right : Vector2.left) + Vector2.down;

        // 장애물을 생성하고 속도와 방향을 설정합니다.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();

        float scaleX = newSquirrel.transform.localScale.x;
        float scaleY = newSquirrel.transform.localScale.y;
        float scaleZ = newSquirrel.transform.localScale.z;
        if (xPos == -8.385f) // 왼쪽 위에서 시작
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
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
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
