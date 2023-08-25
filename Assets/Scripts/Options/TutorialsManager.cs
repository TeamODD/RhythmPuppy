using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField]
    GameObject Click;

    SpriteRenderer ClickSprite;
    [SerializeField]
    float blinkDuration = 0.5f;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        ClickSprite = Click.GetComponent<SpriteRenderer>();

        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        Color originalColor = ClickSprite.color; // �ʱ� ��������Ʈ ���� ����

        while (true)
        {
            // ���������� �κ�
            float elapsedTime = 0f;
            while (elapsedTime < blinkDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (blinkDuration / 2));

                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
                ClickSprite.color = newColor;

                yield return null;
            }

            // ������������ �κ�
            elapsedTime = 0f;
            while (elapsedTime < blinkDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (blinkDuration / 2));

                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
                ClickSprite.color = newColor;

                yield return null;
            }
        }
    }

}
