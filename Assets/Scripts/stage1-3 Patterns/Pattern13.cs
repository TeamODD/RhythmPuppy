using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern13 : MonoBehaviour
{
    [SerializeField]
    private GameObject thorstem;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float reachspeed; //가시덤불이 솓아나는 속도
    [SerializeField]
    private float swingspeed; //가시덤불이 휘둘러지는 속도
    [SerializeField]
    private float rotationDuration; //가시덤불이 유지되는 시간

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
        Vector3 warningPosition = new Vector3(-4.86f, -0.84f, 0f);
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

        Vector3 thorstemPosition = new Vector3(-4.86f, -15.3f, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();
        newthorstemRigidbody.velocity = Vector2.up * reachspeed;

        // 자식 오브젝트도 함께 움직이도록 설정
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = newthorstemRigidbody.velocity;
            }
        }

        while (newthorstem.transform.position.y < -4.60f)
        {
            yield return null;
        }

        // 부모 오브젝트와 자식 오브젝트의 속도 초기화
        newthorstemRigidbody.velocity = Vector2.zero;
        foreach (Transform childTransform in newthorstem.transform)
        {
            Rigidbody2D childRigidbody = childTransform.GetComponent<Rigidbody2D>();
            if (childRigidbody != null)
            {
                childRigidbody.velocity = Vector2.zero;
            }
        }

        newthorstem.transform.Rotate(Vector3.back * swingspeed * Time.deltaTime);

        while (newthorstem.transform.rotation.eulerAngles.z > 250f)
        {
            newthorstem.transform.Rotate(Vector3.back * swingspeed * Time.deltaTime);
            yield return null;
        }

        Destroy(newthorstem);
        Destroy(gameObject);
    }
}
