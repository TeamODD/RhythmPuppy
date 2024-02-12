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

    [Header("I-Frame (Invincible Frame, 피격 무적시간)")]
    [Tooltip("Hit I-Frame(피격 시 무적시간)")]
    public float hitIFrame;
    [Tooltip("Revive I-Frame(부활 시 무적시간)")]
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
    [Tooltip("투사체 발사 시 적용되는 힘(Force)")]
    public float shootForce;
    [Tooltip("발사 종료 후 재사용 대기시간")]
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

        /* Check whether this is Tutorial Scene or not - 튜토리얼 씬인지 아닌지 검사 후 변수 초기화*/
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
                성공 : 튜토리얼 스크립트를 얻어왔으므로, Transform도 해당 스크립트의 transform으로 초기화함. */
            tutorials2Manager = tutorials2ManagerScript.transform;
        }
        else
        {
            /* Fail : Tutorial script is null and Transform will be initialized to null.
                실패 : 튜토리얼 스크립트가 null이므로 Transform도 null로 초기화함. */
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
            //텔레포트 튜토리얼 중 대쉬 사용 방지코드
            if (tutorials2Manager != null)
            {
                if (tutorials2ManagerScript.IsFinishedDashTest == true && tutorials2ManagerScript.IsFinishedTeleportTest == false || tutorials2ManagerScript.IsFinishedJumpTest == false)
                {
                    return;
                }
            }
            //정상코드
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
            //텔레포트 사용 방지코드
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
            if (transform.Find("몸/Hitbox").GetComponent<CapsuleCollider2D>().enabled)
            {
                transform.Find("몸/Hitbox").GetComponent<CapsuleCollider2D>().enabled = false;
                Debug.Log("개발자모드 활성화");
            }
            else
            {
                transform.Find("몸/Hitbox").GetComponent<CapsuleCollider2D>().enabled = true;
                Debug.Log("개발자모드 비활성화");
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
        const float margin = 0.05f; //점프했다고 판단하는 최소 속도

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
        /* 투명도와 물리엔진을 설정한 뒤 dashDelay만큼 지속됨  */
        yield return dashDelay;
        setAlpha(1f);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);
        rig2D.gravityScale = 1f;
        dashCooldownCoroutine = StartCoroutine(dashCooldown());
        dashCoroutine = null;
    }

    private void evade(Collider2D c)
    {
        Debug.Log(string.Format("[{0}] {1}", Time.time, "회피!"));
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
        flip = transform.localScale;    // Player 좌우 반전 여부
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition); // 마우스 좌표
        mouseAngle = headScript.getHeadingAngle(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        //플레이어 중심(정확한 중심은 아닙니다)을 기준으로 다이아몬드 범위 내에 마우스가 들어오면 플립하지 않도록 수정
        if (IsMouseInDiamond(mousePos)) return;

        /** [ Priority list for flip player - 플립(좌우반전)에 대한 우선순위 리스트 ]
            1. Mouse Position (마우스 위치)
                : 만약 마우스가 반대쪽(뒤쪽)을 바라보는 경우, 무조건 좌우반전한다.
            2. Arrow Key Input(Left/Right) with Mouse Position (좌우키 입력 + 마우스 위치)
                : 마우스가 반대쪽을 바라보는 경우엔, 키보드 입력에 따라서 플립한다.       */
        frontSide = flip.x < 0 ? headScript.frontAngle + 180f : headScript.frontAngle;
        backSide = frontSide + 180f;
        /** 현재 마우스 위치가 플레이어의 뒤쪽(반대쪽)이라면 : flip 실행 */
        if (headScript.isBetweenAngles(mouseAngle, backSide - headScript.rotationLimit, backSide + headScript.rotationLimit))
        {
            flip.x = flip.x * -1;
        }
        /** 현재 마우스 위치가 목이 꺾이는 각도의 위치라면 : 키보드 좌우 입력대로 flip 실행 */
        else if (!headScript.isBetweenAngles(mouseAngle, frontSide - headScript.rotationLimit, frontSide + headScript.rotationLimit))
        {
            int xInput = (int)Input.GetAxisRaw("Horizontal");
            /** It works(flips) only there is keyboard input - 오직 키보드 입력이 있을때만 작동함.  */
            if (xInput != 0)
            {
                flip.x = 0 < xInput ? Mathf.Abs(flip.x) : -Mathf.Abs(flip.x);
            }
        }
        transform.localScale = flip;
    }

    bool IsMouseInDiamond(Vector2 mousePosition)
    {
        Vector2 v1 = (Vector2)transform.position + new Vector2(-1f, 0f), // 왼쪽
                v2 = (Vector2)transform.position + new Vector2(0f, 0f), // 아래
                v3 = (Vector2)transform.position + new Vector2(1f, 0f),  // 오른쪽
                v4 = (Vector2)transform.position + new Vector2(0f, 3f);  // 위
        // 다이아몬드 모양 범위 내에 있는지 확인
        if (IsPointInTriangle(mousePosition, v1, v2, v3) || IsPointInTriangle(mousePosition, v1, v3, v4))
        {
            return true;
        }
        return false;
    }

    // 삼각형의 내부에 점이 있는지 확인하는 함수
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

        if (GameObject.FindGameObjectWithTag("Boss") != null) //Boss태그를 단 오브젝트가 있는 스테이지에서 죽었고,
        {
            BoxCollider2D bosscollider2D = GameObject.FindGameObjectWithTag("Boss").transform.GetChild(0).GetComponent<BoxCollider2D>();
            Vector2 bosscollider2D_size = bosscollider2D.size;
            if (-bosscollider2D_size.x <= transform.position.x) //보스의 왼쪽에 죽었으면
            {
                transform.position -= new Vector3(bosscollider2D_size.x - Mathf.Abs(transform.position.x), 0f, 0f);
            }
            else if (transform.position.x < bosscollider2D_size.x) //보스의 오른쪽에 죽었으면
            {
                transform.position += new Vector3(bosscollider2D_size.x - Mathf.Abs(transform.position.x), 0f, 0f);
            }
        }
        else
        {
            /* 기본적으로 부활 시 x좌표를 0으로 변경 */
            Vector3 v = transform.position;
            v.x = 0;
            transform.position = v;
        }

        StartCoroutine(reviveCoroutine());
    }

    private IEnumerator reviveCoroutine()
    {
        /* revive 이벤트 1초뒤에 음악이 시작됨, 또 1초동안 경고등 표시됨 */
        yield return new WaitForSeconds(2);
        anim.ResetTrigger("Revive");
        /* 부활 무적 발동 - 'reviveInvincibleDelay'만큼 무적 발동 */
        hitbox.enabled = false;
        setAlpha(0.5f);
        yield return reviveInvincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);
    }

    private IEnumerator activateInvincibility()
    {
        hitbox.enabled = false;
        setAlpha(0.5f);     // Player를 반투명 상태로 설정
        /* 피격 무적 발동 - 'hitInvincibleDelay'만큼 무적 발동 */
        yield return hitInvincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);     // Player를 정상(불투명) 상태로 되돌림
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
