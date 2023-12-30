using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GotoSelectStage : MonoBehaviour
{
    [SerializeField]
    GameObject DevelopmentTeam;
    [SerializeField]
    GameObject LicenseNotice;
    [SerializeField]
    GameObject Headphone;
    [SerializeField]
    GameObject TitleImage;
    [SerializeField]
    GameObject RhythmPuppyText;
    [SerializeField]
    GameObject PressAnyKeyToPlayGame;
    [SerializeField]
    GameObject ParticleSystem;
    [SerializeField]
    float fadeDuration;
    [SerializeField]
    float comedownspeed;

    private bool FadeInDone = false;
    private bool FadeOutDone = false;

    private bool IsDevelopmentTeamSettingDone = false;
    private bool IsLicenseNoticeSettingDone = false;
    private bool IsHeadphoneSettingDone = false;
    private bool TitleSettingDone = false;

    IEnumerator DevelopmentTeamCoroutine;
    IEnumerator LicenseNoticeCoroutine;
    IEnumerator HeadphoneCoroutine;
    IEnumerator TitleImageCoroutine;

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.anyKeyDown) && TitleSettingDone)
        {
            PlaySelectSound.instance.SelectSound();
            PlaySelectSound.instance.audioSourceSelect.Play();

            SceneManager.LoadScene("SceneMenu_01");
        }

        if ((Input.GetMouseButtonDown(0)))
        {
            if (IsDevelopmentTeamSettingDone == false)
            {
                IsDevelopmentTeamSettingDone = true;
                StopCoroutine(DevelopmentTeamCoroutine);
                StartCoroutine(LicenseNoticeCoroutine);
                DevelopmentTeam.SetActive(false);
                LicenseNotice.SetActive(true);
            }
            else if (IsDevelopmentTeamSettingDone && !IsLicenseNoticeSettingDone)
            {
                IsLicenseNoticeSettingDone = true;
                StopCoroutine(LicenseNoticeCoroutine);
                StartCoroutine(HeadphoneCoroutine);
                LicenseNotice.SetActive(false);
                Headphone.SetActive(true);
            }
            else if (IsLicenseNoticeSettingDone && !IsHeadphoneSettingDone)
            {
                IsHeadphoneSettingDone = true;
                StopCoroutine(HeadphoneCoroutine);
                StartCoroutine(TitleImageCoroutine);
                StartCoroutine(FadeInObjects(TitleImage));
                Headphone.SetActive(false);
                TitleImage.SetActive(true);
                RhythmPuppyText.SetActive(true);
                PressAnyKeyToPlayGame.SetActive(true);

                TitleSettingDone = true;
            }
        }

        if (FadeInDone && TitleSettingDone)
        {
            FadeInDone = false;
            StartCoroutine(FadeOutText(PressAnyKeyToPlayGame));
        }
        else if (FadeOutDone && TitleSettingDone)
        {
            FadeOutDone = false;
            StartCoroutine(FadeInText(PressAnyKeyToPlayGame));
        }
    }

    private void Start()
    {
        ParticleSystem.GetComponent<ParticleSystem>().Stop();

        //아무런 행동도 하지 않았을 경우 진행되는 순서
        DevelopmentTeamCoroutine = DevelopmentTeamSetting(DevelopmentTeam, 3f);
        LicenseNoticeCoroutine = LicenseNoticeSetting(LicenseNotice, 3f);
        HeadphoneCoroutine = HeadphoneSetting(Headphone, 3f);
        TitleImageCoroutine = TitleSetting();

        StartCoroutine(DevelopmentTeamCoroutine);
    }

    private IEnumerator DevelopmentTeamSetting(GameObject obj, float time)
    {
        DevelopmentTeam.SetActive(true);

        //FadeIn 페이드 인, 등장
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        float startTime = Time.time; // Save the start time

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(time); //대기시간

        //FadeOut 페이드 아웃, 퇴장
        startTime = Time.time;

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        IsDevelopmentTeamSettingDone = true;

        DevelopmentTeam.SetActive(false);
        yield return StartCoroutine(LicenseNoticeCoroutine);
    }

    private IEnumerator LicenseNoticeSetting(GameObject obj, float time)
    {
        LicenseNotice.SetActive(true);

        //FadeIn 페이드 인, 등장
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        float startTime = Time.time; // Save the start time

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(time); //대기시간

        //FadeOut 페이드 아웃, 퇴장
        startTime = Time.time;

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        IsLicenseNoticeSettingDone = true;

        LicenseNotice.SetActive(false);
        yield return StartCoroutine(HeadphoneCoroutine);
    }

    private IEnumerator HeadphoneSetting(GameObject obj, float time)
    {
        Headphone.SetActive(true);

        //FadeIn 페이드 인, 등장
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        float startTime = Time.time; // Save the start time

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(time); //대기시간

        //FadeOut 페이드 아웃, 퇴장
        startTime = Time.time;

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        IsHeadphoneSettingDone = true;

        Headphone.SetActive(false);

        StartCoroutine(TitleImageCoroutine);
        StartCoroutine(FadeInObjects(TitleImage));
    }

    private IEnumerator TitleSetting()
    {
        TitleImage.SetActive(true);
        RhythmPuppyText.SetActive(true);
        PressAnyKeyToPlayGame.SetActive(true);

        SpriteRenderer renderer = TitleImage.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        TextMeshProUGUI textMeshPro = PressAnyKeyToPlayGame.GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f);
        }

        RhythmPuppyText.transform.position = new Vector3(0f, 10f, 0f);
        Rigidbody2D RhythmPuppyTextRid2D = RhythmPuppyText.GetComponent<Rigidbody2D>();
        RhythmPuppyTextRid2D.velocity = Vector2.down * comedownspeed;

        while (RhythmPuppyText.transform.position.y > 1.53)
        {
            yield return null;
        }

        RhythmPuppyTextRid2D.velocity = Vector2.zero;
        StartCoroutine(FadeInText(PressAnyKeyToPlayGame));
        fadeDuration = 1f;

        ParticleSystem.GetComponent<ParticleSystem>().Play();

        TitleSettingDone = true;
    }

    private IEnumerator FadeInObjects(GameObject obj)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        float startTime = Time.time; // Save the start time

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    private IEnumerator FadeOutObjects(GameObject obj)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        float startTime = Time.time; // Save the start time

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            renderer.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    private IEnumerator FadeInText(GameObject obj)
    {
        TextMeshProUGUI textMeshPro = obj.GetComponent<TextMeshProUGUI>(); // Get the TextMeshPro component
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found.");
            yield break;
        }

        Color32 originalColor = textMeshPro.color;

        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
            textMeshPro.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        FadeInDone = true;
    }

    private IEnumerator FadeOutText(GameObject obj)
    {
        TextMeshProUGUI textMeshPro = obj.GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found.");
            yield break;
        }

        Color32 originalColor = textMeshPro.color;

        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration) // Repeat until elapsed time is less than fadeDuration
        {
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            textMeshPro.color = newColor;

            yield return null;
        }

        // Set the alpha value to exactly 1 to make it completely opaque.
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        FadeOutDone = true;
    }
}
