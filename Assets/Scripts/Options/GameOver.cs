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

    private IEnumerator GameoverSetting2() //검은색 커튼에 맞춰 모습을 드러내는 1안과, 검은색 커튼이 완전히 내려간 뒤 모습을 드러내는 2안이 존재.
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

        float startTime = Time.time; // 시작 시간 저장

        while (Time.time - startTime < fadeDuration) // 경과 시간이 fadeDuration보다 작을 때까지 반복
        {
            float elapsedTime = Time.time - startTime; // 경과 시간 계산
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            image.color = newColor;

            yield return null;
        }

        // 알파 값을 정확하게 1로 설정하여 완전히 불투명하게 만듭니다.
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
