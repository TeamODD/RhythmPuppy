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
    [SerializeField]
    private GameObject PlayerCorgi;

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
        float randomZRotation;
        float PlayerCorgi_Xpos = PlayerCorgi.transform.position.x;

        if (PlayerCorgi_Xpos < 0f)
        {
            randomZRotation = Random.Range(30f, 60f);
        }
        else
        {
            randomZRotation = Random.Range(-30f, -60f);
        }

        // 경고 오브젝트 생성
        float warningXPos = 0f;
        float warningYPos = -5f;
        float warningZPos = 0f;

        Vector3 warningPosition = new Vector3(warningXPos, warningYPos, warningZPos);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);

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

        // 경고 오브젝트 제거
        Destroy(newWarning);

        float tanValue = Mathf.Tan(randomZRotation * Mathf.PI / 180f + Mathf.PI / 2);

        float thorstemPosX;
        if (PlayerCorgi_Xpos < 0f)
        {
            thorstemPosX = 10f;
        }
        else
        {
            thorstemPosX = -10f;
        }
        float thorstemPosY = tanValue * (thorstemPosX - warningXPos) + warningYPos;

        Vector3 thorstemPosition = new Vector3(thorstemPosX, thorstemPosY, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        newthorstem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation); // 회전 방향 설정

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();

        // 회전된 각도를 사용하여 벡터 회전
        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, randomZRotation) * Vector2.up;

        // 대각선 이동 속도 계산
        Vector2 diagonalVelocity = diagonalDirection.normalized * thorwingspeed;

        newthorstemRigidbody.velocity = diagonalVelocity;

        yield return new WaitUntil(() => newthorstem.transform.position.y >= 0f);

        newthorstemRigidbody.velocity = Vector2.zero;

        yield return FadeOut(newthorstem, 255f, 0f);

        Destroy(newthorstem);
        Destroy(gameObject);
    }

    private IEnumerator FadeOut(GameObject obj, float initialAlpha, float finalAlpha)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //최종 투명도값과 초기 투명도값을 바꿔 작성한 게 맞음.

            // 0에서 255 사이의 값으로 투명도 제한
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer renderer in renderers)
            {
                Color color = renderer.color;

                // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
                float normalizedAlpha = currentAlpha / 255.0f;

                color.a = normalizedAlpha; // 투명도 값 변경
                renderer.color = color; // 변경된 투명도 설정
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
