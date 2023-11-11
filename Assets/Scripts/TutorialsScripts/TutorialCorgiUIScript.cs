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

    //���̵�����, �۾� ����
    private IEnumerator TestCorgiFadeIn(float initialAlpha, float finalAlpha, GameObject targetObject)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //initialAlpha --> finalAlpha ����!

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
        // ���� ���� ���� ���
        float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha,
            Mathf.InverseLerp(initialposition, targetposition, transform.position.x));

        // �ڽ� ������Ʈ�� �ִ� ��� ��������Ʈ �������� ������
        SpriteRenderer[] renderers = targetObject.GetComponentsInChildren<SpriteRenderer>();
        Image[] images = targetObject.GetComponentsInChildren<Image>();

        // ��� ��������Ʈ �������� ������ ����
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // ���� �� ����
            renderer.color = color; // ����� ���� ����
        }

        foreach (Image image in images)
        {
            Color color = image.color;

            // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // ���� �� ����
            image.color = color; // ����� ���� ����
        }

        yield return null;
    }
}
