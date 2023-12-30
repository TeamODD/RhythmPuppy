using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradation : MonoBehaviour
{
    private Image image; // �̹��� ������Ʈ

    public int textureX;
    public int textureY;
    public int whiteY;
    public int nullY;

    void Update()
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
        Texture2D texture = new Texture2D(textureX, textureY);
        texture.SetPixel(0, nullY, color1);
        texture.SetPixel(0, whiteY, color2);
        texture.Apply();

        // �̹����� �ؽ�ó ����
        image.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 2), new Vector2(0.5f, 0.5f));
    }
}
