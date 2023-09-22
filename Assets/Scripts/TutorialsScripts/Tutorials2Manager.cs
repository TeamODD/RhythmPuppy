using Cysharp.Threading.Tasks.Triggers;
using EventManagement;
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
    GameObject TutorialCorgi;
    [SerializeField]
    GameObject PlayerCorgi;
    [SerializeField]
    GameObject Puppy;

    [SerializeField]
    GameObject OakObstacle;
    [SerializeField]
    GameObject OakObstacleWarning;
    [SerializeField]
    GameObject ThorStemObstacle;
    [SerializeField]
    GameObject ThorStemObstacleWarning;

    SpriteRenderer Asprite;
    SpriteRenderer Dsprite;
    SpriteRenderer SpaceBarSprite;
    SpriteRenderer ShiftSprite;

    Rigidbody2D TutorialCorgiRig2D;

    [HideInInspector]
    public bool IsArrivedRightSide = false;
    [HideInInspector]
    public bool IsFinishedMoveLeftAndRightTest = false;
    [HideInInspector]
    public bool IsFinishedJumpTest = false;
    [HideInInspector]
    public bool IsFinishedDashTest = false;
    [HideInInspector]
    public bool IsFirstHited = false;

    GameObject NewOakObstacle;
    GameObject NewOakObstacleWarning;
    GameObject NewThorStemObstacle;
    GameObject NewThorStemObstacleWarning;

    IEnumerator Atest;
    IEnumerator Dtest;
    IEnumerator JumpTest;
    IEnumerator DashTest;

    Animator TutorialCorgiAnim;

    [SerializeField]
    float TutorialCorgiSpeed;

    private enum TestMode
    {
        ADTest,
        SpaceTest,
        DashTest,
        None
    }

    [SerializeField]
    private TestMode testMode = TestMode.None;

    void Start()
    {
        Asprite = A_ButtonImage.GetComponent<SpriteRenderer>();
        Dsprite = D_ButtonImage.GetComponent<SpriteRenderer>();

        SpaceBarSprite = SpaceBarImage.GetComponent<SpriteRenderer>();

        ShiftSprite = Shift_ButtonImage.GetComponent<SpriteRenderer>();

        TutorialCorgiRig2D = TutorialCorgi.GetComponent<Rigidbody2D>();
        TutorialCorgiAnim = TutorialCorgi.GetComponent<Animator>();

        Dtest = PleaseMoveToRightPuppy();
        Atest = PleaseMoveToLeftPuppy();
        JumpTest = PleaseJumpToAvoid();
        DashTest = PleaseDashToAvoid();

        if (testMode == TestMode.ADTest)
        {
            StartCoroutine(Dtest);
        }
        else if (testMode == TestMode.SpaceTest)
        {
            StartCoroutine(JumpTest);
        }
        else if (testMode == TestMode.DashTest)
        {
            StartCoroutine(DashTest);
        }
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

        if (Input.GetKey(KeyCode.Space))
        {
            SpaceBarSprite.sprite = SpaceBar_PressedImage;
        }
        else
        {
            SpaceBarSprite.sprite = SpaceBar_UnPressedImage;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ShiftSprite.sprite = Shift_PressedImage;
        }
        else
        {
            ShiftSprite.sprite = Shift_UnPressedImage;
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
            StopCoroutine(Dtest);
            StartCoroutine(Atest);
        }

        //�÷��̾ ���������� ������ ���� ���ʿ� �������� ��
        if (PlayerCorgi.transform.position.x <= -5 && IsArrivedRightSide == true && IsFinishedMoveLeftAndRightTest == false)
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

            StopCoroutine(Atest);
            StartCoroutine(JumpTest);
        }

        if (PlayerCorgi.transform.position.x >= 5 && IsFinishedMoveLeftAndRightTest == true && IsFinishedJumpTest == false)
        {
            IsFinishedJumpTest = true;

            SpaceBarImage.SetActive(false);
            if (NewOakObstacle.activeSelf == true)
            {
                Destroy(NewOakObstacle);
            }

            PlayerCorgi.transform.position = new Vector3(-7f, -4.3012f, 0f);

            StopCoroutine(JumpTest);
            StartCoroutine(DashTest);
        }

        if (PlayerCorgi.transform.position.x >= 5 && IsFinishedJumpTest == true && IsFinishedDashTest == false)
        {
            IsFinishedDashTest = true;

            Shift_ButtonImage.SetActive(false);
            if (NewThorStemObstacle.activeSelf == true)
            {
                Destroy(NewThorStemObstacle);
            }

            StopCoroutine(DashTest);
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

            TutorialCorgiRig2D.velocity = Vector2.zero;
            TutorialCorgi.transform.position = new Vector3(-4, -4.3012f, 0);

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            yield return StartCoroutine(TestCorgiFadeIn(finalAlpha, initialAlpha));

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            while (TutorialCorgi.transform.position.x < 5)
            {
                TutorialCorgiRig2D.velocity = Vector2.right * TutorialCorgiSpeed;
                yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
            }
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
            TutorialCorgiRig2D.velocity = Vector2.zero;

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
        }
    }

    private IEnumerator PleaseJumpToAvoid()
    {
        A_ButtonImage.SetActive(false);
        D_ButtonImage.SetActive(false);
        SpaceBarImage.SetActive(true);

        IsArrivedRightSide = true;
        IsFinishedMoveLeftAndRightTest = true;

        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        while (!IsFinishedJumpTest)
        {
            Vector3 OakObstcleWarningPosition = new Vector3(8.75f, -3.15f, 0f);
            NewOakObstacleWarning = Instantiate(OakObstacleWarning, OakObstcleWarningPosition, Quaternion.identity);
            NewOakObstacleWarning.SetActive(true);

            Vector3 OakObstclePosition = new Vector3(10.5f, -3.46f, 0f);
            NewOakObstacle = Instantiate(OakObstacle, OakObstclePosition, Quaternion.identity);
            NewOakObstacle.SetActive(true);

            TutorialCorgiRig2D.velocity = Vector2.zero;
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

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            bool IsJumped = false;

            if (IsFirstHited == true) //�¾���
            {
                while (TutorialCorgi.transform.position.x < 5)
                {
                    TutorialCorgiRig2D.velocity = new Vector2(TutorialCorgiSpeed, TutorialCorgiRig2D.velocity.y);

                    if (Mathf.Abs(TutorialCorgi.transform.position.x - NewOakObstacle.transform.position.x) < 4.5f && IsJumped == false)
                    {
                        TutorialCorgiRig2D.velocity = new Vector2(TutorialCorgiRig2D.velocity.x, 6f);
                        IsJumped = true;
                    }
                    IsFirstHited = false;
                    yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
                }
            }
            else if (IsFirstHited == false) //���� ����
            {
                while (TutorialCorgi.transform.position.x < 5)
                {
                    TutorialCorgiRig2D.velocity = Vector2.right * TutorialCorgiSpeed;
                    yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
                }
            }
            Destroy(NewOakObstacle);
        }
    }

    private IEnumerator PleaseDashToAvoid()
    {
        A_ButtonImage.SetActive(false);
        D_ButtonImage.SetActive(false);
        SpaceBarImage.SetActive(false);
        Shift_ButtonImage.SetActive(true);

        IsArrivedRightSide = true;
        IsFinishedMoveLeftAndRightTest = true;
        IsFinishedJumpTest = true;

        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        while (!IsFinishedDashTest)
        {
            StartCoroutine(RunPattern());

            TutorialCorgiRig2D.velocity = Vector2.zero;
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

            TutorialCorgiAnim.SetBool("bAxisInput", true);

            bool IsJumped = false;

            if (IsFirstHited == true) //�¾���
            {
                while (TutorialCorgi.transform.position.x < 5)
                {
                    TutorialCorgiRig2D.velocity = new Vector2(TutorialCorgiSpeed, TutorialCorgiRig2D.velocity.y);

                    if (Mathf.Abs(TutorialCorgi.transform.position.x - NewThorStemObstacle.transform.position.x) < 4.5f && IsJumped == false)
                    {
                        TutorialCorgiRig2D.velocity = new Vector2(TutorialCorgiRig2D.velocity.x + 6f, TutorialCorgiRig2D.velocity.y);
                        IsJumped = true;

                        yield return new WaitForSeconds(0.4f);
                    }
                    IsFirstHited = false;
                    yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
                }
            }
            else if (IsFirstHited == false) //���� ����
            {
                while (TutorialCorgi.transform.position.x < 5)
                {
                    TutorialCorgiRig2D.velocity = Vector2.right * TutorialCorgiSpeed;
                    yield return TestCorgiFadeOut(initialAlpha, finalAlpha, -4f, 5f);
                }
            }
            Destroy(NewThorStemObstacle);
        }
    }

    private IEnumerator RunPattern()
    {
        Vector3 ThorStemObstcleWarningPosition = new Vector3(8.2f, 0f, 0f);
        NewThorStemObstacleWarning = Instantiate(ThorStemObstacleWarning, ThorStemObstcleWarningPosition, Quaternion.identity);
        NewThorStemObstacleWarning.SetActive(true);

        SpriteRenderer[] warningRenderers = NewThorStemObstacleWarning.GetComponentsInChildren<SpriteRenderer>();

        Color targetColor = new Color(1f, 0.3f, 0.3f, 0f);
        foreach (SpriteRenderer renderer in warningRenderers)
        {
            renderer.color = targetColor;
        }

        float totalTime = 0.25f;
        float elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(targetColor, Color.red, t);
            }

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);

            foreach (SpriteRenderer renderer in warningRenderers)
            {
                renderer.color = Color.Lerp(Color.red, targetColor, t);
            }

            yield return null;
        }

        Destroy(NewThorStemObstacleWarning);

        Vector3 ThorStemObstaclePosition = new Vector3(10f, 0f, 0f);
        NewThorStemObstacle = Instantiate(ThorStemObstacle, ThorStemObstaclePosition, Quaternion.identity);
        NewThorStemObstacle.SetActive(true);

        Rigidbody2D stemRigidbody = NewThorStemObstacle.GetComponent<Rigidbody2D>();
        
        stemRigidbody.velocity = Vector2.left * 5f;

        while (NewThorStemObstacle.transform.position.x < -10)
        {
            Destroy(NewThorStemObstacle);
        }
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
