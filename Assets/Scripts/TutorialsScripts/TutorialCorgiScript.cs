using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCorgiScript : MonoBehaviour
{
    [SerializeField]
    private int transparencyValue; // 0���� 255 ������ ���� ���� ������ ����

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
    GameObject SpaceBarImage;
    [SerializeField]
    Sprite SpaceBar_PressedImage;
    [SerializeField]
    Sprite SpaceBar_UnPressedImage;

    [SerializeField]
    GameObject Shift_ButtonImage;
    [SerializeField]
    Sprite Shift_PressedImage;
    [SerializeField]
    Sprite Shift_UnPressedImage;

    [SerializeField]
    GameObject MouseImage;

    SpriteRenderer Asprite;
    SpriteRenderer Dsprite;
    SpriteRenderer SpaceBarSprite;
    SpriteRenderer ShiftSprite;

    private Tutorials2Manager tutorials2Manager;

    Rigidbody2D TutorialCorgiRig2D;

    GameObject TutorialCorgi_Bone;

    private void Start()
    {
        tutorials2Manager = GameObject.Find("Tutorials2Manager").GetComponent<Tutorials2Manager>();

        Asprite = A_ButtonImage.GetComponent<SpriteRenderer>();
        Dsprite = D_ButtonImage.GetComponent<SpriteRenderer>();
        SpaceBarSprite = SpaceBarImage.GetComponent<SpriteRenderer>();
        ShiftSprite = Shift_ButtonImage.GetComponent<SpriteRenderer>();

        TutorialCorgiRig2D = GetComponent<Rigidbody2D>();

        TutorialCorgi_Bone = GameObject.Find("TutorialCorgiProjectile");

        // �ڽ� ������Ʈ�� �ִ� ��� ��������Ʈ �������� ������
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        // ��� ��������Ʈ �������� ������ ����
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // ���� ���� 0���� 255 ���̷� ����
            transparencyValue = Mathf.Clamp(transparencyValue, 0, 255);

            // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
            float normalizedAlpha = transparencyValue / 255.0f;

            color.a = normalizedAlpha; // ���� �� ����
            renderer.color = color; // ����� ���� ����
        }
    }

    private void Update()
    {
        A_ButtonImage.transform.position = new Vector3(transform.position.x - 0.6f, -1f, 0f);
        D_ButtonImage.transform.position = new Vector3(transform.position.x + 0.6f, -1f, 0f);
        SpaceBarImage.transform.position = new Vector3(transform.position.x + 3f, -1f, 0f);
        Shift_ButtonImage.transform.position = new Vector3(transform.position.x, -1f, 0f);

        if (TutorialCorgiRig2D.velocity.x > 0 && tutorials2Manager.IsArrivedRightSide == false)
        {
            Dsprite.sprite = D_PressedImage;
        }
        else
        {
            Dsprite.sprite = D_UnPressedImage;
        }

        if (TutorialCorgiRig2D.velocity.x < 0 && tutorials2Manager.IsArrivedRightSide == true && tutorials2Manager.IsFinishedMoveLeftAndRightTest == false)
        {
            Asprite.sprite = A_PressedImage;
        }
        else
        {
            Asprite.sprite = A_UnPressedImage;
        }

        if (TutorialCorgiRig2D.velocity.y > 0 && tutorials2Manager.IsFinishedMoveLeftAndRightTest == true && tutorials2Manager.IsFinishedJumpTest == false)
        {
            SpaceBarSprite.sprite = SpaceBar_PressedImage;
        }
        else
        {
            SpaceBarSprite.sprite = SpaceBar_UnPressedImage;
        }

        if (TutorialCorgiRig2D.velocity.x > 5 && tutorials2Manager.IsFinishedJumpTest == true && tutorials2Manager.IsFinishedDashTest == false)
        {
            ShiftSprite.sprite = Shift_PressedImage;
        }
        else
        {
            ShiftSprite.sprite = Shift_UnPressedImage;
        }

        if (tutorials2Manager.IsFinishedMoveLeftAndRightTest == true)
        {
            A_ButtonImage.SetActive(false);
            D_ButtonImage.SetActive(false);
            SpaceBarImage.SetActive(true);
        }

        if (tutorials2Manager.IsFinishedJumpTest == true)
        {
            SpaceBarImage.SetActive(false);
            Shift_ButtonImage.SetActive(true);
        }

        if (tutorials2Manager.IsFinishedDashTest == true && tutorials2Manager.IsFinishedTeleportTest == false)
        {
            Shift_ButtonImage.SetActive(false);
            MouseImage.SetActive(true); 

            StartCoroutine(ForeverBlinking(TutorialCorgi_Bone, 50f, 255f));
        }

        if (tutorials2Manager.IsFinishedTeleportTest == true)
        {
            A_ButtonImage.SetActive(false);
            D_ButtonImage.SetActive(false);
            SpaceBarImage.SetActive(false);
            Shift_ButtonImage.SetActive(false);
            MouseImage.SetActive(false);

            TutorialCorgi_Bone.SetActive(false);

            GetComponent<TutorialCorgiUIScript>().TutorialCorgiUI.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(collision.gameObject.layer))
        {
            tutorials2Manager.IsFirstHited = true;
        }
    }

    private IEnumerator ForeverBlinking(GameObject gameobject, float finalAlpha, float initialAlpha)
    {
        float elapsedTime = 0f;
        float fadeDuration = 0.5f;

        while (true) 
        { 
            //TestCorgiFadeIn
            while (elapsedTime < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(finalAlpha, initialAlpha, elapsedTime / fadeDuration); //���� �������� �ʱ� �������� �ٲ� �ۼ��� �� ����.

                // 0���� 255 ������ ������ ���� ����
                currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

                SpriteRenderer[] renderers = gameobject.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // ���� �� ����
                    renderer.color = color; // ����� ���� ����
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;

            //TestCorgiFadeOut
            while (elapsedTime < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / fadeDuration); //���� �������� �ʱ� �������� �ٲ� �ۼ��� �� ����.

                // 0���� 255 ������ ������ ���� ����
                currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

                SpriteRenderer[] renderers = gameobject.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer renderer in renderers)
                {
                    Color color = renderer.color;

                    // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
                    float normalizedAlpha = currentAlpha / 255.0f;

                    color.a = normalizedAlpha; // ���� �� ����
                    renderer.color = color; // ����� ���� ����
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
        }
    }
}
