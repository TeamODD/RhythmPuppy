using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    GameObject BlackBackground;
    [SerializeField]
    float OpeningSpeed;
    [SerializeField]
    GameObject Restart;
    [SerializeField]
    GameObject ExitToMenu;
    [SerializeField]
    float fadeDuration;

    private void OnEnable()
    {
        StartCoroutine(GameoverSetting1());
        StartCoroutine(GameoverSetting2());

        foreach (GameObject obj in new GameObject[] { Restart, ExitToMenu })
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("���ӿ��� â ���� ��ŵ");

            BlackBackground.SetActive(false);
            StopCoroutine(GameoverSetting2());

            foreach (GameObject obj in new GameObject[] { Restart, ExitToMenu })
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                Color originalColor = spriteRenderer.color;
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            }
        }
    }

    private IEnumerator GameoverSetting1()
    {
        BlackBackground.SetActive(true);
        Rigidbody2D bbgRigidbody = BlackBackground.GetComponent<Rigidbody2D>();
        bbgRigidbody.velocity = Vector2.down * OpeningSpeed;

        while (BlackBackground.transform.position.y >= -11)
        {
            yield return null;
        }

        bbgRigidbody.velocity = Vector2.zero;
        BlackBackground.SetActive(false);
        StartCoroutine(GameoverSetting2());
    }

    private IEnumerator GameoverSetting2() //������ Ŀư�� ���� ����� �巯���� 1�Ȱ�, ������ Ŀư�� ������ ������ �� ����� �巯���� 2���� ����.
    {
        while (BlackBackground.transform.position.y >= -5.67f)
        {
            yield return null;
        }
        StartCoroutine(FadeInObjects(Restart));
        
        while (BlackBackground.transform.position.y >= -7.58f)
        {
            yield return null;
        }
        StartCoroutine(FadeInObjects(ExitToMenu));
        
        yield return null;
    }

    private IEnumerator FadeInObjects(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;

        float startTime = Time.time; // ���� �ð� ����

        while (Time.time - startTime < fadeDuration) // ��� �ð��� fadeDuration���� ���� ������ �ݺ�
        {
            float elapsedTime = Time.time - startTime; // ��� �ð� ���
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            spriteRenderer.color = newColor;

            yield return null;
        }

        // ���� ���� ��Ȯ�ϰ� 1�� �����Ͽ� ������ �������ϰ� ����ϴ�.
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
