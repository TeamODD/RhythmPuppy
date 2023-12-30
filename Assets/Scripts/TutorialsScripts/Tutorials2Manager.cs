using Cysharp.Threading.Tasks.Triggers;
using EventManagement;
using SceneData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    GameObject MouseImage;
    [SerializeField]
    Sprite LeftMouseClickedImage;
    [SerializeField]
    Sprite RightMouseClickedImage;
    [SerializeField]
    Sprite MouseUnClickedImage;

    [SerializeField]
    GameObject TutorialCorgi_MouseImage;
    [SerializeField]
    Sprite TutorialCorgi_LeftMouseClickedImage;
    [SerializeField]
    Sprite TutorialCorgi_RightMouseClickedImage;
    [SerializeField]
    Sprite TutorialCorgi_MouseUnClickedImage;

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
    [SerializeField]
    GameObject ThorStems;
    [SerializeField]
    GameObject UICanvas;
    [SerializeField]
    GameObject BlackBox;

    SpriteRenderer Asprite;
    SpriteRenderer Dsprite;
    SpriteRenderer SpaceBarSprite;
    SpriteRenderer ShiftSprite;
    SpriteRenderer MouseSprite;
    SpriteRenderer TutorialCorgi_MouseSprite;

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
    public bool IsFinishedTeleportTest = false;
    [HideInInspector]
    public bool IsFirstHited = false;

    GameObject NewOakObstacle;
    GameObject NewOakObstacleWarning;
    GameObject NewThorStemObstacle;
    GameObject NewThorStemObstacleWarning;
    GameObject TutorialCorgi_Bone;

    IEnumerator Atest;
    IEnumerator Dtest;
    IEnumerator JumpTest;
    IEnumerator DashTest;
    IEnumerator TeleportTest;

    Animator TutorialCorgiAnim;
    AudioSource audioSource;

    [SerializeField]
    float TutorialCorgiSpeed;

    float startTime;

    List<float> PatternTimings = new List<float>();

    private enum TestMode
    {
        ADTest,
        SpaceTest,
        DashTest,
        TeleportTest,
        None
    }

    [SerializeField]
    private TestMode testMode = TestMode.None;

    private void Awake()
    {
        float interval = 2.0f;

        for (float timing = 0.9f; timing <= 294.0f; timing += interval)
        {
            PatternTimings.Add(timing);
        }

        startTime = 0f;
    }

    void Start()
    {
        Asprite = A_ButtonImage.GetComponent<SpriteRenderer>();
        Dsprite = D_ButtonImage.GetComponent<SpriteRenderer>();
        SpaceBarSprite = SpaceBarImage.GetComponent<SpriteRenderer>();
        ShiftSprite = Shift_ButtonImage.GetComponent<SpriteRenderer>();
        MouseSprite = MouseImage.GetComponent<SpriteRenderer>();
        TutorialCorgi_MouseSprite = TutorialCorgi_MouseImage.GetComponent<SpriteRenderer>();

        TutorialCorgiRig2D = TutorialCorgi.GetComponent<Rigidbody2D>();
        TutorialCorgiAnim = TutorialCorgi.GetComponent<Animator>();

        TutorialCorgi_Bone = TutorialCorgi.transform.GetChild(0).gameObject;

        audioSource = GameObject.FindWithTag("MusicManager").GetComponent<AudioSource>();

        Dtest = PleaseMoveToRightPuppy();
        Atest = PleaseMoveToLeftPuppy();
        JumpTest = PleaseJumpToAvoid();
        DashTest = PleaseDashToAvoid();
        TeleportTest = PleaseTeleportToAvoid();

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
        else if (testMode == TestMode.TeleportTest)
        {
            StartCoroutine(TeleportTest);
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

        if (Input.GetMouseButton(0))
        {
            MouseSprite.sprite = LeftMouseClickedImage;
        }
        else if (Input.GetMouseButton(1))
        {
            MouseSprite.sprite = RightMouseClickedImage;
        }
        else
        {
            MouseSprite.sprite = MouseUnClickedImage;
        }

        //�÷��̾ ó������ �����ʿ� �������� ��
        if (PlayerCorgi.transform.position.x >= 6 && IsArrivedRightSide == false)
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
        else if (PlayerCorgi.transform.position.x <= -6 && IsArrivedRightSide == true && IsFinishedMoveLeftAndRightTest == false)
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

        //�÷��̾ ���� �׽�Ʈ�� ������ ���
        else if (PlayerCorgi.transform.position.x >= 6 && IsFinishedMoveLeftAndRightTest == true && IsFinishedJumpTest == false)
        {
            IsFinishedJumpTest = true;

            SpaceBarImage.SetActive(false);

            if (NewOakObstacle != null && NewOakObstacle.activeSelf == true )
            {
                Destroy(NewOakObstacle);
            }

            PlayerCorgi.transform.position = new Vector3(-7f, -4.3012f, 0f);

            StopCoroutine(JumpTest);
            StartCoroutine(DashTest);
        }

        //�÷��̾ �뽬 �׽�Ʈ�� ������ ���
        else if (PlayerCorgi.transform.position.x >= 6 && IsFinishedJumpTest == true && IsFinishedDashTest == false)
        {
            IsFinishedDashTest = true;

            Shift_ButtonImage.SetActive(false);

            if (NewThorStemObstacle != null && NewThorStemObstacle.activeSelf == true)
            {
                Destroy(NewThorStemObstacle);
            }

            PlayerCorgi.transform.position = new Vector3(-7f, -4.3012f, 0f);

            StopCoroutine(DashTest);
            StartCoroutine(TeleportTest);
        }

        //�÷��̾ �ڷ����� �׽�Ʈ�� ������ ���
        else if (PlayerCorgi.transform.position.x >= 6 && IsFinishedDashTest == true && IsFinishedTeleportTest == false)
        {
            IsFinishedTeleportTest = true;

            ThorStems.SetActive(false);
            MouseImage.SetActive(false);

            StopCoroutine(TeleportTest);

            StartCoroutine(TutorialEnd());
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
            for (int i = 0; i < PatternTimings.Count; i++)
            {
                float timing = PatternTimings[i];

                if (timing < startTime)
                {
                    continue;
                }

                yield return new WaitForSeconds(timing - audioSource.time);
                StartCoroutine(RunOakPattern());

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
                yield return new WaitUntil(() => NewOakObstacle.transform.position.x <= -4.5f);
                Destroy(NewOakObstacle);
            }
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

        IsFirstHited = false;

        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        while (!IsFinishedDashTest)
        {
            StartCoroutine(RunThorStemPattern());

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
            yield return new WaitUntil(() => NewThorStemObstacle.transform.position.x <= -4.5f);
            Destroy(NewThorStemObstacle);
        }
    }

    private IEnumerator PleaseTeleportToAvoid()
    {
        A_ButtonImage.SetActive(false);
        D_ButtonImage.SetActive(false);
        SpaceBarImage.SetActive(false);
        Shift_ButtonImage.SetActive(false);
        MouseImage.SetActive(true);

        ThorStems.SetActive(true); //�ִϸ��̼����� ���� 2023.10.26

        IsArrivedRightSide = true;
        IsFinishedMoveLeftAndRightTest = true;
        IsFinishedJumpTest = true;
        IsFinishedDashTest = true;

        IsFirstHited = false;

        float initialAlpha = 100f; // �ʱ� ���� ��
        float finalAlpha = 0f;    // ���� ���� ��

        GameObject TutorialCorgi = GameObject.Find("TutorialCorgi");
        GameObject PlayerCorgi_Bone = PlayerCorgi.transform.GetChild(0).gameObject;

        Rigidbody2D TutorialCorgi_Bone_Rig2D = TutorialCorgi_Bone.GetComponent<Rigidbody2D>();

        TutorialCorgi_Bone.transform.SetParent(null);
        TutorialCorgi_Bone.transform.position = new Vector3(0.753f, -0.739f, 0f);
        PlayerCorgi_Bone.transform.position = new Vector3(1f, -0.6f, 0f);

        while (!IsFinishedTeleportTest)
        {
            TutorialCorgi_Bone_Rig2D.velocity = Vector2.zero;
            TutorialCorgiRig2D.velocity = Vector2.zero;
            TutorialCorgi.transform.position = new Vector3(-4, -4.3012f, 0);
            TutorialCorgi_Bone.transform.position = new Vector3(0.753f, -0.739f, 0f);
            TutorialCorgi_MouseImage.transform.position = new Vector3(TutorialCorgi_Bone.transform.position.x, TutorialCorgi_Bone.transform.position.y + 1.15f, 0f);

            if (TutorialCorgi.transform.localScale.x < 0)
            {
                float scaleX = TutorialCorgi.transform.localScale.x;
                float scaleY = TutorialCorgi.transform.localScale.y;
                float scaleZ = TutorialCorgi.transform.localScale.z;
                TutorialCorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            TutorialCorgiAnim.SetBool("bAxisInput", false);

            yield return StartCoroutine(TestCorgiFadeIn(finalAlpha, initialAlpha)); //0���� 100���� �ö�� �̴ϴ�. ���ڸ� ���� �����Ͻø� ū���� �ƴ����� ���� �� ���ϴ�.

            yield return new WaitForSeconds(1f); //ȸ�� �� 1�� ���

            yield return StartCoroutine(BlinkingMouseClick(TutorialCorgi_RightMouseClickedImage, 3)); //Ʃ�丮�� �ڱ� ���콺 �̹��� ��Ŭ�� 3��
            TutorialCorgi_Bone.transform.position = new Vector3(-3.082999f, -3.219f, 0f); //Ʃ�丮�� �ڱ� ���ٱ� �̵�

            TutorialCorgi_MouseImage.transform.position = new Vector3(TutorialCorgi_Bone.transform.position.x, TutorialCorgi_Bone.transform.position.y + 1.15f, 0f); //Ʃ�� ���콺 �̹��� �̵�
            TutorialCorgi_MouseSprite.sprite = TutorialCorgi_MouseUnClickedImage; //Ʃ���� ���콺 �̹��� ����

            yield return new WaitForSeconds(1f); //���ٱ͸� ������ �� 1�� ����

            yield return StartCoroutine(BlinkingMouseClick(TutorialCorgi_LeftMouseClickedImage, 3)); //Ʃ�丮�� �ڱ� ���콺 �̹��� ��Ŭ�� 3��
            TutorialCorgi_Bone_Rig2D.velocity = Vector2.right * 6f; //���ٱͿ� ���ӵ� �ο�

            TutorialCorgi_MouseSprite.sprite = TutorialCorgi_MouseUnClickedImage;
            TutorialCorgi_MouseImage.transform.position = new Vector3(6.016997f, -2.069f, 0f);

            yield return new WaitUntil(() => TutorialCorgi_Bone.transform.position.x >= 6f); //���ٱ��� ����� x��ǥ�� 80f�� ���� ������ ���

            TutorialCorgi_Bone_Rig2D.velocity = Vector2.zero;
            TutorialCorgi.transform.position = new Vector3(TutorialCorgi_Bone.transform.position.x - 0.983002f, TutorialCorgi.transform.position.y, TutorialCorgi.transform.position.z); //���ٱ��� x��ǥ�� 5f�� ������ �� ��ġ�� �̵�
            TutorialCorgi_MouseSprite.sprite = TutorialCorgi_LeftMouseClickedImage;

            yield return new WaitForSeconds(0.4f);

            TutorialCorgi_MouseSprite.sprite = TutorialCorgi_MouseUnClickedImage;

            yield return new WaitForSeconds(1f); //1�� ��� �� �ݺ�

        }
        yield return null;
    }

    private IEnumerator TutorialEnd()
    {
        float elapsedTime = 0f;
        float fadeDuration = 2f;

        while (elapsedTime < fadeDuration)
        {
            float currentAlphaBlackBox = Mathf.Lerp(0f, 255f, elapsedTime / fadeDuration); // fade in
            float currentAlphaPlayerCorgi = Mathf.Lerp(255f, 0f, elapsedTime / fadeDuration); // fade out

            // 0���� 255 ������ ������ ���� ����
            currentAlphaBlackBox = Mathf.Clamp(currentAlphaBlackBox, 0f, 255f);
            currentAlphaPlayerCorgi = Mathf.Clamp(currentAlphaPlayerCorgi, 0f, 255f);

            SpriteRenderer BlackBoxrenderer = BlackBox.GetComponent<SpriteRenderer>();
            SpriteRenderer[] PlayerCorgirenderers = PlayerCorgi.GetComponentsInChildren<SpriteRenderer>();

            // �� �迭 ��ġ��
            SpriteRenderer[] renderers = new SpriteRenderer[] { BlackBoxrenderer }.Concat(PlayerCorgirenderers).ToArray();

            foreach (SpriteRenderer renderer in renderers)
            {
                Color color = renderer.color;

                if (renderer == BlackBoxrenderer)
                {
                    float normalizedAlpha = currentAlphaBlackBox / 255.0f;
                    color.a = normalizedAlpha; // ���� �� ����
                }
                else
                {
                    float normalizedAlpha = currentAlphaPlayerCorgi / 255.0f;
                    color.a = normalizedAlpha; // ���� �� ����
                }

                renderer.color = color; // ����� ���� ����
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync("SceneMenu_01");
    }

    private IEnumerator RunOakPattern()
    {
        Vector3 OakObstcleWarningPosition = new Vector3(8.75f, -3.15f, 0f);
        NewOakObstacleWarning = Instantiate(OakObstacleWarning, OakObstcleWarningPosition, Quaternion.identity);
        NewOakObstacleWarning.SetActive(true);

        SpriteRenderer[] warningRenderers = NewOakObstacleWarning.GetComponentsInChildren<SpriteRenderer>();

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

        Destroy(NewOakObstacleWarning);

        Vector3 OakObstclePosition = new Vector3(10.5f, -3.46f, 0f);
        NewOakObstacle = Instantiate(OakObstacle, OakObstclePosition, Quaternion.identity);
        NewOakObstacle.SetActive(true);
    }

    private IEnumerator RunThorStemPattern()
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

    private IEnumerator BlinkingMouseClick(Sprite sprite, int times)
    {
        for (int i = 0; i < times; i++)
        {
            TutorialCorgi_MouseSprite.sprite = sprite;
            yield return new WaitForSeconds(0.4f);
            TutorialCorgi_MouseSprite.sprite = TutorialCorgi_MouseUnClickedImage;
            yield return new WaitForSeconds(0.4f);
        }
        yield return null;
    }
}
