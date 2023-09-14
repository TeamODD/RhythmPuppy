using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCorgiScript : MonoBehaviour
{
    [SerializeField]
    private int transparencyValue; // 0에서 255 사이의 투명도 값을 조절할 변수

    private void Start()
    {
        // 자식 오브젝트에 있는 모든 스프라이트 렌더러를 가져옴
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        // 모든 스프라이트 렌더러의 투명도를 조절
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // 투명도 값을 0에서 255 사이로 제한
            transparencyValue = Mathf.Clamp(transparencyValue, 0, 255);

            // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
            float normalizedAlpha = transparencyValue / 255.0f;

            color.a = normalizedAlpha; // 투명도 값 변경
            renderer.color = color; // 변경된 투명도 설정
        }
    }
}
