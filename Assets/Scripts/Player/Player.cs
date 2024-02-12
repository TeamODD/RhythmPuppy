using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

using SceneData;
using EventManagement;

public class Player : MonoBehaviour
{
    enum PlayerAction
    {
        Dash,
        Jump,
        Shoot,
        ShootCancel,
    }

    [Header("Basic Status")]
    public int maxHP;
    public float maxStamina;
    [SerializeField]
    float staminaGen;
    [SerializeField]
    float speed;

    [Header("I-Frame (Invincible Frame, �ǰ� �����ð�)")]
    [Tooltip("Hit I-Frame(�ǰ� �� �����ð�)")]
    public float hitIFrame;
    [Tooltip("Revive I-Frame(��Ȱ �� �����ð�)")]
    public float reviveIFrame;

    [Header("Jump")]
    [SerializeField]
    float jumpForce;

    [Header("Dash")]
    [SerializeField]
    float dashForce;
    public float dashDuration;
    [SerializeField]
    float dashCooltime;
    [SerializeField]
    float dashStaminaCost;
    [SerializeField]
    float dashEvasionGen;

    [Header("Shoot")]
    [SerializeField]
    float shootStaminaCost;
    [SerializeField]
    float shootCancelStaminaGen;
    [Tooltip("����ü �߻� �� ����Ǵ� ��(Force)")]
    public float shootForce;
    [Tooltip("�߻� ���� �� ���� ���ð�")]
    public float shootCooltime;

    public float currentHP { get; private set; }
    public float currentStamina { get; private set; }
    public float deathCount { get; private set; }
    public bool S_Rank_True { get; private set; }

    public Transform projectile { get; private set; }
    public Transform head { get; private set; }
    public Transform neck { get; private set; }
    public Transform playerUICanvas { get; private set; }
    public Transform mark { get; private set; }

    Camera mainCamera;
    Head headScript;
    Projectile projectileScript;
    Collider2D hitbox;
    Rigidbody2D rig2D;
    SpriteRenderer[] spriteList;
    Animator anim;
    EventManager eventManager;
    Transform tutorials2Manager;
    Tutorials2Manager tutorials2ManagerScript;

    bool onFired, movable;
    Vector3 velocity, currentPosition;
    Coroutine dashCoroutine, dashCooldownCoroutine, shootCooldownCoroutine, invincibilityCoroutine;
    WaitForSeconds hitInvincibleDelay, reviveInvincibleDelay, dashDelay, dashCooldownDelay, shootCooldownDelay;

    void Awake()
    {
        /* Init attirutes */
        // objects
        projectileScript = transform.GetComponentInChildren<Projectile>();
        projectile = projectileScript.transform;
        headScript = transform.GetComponentInChildren<Head>();
        head = headScript.transform;
        neck = head.GetComponent<SpriteSkin>().rootBone.transform;
        playerUICanvas = transform.Find("PlayerUICanvas");
        mark = playerUICanvas.Find("Mark");
        // values
        if (Menu_PlayerTransform.difficulty_num == 0) maxHP = 6;
        currentHP = maxHP;
        currentStamina = maxStamina;
        deathCount = 0;
        S_Rank_True = true;

        /* Init fields */
        // objects
        mainCamera = Camera.main;
        rig2D = GetComponent<Rigidbody2D>();
        hitbox = transform.GetComponentInChildren<CapsuleCollider2D>();
        spriteList = transform.GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hitInvincibleDelay = new WaitForSeconds(hitIFrame);
        reviveInvincibleDelay = new WaitForSeconds(3);
        dashDelay = new WaitForSeconds(dashDuration);
        dashCooldownDelay = new WaitForSeconds(dashCooltime);
        shootCooldownDelay = new WaitForSeconds(shootCooltime);
        // values
        onFired = false;
        movable = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = shootCooldownCoroutine = null;

        /* Check whether this is Tutorial Scene or not - Ʃ�丮�� ������ �ƴ��� �˻� �� ���� �ʱ�ȭ*/
        checkTutorial();

        /** Init animator fields */
        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

    }

    void Start()
    {
        eventManager = GetComponentInParent<EventManager>();
        /** Add event functions to global event manager */
        eventManager.onDash.AddListener(dashEvent);
        eventManager.onShoot.AddListener(shootEvent);
        eventManager.onShootCancel.AddListener(shootCancelEvent);
        eventManager.onTeleport.AddListener(teleportEvent);
        eventManager.onAttacked.AddListener(playerHitEvent);
        eventManager.onDeath.AddListener(deathEvent);
        eventManager.onGameClear.AddListener(freeze);
        eventManager.onPause.AddListener(freeze);
        eventManager.onResume.AddListener(defreeze);
    }

