using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWayScript : MonoBehaviour
{
    public int ClearIndex;
    SpriteRenderer spriteRenderer;

    Dictionary<int, float> clearIndexToSizeX = new Dictionary<int, float>
    {
        { 0, 15.02591f },
        { 1, 25.22173f },
        { 2, 34.50952f } //max=48
        // �߰� ClearIndex�� ���� ������ �ʿ信 ���� �߰�
    };

    float targetSizeX;
    float transitionSpeed = 2.0f; // ���ϴ� ��ȯ �ӵ�

    void Awake()
    {
        if (PlayerPrefs.HasKey("clearIndex"))
        {
            ClearIndex = PlayerPrefs.GetInt("clearIndex");
        }
        else
        {
            ClearIndex = 1; //���� ������ 1�� �ٲ��ּ���.
        }
    }

        void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        float sizeX = clearIndexToSizeX.ContainsKey(ClearIndex - 1) ? clearIndexToSizeX[ClearIndex - 1] : 15.02591f;
        spriteRenderer.size = new Vector2(sizeX, 1.2f);

        targetSizeX = clearIndexToSizeX.ContainsKey(ClearIndex) ? clearIndexToSizeX[ClearIndex] : 48f;
        StartCoroutine(ChangeSizeOverTime());
    }

    private IEnumerator ChangeSizeOverTime()
    {
        while (Mathf.Abs(spriteRenderer.size.x - targetSizeX) > 0.01f)
        {
            // ���� ũ�⿡�� ��ǥ ũ��� ������ ��ȭ
            float currentSizeX = Mathf.Lerp(spriteRenderer.size.x, targetSizeX, Time.deltaTime * transitionSpeed);
            spriteRenderer.size = new Vector2(currentSizeX, 1.2f);

            yield return null;
        }
    }
}
