using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern10 : MonoBehaviour
{
    [SerializeField]
    private GameObject chestnut;
    [SerializeField]
    private GameObject chestnutProjectile;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private float chestnutSpeed;
    [SerializeField]
    private float splinterSpeed;
    [SerializeField]
    private float splinterInterval;

    private void OnEnable()
    {
        StartCoroutine(DropChestnuts());
    }

    private void OnDisable()
    {
        StopCoroutine(DropChestnuts());
    }

    private IEnumerator DropChestnuts()
    { 
        
        float xPos = UnityEngine.Random.Range(-7f, 7f);
        Vector3 chestnutPosition = new Vector3(xPos, 5.5f, 0f);

        // 경고 오브젝트 생성
        Vector3 warningPosition = new Vector3(xPos, chestnutPosition.y - 1f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        // ChestNut 오브젝트 생성
        GameObject newChestnut = Instantiate(chestnut, chestnutPosition, Quaternion.identity);
        Rigidbody2D chestnutRigidbody = newChestnut.GetComponent<Rigidbody2D>();
        chestnutRigidbody.velocity = Vector2.down * chestnutSpeed;

        Destroy(newWarning);

        yield return new WaitForSeconds(1f);

        ExplodeChestnut(newChestnut.transform.position);
        Destroy(newChestnut);
        yield return null;
    }

    private void ExplodeChestnut(Vector3 position)
    {
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
