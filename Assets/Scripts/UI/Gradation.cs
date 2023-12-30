using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradation : MonoBehaviour
{
    private Image image; // 이미지 컴포넌트

    void Start()
    {
        image = GetComponent<Image>();

        // 그라데이션 색상 설정
        Color color1 = new Color(1f, 1f, 1f, 0f); // 투명하게
        Color color2 = Color.white;

        // 그라데이션 적용
        ApplyVerticalGradient(color1, color2);
    }

    void ApplyVerticalGradient(Color color1, Color color2)
    {
        // 그라데이션 텍스처 생성
        Texture2D texture = new Texture2D(1, 4);
        texture.SetPixel(0, 0, color1);
        texture.SetPixel(0, 1, color2);
        texture.Apply();

        // 이미지에 텍스처 적용
        image.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 2), new Vector2(0.5f, 0.5f));
    }
}
