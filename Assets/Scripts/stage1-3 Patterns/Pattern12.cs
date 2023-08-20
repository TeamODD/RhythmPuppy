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
    private float thorwingspeed;

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
        Vector3 warningPosition = new Vector3(-4.56f, -4.49f, 0f);
        GameObject newWarning = Instantiate(warning, warningPosition, Quaternion.identity);

        float randomZRotation = Random.Range(-80f, -40f);
        newWarning.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);

        
        SpriteRenderer warningRenderer = newWarning.GetComponent<SpriteRenderer>();
        Color originalColor = warningRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float totalTime = 0.5f; 
        float fadeInDuration = 0.3f; 
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            if (elapsedTime <= fadeInDuration)
            {
                warningRenderer.color = originalColor;
            }
            else 
            {
                float fadeOutDuration = totalTime - fadeInDuration; // ���������� �ð� (0.2��)
                warningRenderer.color = Color.Lerp(originalColor, targetColor, t);
            }

            yield return null;
        }

        Destroy(newWarning);

        
        Vector3 thorstemPosition = new Vector3(-18.56f, -18.49f, 0f);
        GameObject newthorstem = Instantiate(thorstem, thorstemPosition, Quaternion.identity);

        newthorstem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation); // ȸ�� ���� ����

        Rigidbody2D newthorstemRigidbody = newthorstem.GetComponent<Rigidbody2D>();

        Vector2 diagonalDirection = Quaternion.Euler(0f, 0f, randomZRotation) * Vector2.up;
        

        Vector2 diagonalVelocity = diagonalDirection.normalized * thorwingspeed;

        newthorstemRigidbody.velocity = diagonalVelocity;

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
