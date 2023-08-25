using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pattern15 : MonoBehaviour
{
    [SerializeField]
    GameObject flower;
    [SerializeField]
    Sprite changedflower;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    float ComingOutSpeed;
    [SerializeField]
    float GetDownSpeed;

    private void OnEnable()
    {
        StartCoroutine(pattern());
    }

    private void OnDisable()
    {
        StopCoroutine(pattern());
    }

    private IEnumerator pattern()
    {
        // 경고 오브젝트 생성
        float Xpos = Random.Range(-5.05f, 7.868f);
        Vector3 warningPosition = new Vector3(Xpos, -3.416f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        // 경고 오브젝트가 0.5초에 걸쳐서 투명해지도록 알파값 조정
        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
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

        Vector3 flowerPosition = new Vector3(Xpos, -5.198f, 0f);
        GameObject newflower = Instantiate(flower, flowerPosition, Quaternion.identity);
        Rigidbody2D flowerRigidBody2D = newflower.GetComponent<Rigidbody2D>();

        //최초 등장
        flowerRigidBody2D.velocity = Vector2.up * 5;

        while (newflower.transform.position.y < -4.12f)
        {
            yield return null;
        }

        flowerRigidBody2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);
        ChangeImg(newflower);

        //본 모습 등장
        flowerRigidBody2D.velocity = Vector2.up * ComingOutSpeed;

        while (newflower.transform.position.y < -2.13)
        {
            yield return null;
        }

        flowerRigidBody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);

        flowerRigidBody2D.velocity = Vector2.down * GetDownSpeed;
        StartCoroutine(DestroyIfOutOfBounds(newflower));
    }

    private void ChangeImg(GameObject newflower)
    {
        new WaitForSeconds(0.3f);
        SpriteRenderer flowerSpriteRenderer = newflower.GetComponent<SpriteRenderer>();
        flowerSpriteRenderer.sprite = changedflower;
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
        float minY = -7f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
