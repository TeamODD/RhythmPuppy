using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern6 : MonoBehaviour
{
    [SerializeField]
    private GameObject thornStem;
    [SerializeField]
    private GameObject thornStemWarning;
    [SerializeField]
    private float stemSpeed = 3f;

    private bool isPatternRunning = false;
    private GameObject currentStem;

    private void OnEnable()
    {
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
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
            Destroy(currentStem);
            currentStem = null;
        }
        StopAllCoroutines();
    }

    private IEnumerator RunPattern()
    {
        while (isPatternRunning)
        {
            if (currentStem == null) // 현재 생성된 장애물이 없을 때만 패턴 실행
            {
                // 왼쪽 또는 오른쪽 위치에서 시작
                float startX = Random.Range(0f, 1f) > 0.5f ? -10f : 10f;
                float startY = 0;
                Vector3 startPos = new Vector3(startX, startY, 0f);

                // 경고 오브젝트 생성
                Vector3 warningPosition = new Vector3(startX, startY, 0f);
                GameObject warning = Instantiate(thornStemWarning, warningPosition, Quaternion.identity);

                yield return new WaitForSeconds(0.5f);

                // 가시 줄기 생성
                currentStem = Instantiate(thornStem, startPos, Quaternion.identity);
                Rigidbody2D stemRigidbody = currentStem.GetComponent<Rigidbody2D>();

                // 경고 오브젝트 제거
                Destroy(warning);

                // 오른쪽으로 이동
                if (startX < 0f)
                    stemRigidbody.velocity = Vector2.right * stemSpeed;
                // 왼쪽으로 이동
                else
                    stemRigidbody.velocity = Vector2.left * stemSpeed;

                StartCoroutine(DestroyIfOutOfBounds(currentStem));
            }

            yield return null;
        }
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (isPatternRunning)
        {
            // 맵 밖으로 나갈 경우 오브젝트를 파괴합니다.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                Destroy(obj);
                currentStem = null;
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
