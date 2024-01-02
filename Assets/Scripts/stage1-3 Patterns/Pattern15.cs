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
        float Xpos;
        if (Random.Range(0, 2) == 0)
        {
            Xpos = Random.Range(-7.98f, -2.32f);
        }
        else
        {
            Xpos = Random.Range(2.32f, 7.98f);
        }

        Vector3 warningPosition = new Vector3(Xpos, -3.416f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

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

        yield return new WaitForSeconds(0.4f);
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
