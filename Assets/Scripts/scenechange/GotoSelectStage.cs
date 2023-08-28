using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown) 
        {
            PlaySelectSound.instance.SelectSound();
            PlaySelectSound.instance.audioSourceSelect.Play();

            SceneManager.LoadScene("SceneMenu_01");

        }
    }

    private void Start()
    {
        StartCoroutine(TitleSetting());
        StartCoroutine(FadeInObjects(TitleImage));
        StartCoroutine(FadeInText(PressAnyKeyToPlayGame));
    }

    private IEnumerator TitleSetting()
    {
        SpriteRenderer renderer = TitleImage.GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        RhythmPuppyText.transform.position = new Vector3(0f, 10f, 0f);
        Rigidbody2D RhythmPuppyTextRid2D = RhythmPuppyText.GetComponent<Rigidbody2D>();
        RhythmPuppyTextRid2D.velocity = Vector2.down * 5;

        while (RhythmPuppyText.transform.position.y > 1.53)
        {
            yield return null;
        }

        RhythmPuppyTextRid2D.velocity = Vector2.zero;
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
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Color32 originalColor = textMeshPro.color;
        Color32 transparentColor = new Color32(originalColor.r, originalColor.g, originalColor.b, 0);

        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color32 newVertexColor = Color32.Lerp(transparentColor, originalColor, t);

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                vertexColors[vertexIndex + 0] = newVertexColor;
                vertexColors[vertexIndex + 1] = newVertexColor;
                vertexColors[vertexIndex + 2] = newVertexColor;
                vertexColors[vertexIndex + 3] = newVertexColor;
            }

            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return null;
        }

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            vertexColors[vertexIndex + 0] = originalColor;
            vertexColors[vertexIndex + 1] = originalColor;
            vertexColors[vertexIndex + 2] = originalColor;
            vertexColors[vertexIndex + 3] = originalColor;
        }

        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
