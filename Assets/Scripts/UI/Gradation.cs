using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradation : MonoBehaviour
{
    private Image image; // �̹��� ������Ʈ

    void Start()
    {
        image = GetComponent<Image>();

        // �׶��̼� ���� ����
        Color color1 = new Color(1f, 1f, 1f, 0f); // �����ϰ�
        Color color2 = Color.white;

        // �׶��̼� ����
        ApplyVerticalGradient(color1, color2);
    }

    void ApplyVerticalGradient(Color color1, Color color2)
    {
        // �׶��̼� �ؽ�ó ����
        Texture2D texture = new Texture2D(1, 4);
        texture.SetPixel(0, 0, color1);
        texture.SetPixel(0, 1, color2);
        texture.Apply();

        // �̹����� �ؽ�ó ����
        image.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 2), new Vector2(0.5f, 0.5f));
    }
}
