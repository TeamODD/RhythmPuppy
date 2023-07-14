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

    private void OnEnable()
    {
        StartCoroutine(MoveThornStem());
    }

    private void OnDisable()
    {
        StopCoroutine(MoveThornStem());
    }

    private IEnumerator MoveThornStem()
    {
        while (true)
        {
            // 왼쪽 또는 오른쪽 위치에서 시작
            float startX = Random.Range(0f, 1f) > 0.5f ? -7f : 7f;
            float startY = Random.Range(0f, 1f) > 0.5f ? -2.5f : 2.5f;
            Vector3 startPos = new Vector3(startX, startY, 0f);

            // 경고 오브젝트 생성
            Vector3 warningPosition = new Vector3(0.06f, startY, 0f);
            GameObject warning = Instantiate(thornStemWarning, warningPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.5f);

            // 가시 줄기 생성
            GameObject stem = Instantiate(thornStem, startPos, Quaternion.identity);
            Rigidbody2D stemRigidbody = stem.GetComponent<Rigidbody2D>();

            // 경고 오브젝트 제거
            Destroy(warning);

            // 오른쪽으로 이동
            if (startX < 0f)
                stemRigidbody.velocity = Vector2.right * stemSpeed;
            // 왼쪽으로 이동
            else
                stemRigidbody.velocity = Vector2.left * stemSpeed;


            StartCoroutine(DestroyIfOutOfBounds(stem));
        }
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
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
