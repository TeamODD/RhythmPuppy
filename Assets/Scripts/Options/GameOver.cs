using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject RestartArrow;
    [SerializeField]
    GameObject ExitToMenuArrow;
    [SerializeField]
    float fadeDuration;

    bool IsSettingsDone = false;

    private void OnEnable()
    {
        StartCoroutine(GameoverSetting1());
        StartCoroutine(GameoverSetting2());

        foreach (GameObject obj in new GameObject[] { Restart, ExitToMenu, RestartArrow, ExitToMenuArrow })
        {
            Image image = obj.GetComponent<Image>();
            Color originalColor = image.color;
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !IsSettingsDone)
        {
            BlackBackground.SetActive(false);
            StopCoroutine(GameoverSetting2());

            foreach (GameObject obj in new GameObject[] { Restart, ExitToMenu, RestartArrow, ExitToMenuArrow })
            {
                Image image = obj.GetComponent<Image>();
                Color originalColor = image.color;
                image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
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
    }

    private IEnumerator GameoverSetting2() //������ Ŀư�� ���� ����� �巯���� 1�Ȱ�, ������ Ŀư�� ������ ������ �� ����� �巯���� 2���� ����.
    {
        while (BlackBackground.transform.position.y >= -5.67f)
        {
            yield return null;
        }
        StartCoroutine(FadeInObjects(Restart));
        StartCoroutine(FadeInObjects(RestartArrow));
        
        while (BlackBackground.transform.position.y >= -7.58f)
        {
            yield return null;
        }
        StartCoroutine(FadeInObjects(ExitToMenu));
        StartCoroutine (FadeInObjects(ExitToMenuArrow));    
        
        yield return null;

        IsSettingsDone = true;
    }

    private IEnumerator FadeInObjects(GameObject obj)
    {
        Image image = obj.GetComponent<Image>();
        Color originalColor = image.color;

        float startTime = Time.time; // ���� �ð� ����

        while (Time.time - startTime < fadeDuration) // ��� �ð��� fadeDuration���� ���� ������ �ݺ�
        {
            float elapsedTime = Time.time - startTime; // ��� �ð� ���
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            image.color = newColor;

            yield return null;
        }

        // ���� ���� ��Ȯ�ϰ� 1�� �����Ͽ� ������ �������ϰ� ����ϴ�.
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
