using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pattern7b : MonoBehaviour
{
    [SerializeField]
    private GameObject redapple;
    [SerializeField]
    private GameObject greenapple;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float redappleSpeed;
    [SerializeField]
    private float greenappleSpeed;

    private List<float> RedApplepatternTimings = new List<float> {0f, 0.4f, 0.8f, 1.2f, 1.8f};
    private List<float> GreenApplepatternTimings = new List<float> {2f, 2.4f, 2.8f, 3f, 3.4f, 3.8f};

    private void OnEnable()
    {
        StartCoroutine(DropRedapple());
    }

    private void OnDisable()
    {
        StopCoroutine(DropRedapple());
        StopCoroutine(DropGreenapple());
    }

    private IEnumerator DropRedapple()
    {
        // 원하는 타이밍에 따라 패턴을 실행합니다.
        for (int i = 0; i < RedApplepatternTimings.Count; i++)
        {
            float timing = RedApplepatternTimings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }

            // 경고 오브젝트 생성
            float xPos = UnityEngine.Random.Range(-7f, 7f);
            Vector3 warningPosition = new Vector3(xPos, 4.5f, 0f);
            GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

            SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
            if (warningRenderer != null)
            {
                warningRenderer.sortingOrder = int.MaxValue;
            }

            Destroy(newWarning, 0.5f);

            // 원하는 타이밍에 패턴을 실행합니다.
            Vector3 RedApplePosition = new Vector3(xPos, 7f, 0f);

            // Chestnut 오브젝트 생성
            GameObject newRedApple = Instantiate(redapple, RedApplePosition, Quaternion.identity);
            Rigidbody2D RedAppleRigidbody = newRedApple.GetComponent<Rigidbody2D>();
            RedAppleRigidbody.velocity = Vector2.down * redappleSpeed;

            StartCoroutine(DestroyIfOutOfBounds(newRedApple));
        }
        StopCoroutine(DropRedapple());
        StartCoroutine(DropGreenapple());
    }

    private IEnumerator DropGreenapple()
    {
        for (int i = 0; i < GreenApplepatternTimings.Count; i++)
        {
            float timing = GreenApplepatternTimings[i];

            while (Time.time < timing)
            {
                // 현재 경과 시간이 지정된 타이밍에 도달할 때까지 기다립니다.
                yield return null;
            }

            // 경고 오브젝트 생성
            float xPos = UnityEngine.Random.Range(-7f, 7f);
            Vector3 warningPosition = new Vector3(xPos, 4.5f, 0f);
            GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

            Destroy(newWarning, 0.5f);

            // 원하는 타이밍에 패턴을 실행합니다.
            Vector3 GreenApplePosition = new Vector3(xPos, 7f, 0f);

            // Chestnut 오브젝트 생성
            GameObject newGreenApple = Instantiate(greenapple, GreenApplePosition, Quaternion.identity);
            Rigidbody2D GreenAppleRigidbody = newGreenApple.GetComponent<Rigidbody2D>();
            GreenAppleRigidbody.velocity = Vector2.down * greenappleSpeed;

            StartCoroutine(DestroyIfOutOfBounds(newGreenApple));
        }

        // 패턴이 모두 실행되면 스크립트를 비활성화합니다.
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (true)
        {
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
            if (!IsWithinMapBounds(obj.transform.position))
            {
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
        float maxY = 10f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
