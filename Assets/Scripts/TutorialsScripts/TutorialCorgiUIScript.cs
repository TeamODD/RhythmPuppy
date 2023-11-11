using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCorgiUIScript : MonoBehaviour
{
    [SerializeField]
    public GameObject TutorialCorgiUI;
    [SerializeField]
    GameObject BoneIcon;
    [SerializeField]
    Sprite HP1sprite;
    [SerializeField]
    Sprite HP4sprite;

    Image StaminaGuageImage;

    SpriteRenderer BoneIconSprite;

    Rigidbody2D TutorialCorgiRig2D;

    private Tutorials2Manager tutorials2Manager;

    void Start()
    {
        StaminaGuageImage = GameObject.Find("TutorialCorgiStaminaGuage").GetComponent<Image>();
        BoneIconSprite = BoneIcon.GetComponent<SpriteRenderer>();
        tutorials2Manager = GameObject.Find("Tutorials2Manager").GetComponent<Tutorials2Manager>();
        TutorialCorgiRig2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TutorialCorgiUI.transform.position = new Vector3(transform.position.x + 0.15f, -2f, 0f);
    
        if (transform.position.x == -4f)
        {
            BoneIconSprite.sprite = HP1sprite;
        }

        if (TutorialCorgiRig2D.velocity.x > 5 && tutorials2Manager.IsFinishedJumpTest == true && tutorials2Manager.IsFinishedDashTest == false)
        {
            StaminaGuageImage.fillAmount = 0.75f;
        }
        else
        {
            StaminaGuageImage.fillAmount = Mathf.Clamp(StaminaGuageImage.fillAmount + 0.3f * Time.deltaTime, 0f, 1f);
        }

        if (transform.position.x < 5f)
        {
            TutorialCorgiUI.SetActive(true);
        }
        else
        {
            TutorialCorgiUI.SetActive(false);
        }

        if (tutorials2Manager.IsFinishedDashTest == true)
        {
            TutorialCorgiUI.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(collision.gameObject.layer))
        {
            if (TutorialCorgiRig2D.velocity.x > 5)
            {
                return;
            }
            BoneIconSprite.sprite = HP4sprite;
        }
    }

    //더미데이터, 작업 실패
    private IEnumerator TestCorgiFadeIn(float initialAlpha, float finalAlpha, GameObject targetObject)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //initialAlpha --> finalAlpha 주의!

            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            SpriteRenderer[] renderers = targetObject.GetComponentsInChildren<SpriteRenderer>();
            Image[] images = targetObject.GetComponentsInChildren<Image>();

            foreach (SpriteRenderer renderer in renderers)
            {
                Color color = renderer.color;

                float normalizedAlpha = currentAlpha / 255.0f;

                color.a = normalizedAlpha;
                renderer.color = color;
            }

            foreach (Image image in images)
            {
                Color color = image.color;

                float normalizedAlpha = currentAlpha / 255.0f;

                color.a = normalizedAlpha;
                image.color = color;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TestCorgiFadeOut(float initialAlpha, float finalAlpha, float initialposition, float targetposition, GameObject targetObject)
    {
        // 현재 투명도 값을 계산
        float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha,
            Mathf.InverseLerp(initialposition, targetposition, transform.position.x));

        // 자식 오브젝트에 있는 모든 스프라이트 렌더러를 가져옴
        SpriteRenderer[] renderers = targetObject.GetComponentsInChildren<SpriteRenderer>();
        Image[] images = targetObject.GetComponentsInChildren<Image>();

        // 모든 스프라이트 렌더러의 투명도를 조절
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // 투명도 값 변경
            renderer.color = color; // 변경된 투명도 설정
        }

        foreach (Image image in images)
        {
            Color color = image.color;

            // 0부터 255 범위의 값을 0부터 1 사이의 실수로 변환
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // 투명도 값 변경
            image.color = color; // 변경된 투명도 설정
        }

        yield return null;
    }
}
