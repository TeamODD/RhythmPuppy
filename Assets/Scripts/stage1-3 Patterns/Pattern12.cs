using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern12 : MonoBehaviour
{
    [SerializeField]
    private GameObject thorstem;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float thorwingspeed; //가시덤블을 날리는 속도

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
        Vector3 warningPosition = new Vector3(-4.56f, -4.49f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        float randomZRotation = Random.Range(-80f, -40f);
        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);

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

        float tanValue = Mathf.Tan(Mathf.Deg2Rad * (90 + randomZRotation));
        float thorstemPosX = -20f;
        float thorstemPosY = -4.49f + (thorstemPosX + 4.56f) * tanValue;

        Debug.Log(Mathf.Tan(Mathf.Deg2Rad * -randomZRotation));
        Debug.Log(thorstemPosY);

        Vector3 thorstemPosition = new Vector3(thorstemPosX, thorstemPosY, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        newthorstem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation); // 회전 방향 설정

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();

        // 회전된 각도를 사용하여 벡터 회전
        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, randomZRotation) * Vector2.up;

        // 대각선 이동 속도 계산
        Vector2 diagonalVelocity = diagonalDirection.normalized * thorwingspeed;

        newthorstemRigidbody.velocity = diagonalVelocity;

        // 자식 오브젝트도 함께 움직이도록 설정
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = diagonalVelocity;
            }
        }

        while (newthorstem.transform.position.y < 5.79f)
        {
            yield return null;
        }

        Destroy(newthorstem);
        Destroy(gameObject);
    }
}
