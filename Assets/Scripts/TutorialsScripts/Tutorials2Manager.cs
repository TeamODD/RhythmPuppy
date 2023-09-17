using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
    bool IsFinishedMoveLeftAndRightTest = false;
    bool IsFinishedJumpTest = false;
    bool IsFinishedDashTest = false;


    IEnumerator Atest;
    IEnumerator Dtest;
    IEnumerator JumpTest;

    Animator TutorialCorgiAnim;

    [SerializeField]
    float TutorialCorgiSpeed;

    void Start()
    {
        Asprite = A_ButtonImage.GetComponent<SpriteRenderer>();
        Dsprite = D_ButtonImage.GetComponent<SpriteRenderer>();

        TutorialCorgiRig2D = TutorialCorgi.GetComponent<Rigidbody2D>();
        TutorialCorgiAnim = TutorialCorgi.GetComponent<Animator>();

        Atest = PleaseMoveToRightPuppy();
        Dtest = PleaseMoveToLeftPuppy();
        StartCoroutine(Atest);

        JumpTest = PleaseJumpToAvoid();

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

        //�÷��̾ ó������ �����ʿ� �������� ��
        if (PlayerCorgi.transform.position.x >= 5 && IsArrivedRightSide == false)
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
            StopCoroutine(Atest);
            TutorialCorgiRig2D.velocity = Vector2.zero;
            StartCoroutine(Dtest);
        }

        //�÷��̾ ���������� ������ ���� ���ʿ� �������� ��
        if (PlayerCorgi.transform.position.x <= -5 && IsArrivedRightSide == true)
        {
            IsFinishedMoveLeftAndRightTest = true;

            Puppy.transform.position = new Vector3(7.34f, -3.15f, 0);
            if (Puppy.transform.localScale.x < 0)
            {
                float scaleX = Puppy.transform.localScale.x;
                float scaleY = Puppy.transform.localScale.y;
                float scaleZ = Puppy.transform.localScale.z;
                Puppy.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }
            A_ButtonImage.SetActive(false);
            D_ButtonImage.SetActive(false);

            StopCoroutine(Dtest);
            TutorialCorgiRig2D.velocity = Vector2.zero;
            //StartCoroutine(JumpTest);
        }
    }

    private IEnumerator PleaseMoveToRightPuppy()
    {
        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        //���ʿ��� ����������
        while (!IsArrivedRightSide)
        {
            Puppy.transform.position = new Vector3(7.34f, -3.15f, 0);

            TutorialCorgi.transform.position = new Vector3(-4, -4.3012f, 0);

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            yield return StartCoroutine(TestCorgiFadeIn(finalAlpha, initialAlpha));

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            while (TutorialCorgi.transform.position.x < 5)
            {
                TutorialCorgiRig2D.velocity = Vector2.right * TutorialCorgiSpeed;
                yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
            }

            TutorialCorgiRig2D.velocity = Vector2.zero;
        }
    }

    private IEnumerator PleaseMoveToLeftPuppy()
    {
        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        //�����ʿ��� ��������
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

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            yield return StartCoroutine(TestCorgiFadeIn(finalAlpha, initialAlpha));

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            while (TutorialCorgi.transform.position.x > -5)
            {
                TutorialCorgiRig2D.velocity = Vector2.left * TutorialCorgiSpeed;
                yield return TestCorgiFadeOut(initialAlpha, finalAlpha, 4f, -5f);
            }

            TutorialCorgiRig2D.velocity = Vector2.zero;
        }
    }

    private IEnumerator PleaseJumpToAvoid()
    {
        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        while (!IsFinishedJumpTest)
        {
            TutorialCorgi.transform.position = new Vector3(-4, -4.3012f, 0);

            if (TutorialCorgi.transform.localScale.x < 0)
            {
                float scaleX = TutorialCorgi.transform.localScale.x;
                float scaleY = TutorialCorgi.transform.localScale.y;
                float scaleZ = TutorialCorgi.transform.localScale.z;
                TutorialCorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            yield return StartCoroutine(TestCorgiFadeIn(finalAlpha, initialAlpha)); //0���� 100���� �ö�� �̴ϴ�. ���ڸ� ���� �����Ͻø� ū���� �ƴ����� ���� �� ���ϴ�.
        }

        yield return null;
    }

    private IEnumerator TestCorgiFadeIn(float finalAlpha, float initialAlpha)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(finalAlpha, initialAlpha, elapsedTime / fadeDuration); //���� �������� �ʱ� �������� �ٲ� �ۼ��� �� ����.

            // 0���� 255 ������ ������ ���� ����
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 255f);

            SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

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
    }

    private IEnumerator TestCorgiFadeOut(float initialAlpha, float finalAlpha, float initialposition, float targetposition)
    {
        // ���� ���� ���� ���
        float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha,
            Mathf.InverseLerp(initialposition, targetposition, TutorialCorgi.transform.position.x));

        // �ڽ� ������Ʈ�� �ִ� ��� ��������Ʈ �������� ������
        SpriteRenderer[] renderers = TutorialCorgi.GetComponentsInChildren<SpriteRenderer>();

        // ��� ��������Ʈ �������� ������ ����
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
            float normalizedAlpha = currentAlpha / 255.0f;

            color.a = normalizedAlpha; // ���� �� ����
            renderer.color = color; // ����� ���� ����
        }

        yield return null;
    }
}