    private void checkTutorial()
    {
        tutorials2ManagerScript = FindObjectOfType<Tutorials2Manager>();
        if (tutorials2ManagerScript != null)
        {
            /* Success : Tutorial script is initialized.
                ���� : Ʃ�丮�� ��ũ��Ʈ�� �������Ƿ�, Transform�� �ش� ��ũ��Ʈ�� transform���� �ʱ�ȭ��. */
            tutorials2Manager = tutorials2ManagerScript.transform;
        }
        else
        {
            /* Fail : Tutorial script is null and Transform will be initialized to null.
                ���� : Ʃ�丮�� ��ũ��Ʈ�� null�̹Ƿ� Transform�� null�� �ʱ�ȭ��. */
            tutorials2Manager = null;
        }
    }

    void Update()
    {
        if (!movable) return;
        checkJumpStatus();
        passiveStaminaGen();

        if (Input.GetButtonDown("Dash"))
        {
            //�ڷ���Ʈ Ʃ�丮�� �� �뽬 ��� �����ڵ�
            if (tutorials2Manager != null)
            {
                if (tutorials2ManagerScript.IsFinishedDashTest == true && tutorials2ManagerScript.IsFinishedTeleportTest == false || tutorials2ManagerScript.IsFinishedJumpTest == false)
                {
                    return;
                }
            }
            //�����ڵ�
            if (currentStamina < dashStaminaCost)
                Debug.Log("not enough stamina!");
            else if (!Input.GetAxisRaw("Horizontal").Equals(0) && dashCoroutine == null && dashCooldownCoroutine == null)
                eventManager.onDash.Invoke();

        }
        if (Input.GetButtonDown("Jump"))
        {
            jump();
        }
        if (Input.GetButtonDown("Shoot"))
        {
            //�ڷ���Ʈ ��� �����ڵ�
            if (tutorials2Manager != null)
            {
                if (tutorials2ManagerScript.IsFinishedDashTest == false)
                {
                    return;
                }
            }
            if (!onFired)
            {
                if (currentStamina < shootStaminaCost)
                    Debug.Log("not enough stamina!");
                else if (shootCooldownCoroutine == null)
                {
                    //Projectile projectile = GameObject.Find("Projectile").GetComponent<Projectile>();
                    //if (!projectile.IsBoneRecovered)
                    if (!projectileScript.IsBoneRecovered)
                    {
                        return;
                    }
                    eventManager.onShoot.Invoke();
                }
            }
            else
            {
                eventManager.onTeleport.Invoke();
            }
        }
        if (Input.GetButtonDown("ShootCancel"))
        {
            if (onFired)
                eventManager.onShootCancel.Invoke();
        }
        if (Input.GetKey(KeyCode.F1) && Input.GetKeyDown(KeyCode.F12))
        {
            if (transform.Find("��/Hitbox").GetComponent<CapsuleCollider2D>().enabled)
            {
                transform.Find("��/Hitbox").GetComponent<CapsuleCollider2D>().enabled = false;
                Debug.Log("�����ڸ�� Ȱ��ȭ");
            }
            else
            {
                transform.Find("��/Hitbox").GetComponent<CapsuleCollider2D>().enabled = true;
                Debug.Log("�����ڸ�� ��Ȱ��ȭ");
            }
        }
    }

    void FixedUpdate()
    {
        if (!movable) return;
        velocity = rig2D.velocity;
        currentPosition = transform.position;
        flipBody();
        move();
    }

