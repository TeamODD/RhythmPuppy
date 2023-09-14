using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorials2Manager : MonoBehaviour
{
    [SerializeField]
    GameObject A_ButtonImage;
    [SerializeField]
    GameObject D_ButtonImage;
    [SerializeField]
    Sprite A_PressedImage;
    [SerializeField]
    Sprite D_PressedImage;
    [SerializeField]
    Sprite A_UnPressedImage;
    [SerializeField]    
    Sprite D_UnPressedImage;

    [SerializeField]
    GameObject TutorialCorgi;
    [SerializeField]
    GameObject PlayerCorgi;
    [SerializeField]
    GameObject Puppy;

    SpriteRenderer Asprite;
    SpriteRenderer Dsprite;

    Rigidbody2D TutorialCorgiRig2D;

    bool IsArrivedRightSide = false;

    [SerializeField]
    float TutorialCorgiSpeed;

    void Start()
    {
        Asprite = A_ButtonImage.GetComponent<SpriteRenderer>();
        Dsprite = D_ButtonImage.GetComponent<SpriteRenderer>();

        TutorialCorgiRig2D = TutorialCorgi.GetComponent<Rigidbody2D>();

        StartCoroutine(PleaseMoveToPuppy());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Asprite.sprite = A_PressedImage;
        }
        else
        {
            Asprite.sprite = A_UnPressedImage;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Dsprite.sprite = D_PressedImage;
        }
        else
        {
            Dsprite.sprite= D_UnPressedImage;
        }

        if (PlayerCorgi.transform.position.x >= 5)
        {
            IsArrivedRightSide = true;

            Puppy.transform.position = new Vector3(-7.34f, -3.15f, 0);
            if (Puppy.transform.localScale.x > 0)
            {
                float scaleX = Puppy.transform.localScale.x;
                float scaleY = Puppy.transform.localScale.y;
                float scaleZ = Puppy.transform.localScale.z;
                Puppy.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }
        }
    }

    private IEnumerator PleaseMoveToPuppy()
    {
        float initialAlpha = 100f; // 초기 투명도 값
        float finalAlpha = 0f;    // 최종 투명도 값

        //왼쪽에서 오른쪽으로
        while (!IsArrivedRightSide)
        {
            Puppy.transform.position = new Vector3(7.34f, -3.15f, 0);

            TutorialCorgi.transform.position = new Vector3(-4, -4.3012f, 0);
            Animator TutorialCorgiAnim = TutorialCorgi.GetComponent<Animator>();

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            float elapsedTime = 0f;
            float fadeDuration = 1.0f;

            while (elapsedTime < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(finalAlpha, initialAlpha, elapsedTime / fadeDuration); //최종 투명도값과 초기 투명도값을 바꿔 작성한 게 맞음.

                // 0에서 255 사이의 값으로 투명도 제한
                currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

                SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // 투명도 값 변경
                    renderer.color = color; // 변경된 투명도 설정
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            TutorialCorgiRig2D.velocity = Vector2.right * TutorialCorgiSpeed;

            while (TutorialCorgi.transform.position.x < 5)
            {
                // 현재 투명도 값을 계산
                float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha,
                    Mathf.InverseLerp(-4f, 5f, TutorialCorgi.transform.position.x));

                // 자식 오브젝트에 있는 모든 스프라이트 렌더러를 가져옴
                SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

                // 모든 스프라이트 렌더러의 투명도를 조절
                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // 투명도 값 변경
                    renderer.color = color; // 변경된 투명도 설정
                }

                yield return null;
            }

            TutorialCorgiRig2D.velocity = Vector2.zero;
        }

        //오른쪽에서 왼쪽으로
        while (IsArrivedRightSide)
        {
            TutorialCorgi.transform.position = new Vector3(4, -4.3012f, 0);

            if (TutorialCorgi.transform.localScale.x > 0)
            {
                float scaleX = TutorialCorgi.transform.localScale.x;
                float scaleY = TutorialCorgi.transform.localScale.y;
                float scaleZ = TutorialCorgi.transform.localScale.z;
                TutorialCorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            Animator TutorialCorgiAnim = TutorialCorgi.GetComponent<Animator>();

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            float elapsedTime = 0f;
            float fadeDuration = 1.0f;

            while (elapsedTime < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(finalAlpha, initialAlpha, elapsedTime / fadeDuration); //최종 투명도값과 초기 투명도값을 바꿔 작성한 게 맞음.

                // 0에서 255 사이의 값으로 투명도 제한
                currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

                SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // 투명도 값 변경
                    renderer.color = color; // 변경된 투명도 설정
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            TutorialCorgiRig2D.velocity = Vector2.left * TutorialCorgiSpeed;

            while (TutorialCorgi.transform.position.x > -5)
            {
                // 현재 투명도 값을 계산
                float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha,
                    Mathf.InverseLerp(4f, -5f, TutorialCorgi.transform.position.x));

                // 자식 오브젝트에 있는 모든 스프라이트 렌더러를 가져옴
                SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

                // 모든 스프라이트 렌더러의 투명도를 조절
                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // 투명도 값 변경
                    renderer.color = color; // 변경된 투명도 설정
                }

                yield return null;
            }

            TutorialCorgiRig2D.velocity = Vector2.zero;
        }
    }
}
