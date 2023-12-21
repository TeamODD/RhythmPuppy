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
        // 날다람쥐가 시작하는 위치를 랜덤으로 선택합니다.
        float xPos = 0;
        float yPos = 4.293f;

        if (Random.Range(0, 2) == 0) // 왼쪽 위에서 시작
            xPos = -8.16f;
            
        else // 오른쪽 위에서 시작
            xPos = 8.16f;

        Vector3 warningPosition = new Vector3(xPos, yPos, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        float scaleX = newWarning.transform.localScale.x;
        float scaleY = newWarning.transform.localScale.y;
        float scaleZ = newWarning.transform.localScale.z;
        if (xPos == -8.16f) // 왼쪽 위에서 시작
        {
            newWarning.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }

        // 경고 오브젝트와 자식 오브젝트의 Sprite Renderer 배열 얻기
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
        //경고오브젝트 파괴
        Destroy(newWarning);

        //실질적인 패턴 시작
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        // 대각선 방향을 설정합니다.
        

        // 장애물을 생성하고 속도와 방향을 설정합니다.
        GameObject newSquirrel = Instantiate(flyingSquirrel, spawnPosition, Quaternion.identity);
        Rigidbody2D squirrelRigidbody = newSquirrel.GetComponent<Rigidbody2D>();

        scaleX = newSquirrel.transform.localScale.x;
        scaleY = newSquirrel.transform.localScale.y;
        scaleZ = newSquirrel.transform.localScale.z;

        float RandomZRoation = Random.Range(-225f, -250f); 
        if (xPos == -8.16f) // 왼쪽 위에서 시작
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