    void LateUpdate()
    {
        if (!movable) return;
        fixPositionIntoScreen();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            rig2D.velocity = velocity;
            transform.position = currentPosition;
            Collider2D d = c.collider;
            if (!isCollisionVisibleOnTheScreen(c))
            {
                return;
            }
            if (dashCoroutine != null) evade(c.collider);
            else if (invincibilityCoroutine == null) eventManager.onAttacked.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            if (SceneManager.GetActiveScene().name == "Tutorials2")
            {
                if (dashCoroutine != null)
                {
                    evade(c);
                    return;
                }
                transform.position = new Vector3(-7f, -4.3012f, 0f);
                return;
            }
            if (dashCoroutine != null) evade(c);
            else if (invincibilityCoroutine == null) eventManager.onAttacked.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            if (dashCoroutine != null) evade(c);
            else if (invincibilityCoroutine == null) eventManager.onAttacked.Invoke();
        }
    }

    private void passiveStaminaGen()
    {
        float s = staminaGen * Time.deltaTime;
        currentStamina = maxStamina < currentStamina + s ? maxStamina : currentStamina + s;
    }

    private void move()
    {
        float xInput = Input.GetAxis("Horizontal");
        if (xInput.Equals(0))
        {
            anim.SetBool("bAxisInput", false);
        }
        else
        {
            anim.SetBool("bAxisInput", true);
            transform.Translate(new Vector3(xInput * speed * Time.fixedDeltaTime, 0, 0));
        }
    }

    private void jump()
    {
        int count = anim.GetInteger("JumpCount");
        switch (count)
        {
            case 0:
            case 1:
                anim.SetTrigger("Jump");
                anim.SetInteger("JumpCount", count + 1);
                rig2D.velocity = new Vector2(rig2D.velocity.x, jumpForce);
                break;
            default:
                break;
        }
    }

    private bool isJump()
    {
        const float margin = 0.05f; //�����ߴٰ� �Ǵ��ϴ� �ּ� �ӵ�

        if (Mathf.Abs(rig2D.velocity.y) < margin)
        {
            return false;
        }
        return true;
    }

    private void dashEvent()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) return;
        currentStamina -= dashStaminaCost;
        dashCoroutine = StartCoroutine(dash(dir));
    }

    private IEnumerator dash(float dir)
    {
        setAlpha(0.5f);
        rig2D.velocity = new Vector2(dir * dashForce, rig2D.velocity.y);
        rig2D.gravityScale = 1f;
        anim.SetTrigger("Dash");
        /* ������ ���������� ������ �� dashDelay��ŭ ���ӵ�  */
        yield return dashDelay;
        setAlpha(1f);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);
        rig2D.gravityScale = 1f;
        dashCooldownCoroutine = StartCoroutine(dashCooldown());
        dashCoroutine = null;
    }

    private void evade(Collider2D c)
    {
        Debug.Log(string.Format("[{0}] {1}", Time.time, "ȸ��!"));
        currentStamina = maxStamina < currentStamina + dashEvasionGen ? maxStamina : currentStamina + dashEvasionGen;
        StartCoroutine(delayCollision(c, dashDelay));
    }

    private IEnumerator dashCooldown()
    {
        yield return dashCooldownDelay;
        anim.ResetTrigger("Dash");
        dashCooldownCoroutine = null;
    }

    private void shootEvent()
    {
        currentStamina -= shootStaminaCost;
        onFired = true;
        shootCooldownCoroutine = StartCoroutine(runShootCooldown());
    }

    private IEnumerator runShootCooldown()
    {
        yield return shootCooldownDelay;
        shootCooldownCoroutine = null;
    }

    private void teleportEvent()
    {
        int count = anim.GetInteger("JumpCount");
        Vector3 pos = projectile.position;
        transform.position = pos;
        if (2 <= count)
            anim.SetInteger("JumpCount", count - 1);
        onFired = false;
    }

    private void shootCancelEvent()
    {
        currentStamina = maxStamina < currentStamina + shootCancelStaminaGen ? maxStamina : currentStamina + shootCancelStaminaGen;
        projectile.position = neck.transform.position;
        onFired = false;
        shootCooldownCoroutine = StartCoroutine(runShootCooldown());
    }

    private void checkJumpStatus()
    {
        if (isJump())
        {
            if (anim.GetInteger("JumpCount").Equals(0))
            {
                anim.SetInteger("JumpCount", 1);
                anim.SetTrigger("Jump");
            }
        }
        else
        {
            anim.SetInteger("JumpCount", 0);
            anim.ResetTrigger("Jump");
        }
    }

    private void fixPositionIntoScreen()
    {
        Vector3 pos = mainCamera.WorldToViewportPoint(transform.position);
        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;
        transform.position = mainCamera.ViewportToWorldPoint(pos);
    }


    /** Flip body if player is heading behind or moving to behind. */
    private void flipBody()
    {
        Vector3 flip, mousePos;
        float mouseAngle, frontSide, backSide;
        flip = transform.localScale;    // Player �¿� ���� ����
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition); // ���콺 ��ǥ
        mouseAngle = headScript.getHeadingAngle(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        //�÷��̾� �߽�(��Ȯ�� �߽��� �ƴմϴ�)�� �������� ���̾Ƹ�� ���� ���� ���콺�� ������ �ø����� �ʵ��� ����
        if (IsMouseInDiamond(mousePos)) return;

        /** [ Priority list for flip player - �ø�(�¿����)�� ���� �켱���� ����Ʈ ]
            1. Mouse Position (���콺 ��ġ)
                : ���� ���콺�� �ݴ���(����)�� �ٶ󺸴� ���, ������ �¿�����Ѵ�.
            2. Arrow Key Input(Left/Right) with Mouse Position (�¿�Ű �Է� + ���콺 ��ġ)
                : ���콺�� �ݴ����� �ٶ󺸴� ��쿣, Ű���� �Է¿� ���� �ø��Ѵ�.       */
        frontSide = flip.x < 0 ? headScript.frontAngle + 180f : headScript.frontAngle;
        backSide = frontSide + 180f;
        /** ���� ���콺 ��ġ�� �÷��̾��� ����(�ݴ���)�̶�� : flip ���� */
        if (headScript.isBetweenAngles(mouseAngle, backSide - headScript.rotationLimit, backSide + headScript.rotationLimit))
        {
            flip.x = flip.x * -1;
        }
        /** ���� ���콺 ��ġ�� ���� ���̴� ������ ��ġ��� : Ű���� �¿� �Է´�� flip ���� */
        else if (!headScript.isBetweenAngles(mouseAngle, frontSide - headScript.rotationLimit, frontSide + headScript.rotationLimit))
        {
            int xInput = (int)Input.GetAxisRaw("Horizontal");
            /** It works(flips) only there is keyboard input - ���� Ű���� �Է��� �������� �۵���.  */
            if (xInput != 0)
            {
                flip.x = 0 < xInput ? Mathf.Abs(flip.x) : -Mathf.Abs(flip.x);
            }
        }
        transform.localScale = flip;
    }

    bool IsMouseInDiamond(Vector2 mousePosition)
    {
        Vector2 v1 = (Vector2)transform.position + new Vector2(-1f, 0f), // ����
                v2 = (Vector2)transform.position + new Vector2(0f, 0f), // �Ʒ�
                v3 = (Vector2)transform.position + new Vector2(1f, 0f),  // ������
                v4 = (Vector2)transform.position + new Vector2(0f, 3f);  // ��
        // ���̾Ƹ�� ��� ���� ���� �ִ��� Ȯ��
        if (IsPointInTriangle(mousePosition, v1, v2, v3) || IsPointInTriangle(mousePosition, v1, v3, v4))
        {
            return true;
        }
        return false;
    }

    // �ﰢ���� ���ο� ���� �ִ��� Ȯ���ϴ� �Լ�
    bool IsPointInTriangle(Vector2 point, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));
        float a = ((p2.y - p3.y) * (point.x - p3.x) + (p3.x - p2.x) * (point.y - p3.y)) / denominator;
        float b = ((p3.y - p1.y) * (point.x - p3.x) + (p1.x - p3.x) * (point.y - p3.y)) / denominator;
        float c = 1 - a - b;

        return a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1;
    }

    private void playerHitEvent()
    {
        currentHP--;
        S_Rank_True = false;
        if (currentHP <= 0)
        {
            deathCount++;
            eventManager.onDeath.Invoke();
            return;
        }
        invincibilityCoroutine = StartCoroutine(activateInvincibility());
    }

    private void deathEvent()
    {
        StopAllCoroutines();
        StartCoroutine(deathAction());
    }

    private IEnumerator deathAction()
    {
        movable = false;
        hitbox.enabled = false;
        setAlpha(1);
        anim.SetTrigger("Death");

        yield return new WaitForSeconds(5f);

        if (deathCount <= 2)
        {
            eventManager.onRewind.Invoke();
            eventManager.onRevive.Invoke();
        }
        else
        {
            Debug.Log("load gameover scene");

            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            PlayerPrefs.SetString("PlayingSceneName", sceneName);

            SceneManager.LoadScene(SceneInfo.getSceneName(SceneName.GAMEOVER), LoadSceneMode.Single);
        }
    }

    public void reviveEvent()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        movable = true;
        hitbox.enabled = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = null;
        setAlpha(1);
        anim.ResetTrigger("Death");
        anim.SetTrigger("Revive");

        if (GameObject.FindGameObjectWithTag("Boss") != null) //Boss�±׸� �� ������Ʈ�� �ִ� ������������ �׾���,
        {
            BoxCollider2D bosscollider2D = GameObject.FindGameObjectWithTag("Boss").transform.GetChild(0).GetComponent<BoxCollider2D>();
            Vector2 bosscollider2D_size = bosscollider2D.size;
            if (-bosscollider2D_size.x <= transform.position.x) //������ ���ʿ� �׾�����
            {
                transform.position -= new Vector3(bosscollider2D_size.x - Mathf.Abs(transform.position.x), 0f, 0f);
            }
            else if (transform.position.x < bosscollider2D_size.x) //������ �����ʿ� �׾�����
            {
                transform.position += new Vector3(bosscollider2D_size.x - Mathf.Abs(transform.position.x), 0f, 0f);
            }
        }
        else
        {
            /* �⺻������ ��Ȱ �� x��ǥ�� 0���� ���� */
            Vector3 v = transform.position;
            v.x = 0;
            transform.position = v;
        }

        StartCoroutine(reviveCoroutine());
    }

    private IEnumerator reviveCoroutine()
    {
        /* revive �̺�Ʈ 1�ʵڿ� ������ ���۵�, �� 1�ʵ��� ���� ǥ�õ� */
        yield return new WaitForSeconds(2);
        anim.ResetTrigger("Revive");
        /* ��Ȱ ���� �ߵ� - 'reviveInvincibleDelay'��ŭ ���� �ߵ� */
        hitbox.enabled = false;
        setAlpha(0.5f);
        yield return reviveInvincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);
    }

    private IEnumerator activateInvincibility()
    {
        hitbox.enabled = false;
        setAlpha(0.5f);     // Player�� ������ ���·� ����
        /* �ǰ� ���� �ߵ� - 'hitInvincibleDelay'��ŭ ���� �ߵ� */
        yield return hitInvincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);     // Player�� ����(������) ���·� �ǵ���
        invincibilityCoroutine = null;
    }

    private void setAlpha(float a)
    {
        const int exceptSortingIndex = 100;
        Color c;

        for (int i = 0; i < spriteList.Length; i++)
        {
            if (spriteList[i].sortingOrder.Equals(exceptSortingIndex)) continue;
            c = spriteList[i].color;
            c.a = a;
            spriteList[i].color = c;
        }
    }

    private IEnumerator delayCollision(Collider2D c, WaitForSeconds delay)
    {
        Physics2D.IgnoreCollision(hitbox, c, true);
        yield return delay;
        if (c == null) yield break;
        Physics2D.IgnoreCollision(hitbox, c, false);
    }

    private bool isCollisionVisibleOnTheScreen(Collision2D c)
    {
        Vector2 point = c.GetContact(0).point;
        LayerMask layerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Obstacle");
        RaycastHit2D[] hit = Physics2D.RaycastAll(point, Vector2.zero, 0, layerMask);
        SpriteRenderer sp = null, tmp = null;

        for (int i = 0; i < hit.Length; i++)
        {
            tmp = null;
            if (hit[i].transform.TryGetComponent(out tmp))
            {
                if (sp == null || getBiggerSortingOrder(ref tmp, ref sp).Equals(tmp))
                    sp = tmp;
            }
        }
        if (c != null && sp != null && c.transform.Equals(sp.transform))
            return true;
        return false;
    }

    private ref SpriteRenderer getBiggerSortingOrder(ref SpriteRenderer s1, ref SpriteRenderer s2)
    {
        int s1Layer = s1.gameObject.layer, s2Layer = s2.gameObject.layer, s1SortingOrder = s1.sortingOrder, s2SortingOrder = s2.sortingOrder;
        if (LayerMask.LayerToName(s1Layer).Equals("Ground"))
            return ref s1;
        else if (LayerMask.LayerToName(s2Layer).Equals("Ground"))
            return ref s2;

        if (s1SortingOrder < s2SortingOrder)
            return ref s2;
        else if (s1SortingOrder > s2SortingOrder)
            return ref s1;

        return ref s1;
    }

    private void freeze()
    {
        movable = false;
    }

    private void defreeze()
    {
        movable = true;
    }
}
