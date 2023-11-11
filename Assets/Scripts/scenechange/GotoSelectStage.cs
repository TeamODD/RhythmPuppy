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
    GameObject TitleImage;
    [SerializeField]
    GameObject RhythmPuppyText;
    [SerializeField]
    GameObject PressAnyKeyToPlayGame;
    [SerializeField]
    float fadeDuration;
    [SerializeField]
    float comedownspeed;
    [SerializeField]
    GameObject LoadingScreen;
    [SerializeField]
    AudioSource volume;

    private float alpha;

    private bool FadeInDone = false;
    private bool FadeOutDone = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            PlaySelectSound.instance.SelectSound();
            PlaySelectSound.instance.audioSourceSelect.Play();

            StartCoroutine(SceneLoading());
        }

        if (FadeInDone)
        {
            FadeInDone = false;
            StartCoroutine(FadeOutText(PressAnyKeyToPlayGame));
        }
        else if (FadeOutDone)
        {
            FadeOutDone = false;
            StartCoroutine(FadeInText(PressAnyKeyToPlayGame));
        }
        
    }


    private void Start()
    {
        alpha = 0;
        StartCoroutine(TitleSetting());
        StartCoroutine(FadeInObjects(TitleImage));
    }

    private IEnumerator SceneLoading()
    {
        while (alpha < 1)
        {
            LoadingScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
            volume.volume = 1f - alpha;
            alpha += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        PlaySelectSound.instance.StartLoading("SceneMenu_01", LoadingScreen);
    }

    private IEnumerator TitleSetting()
    {
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
