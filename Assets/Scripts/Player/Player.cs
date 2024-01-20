using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

using SceneData;
using EventManagement;
using System.Net.Sockets;

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
    public float invincibleDuration;

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

    Camera mainCamera;
    GameObject projectile, head, neck;
    Collider2D hitbox;
    Rigidbody2D rig2D;
    SpriteRenderer[] spriteList;
    Animator anim;
    EventManager eventManager;
    Tutorials2Manager tutorials2Manager;


    bool onFired, movable;
    [HideInInspector] public float currentHP, currentStamina;
    float headCorrectFactor;
    [HideInInspector] public float deathCount;
    [HideInInspector] public bool S_Rank_True;
    Vector3 velocity, currentPosition;
    Coroutine dashCoroutine, dashCooldownCoroutine, shootCooldownCoroutine, invincibilityCoroutine;

    WaitForSeconds invincibleDelay, reviveInvincibleDelay, dashDelay, dashCooldownDelay, shootCooldownDelay;

    void Awake()
    {
        init();
    }

    public void init()
    {
        mainCamera = Camera.main;
        projectile = transform.Find("Projectile").gameObject;
        head = transform.Find("Head").gameObject;
        neck = head.GetComponent<SpriteSkin>().rootBone.gameObject;

        rig2D = GetComponent<Rigidbody2D>();
        hitbox = transform.GetComponentInChildren<CapsuleCollider2D>();
        spriteList = transform.GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        eventManager = FindObjectOfType<EventManager>();

        invincibleDelay = new WaitForSeconds(invincibleDuration);
        reviveInvincibleDelay = new WaitForSeconds(3);
        dashDelay = new WaitForSeconds(dashDuration);
        dashCooldownDelay = new WaitForSeconds(dashCooltime);
        shootCooldownDelay = new WaitForSeconds(shootCooltime);

        onFired = false;
        movable = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = shootCooldownCoroutine = null;
        headCorrectFactor = neck.transform.rotation.eulerAngles.z + head.transform.rotation.eulerAngles.z;
        deathCount = 0;
        S_Rank_True = true;
        if (Menu_PlayerTransform.difficulty_num == 0)
            maxHP = 6;
        currentHP = maxHP;   
        currentStamina = maxStamina;

        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.stageEvent.clearEvent += freeze;
        eventManager.stageEvent.pauseEvent += freeze;
        eventManager.stageEvent.resumeEvent += defreeze;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += reviveEvent;
        eventManager.playerEvent.dashEvent += dashEvent;
        eventManager.playerEvent.shootEvent += shootEvent;
        eventManager.playerEvent.teleportEvent += teleportEvent;
        eventManager.playerEvent.shootCancelEvent += shootCancelEvent;
    }

    void Update()
    {
        if (!movable) return;
        checkJumpStatus();
        passiveStaminaGen();

        if (Input.GetButtonDown("Dash"))
        {
            //텔레포트 튜토리얼 중 대쉬 사용 방지코드
            if (GameObject.Find("Tutorials2Manager") != null)
            {
                tutorials2Manager = GameObject.Find("Tutorials2Manager").GetComponent<Tutorials2Manager>();
                if (tutorials2Manager.IsFinishedDashTest == true && tutorials2Manager.IsFinishedTeleportTest == false || tutorials2Manager.IsFinishedJumpTest == false)
                {
                    return;
                }
            }
                
            //정상코드
            if (currentStamina < dashStaminaCost)
                Debug.Log("not enough stamina!");
            else if (!Input.GetAxisRaw("Horizontal").Equals(0) && dashCoroutine == null && dashCooldownCoroutine == null)
                eventManager.playerEvent.dashEvent();

        }
        if (Input.GetButtonDown("Jump"))
        {
            jump();
        }
        if (Input.GetButtonDown("Shoot"))
        {
            //텔레포트 사용 방지코드
            if (GameObject.Find("Tutorials2Manager") != null)
            {
                tutorials2Manager = GameObject.Find("Tutorials2Manager").GetComponent<Tutorials2Manager>();
                if (tutorials2Manager.IsFinishedDashTest == false)
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
                    Projectile projectile = GameObject.Find("Projectile").GetComponent<Projectile>();
                    if (!projectile.IsBoneRecovered)
                    {
                        return;
                    }
                    eventManager.playerEvent.shootEvent();
                }
                    
            }
            else
            {
                eventManager.playerEvent.teleportEvent();
            }
        }
        if (Input.GetButtonDown("ShootCancel"))
        {
            if (onFired)
                eventManager.playerEvent.shootCancelEvent();
        }

        if (Input.GetKey(KeyCode.F1)&&Input.GetKeyDown(KeyCode.F12))
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
            else if (invincibilityCoroutine == null) eventManager.playerEvent.playerHitEvent();
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
            else if (invincibilityCoroutine == null) eventManager.playerEvent.playerHitEvent();
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            if (dashCoroutine != null) evade(c);
            else if (invincibilityCoroutine == null) eventManager.playerEvent.playerHitEvent();
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
        Vector3 pos = projectile.transform.position;
        transform.position = pos;
        if (2 <= count)
            anim.SetInteger("JumpCount", count - 1);
        onFired = false;
    }

    private void shootCancelEvent()
    {
        currentStamina = maxStamina < currentStamina + shootCancelStaminaGen ? maxStamina : currentStamina + shootCancelStaminaGen;
        projectile.transform.position = neck.transform.position;
        onFired = false;
        shootCooldownCoroutine = StartCoroutine(runShootCooldown());
    }

    private void checkJumpStatus()
    {
        if(isJump())
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

    /** Flip body if corgi is heading behind or moving to behind. */

    private void flipBody()
    {
        Vector2 vertex1 = (Vector2)transform.position + new Vector2(-1f, 0f); // 왼쪽
        Vector2 vertex2 = (Vector2)transform.position + new Vector2(0f, 0f); // 아래
        Vector2 vertex3 = (Vector2)transform.position + new Vector2(1f, 0f);  // 오른쪽
        Vector2 vertex4 = (Vector2)transform.position + new Vector2(0f, 3f);  // 위

        //플레이어 중심(정확한 중심은 아닙니다)을 기준으로 다이아몬드 범위 내에 마우스가 들어오면 플립하지 않도록 수정
        if (!IsMouseInDiamond(mainCamera.ScreenToWorldPoint(Input.mousePosition), vertex1, vertex2, vertex3, vertex4))
        {
            // 기존 코드
            const float detailCorrFactor = 16f;
            Vector3 flip = transform.localScale;

            float rot = neck.transform.localRotation.eulerAngles.z - headCorrectFactor + detailCorrFactor;
            rot = 0 <= rot ? rot % 360 : rot % 360 + 360;
            if (110 < rot && rot < 250)
            {
                flip.x = flip.x * -1;
            }
            else if (80 < rot && rot < 280)
            {
                int xInput = (int)Input.GetAxisRaw("Horizontal");
                switch (xInput)
                {
                    case -1:
                        flip.x = Mathf.Abs(flip.x) * -1;
                        break;
                    case 1:
                        flip.x = Mathf.Abs(flip.x);
                        break;
                }
            }
            transform.localScale = flip;
        }
    }

    bool IsMouseInDiamond(Vector2 mousePosition, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
    {
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
            eventManager.playerEvent.deathEvent();
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
            eventManager.stageEvent.rewindEvent();
            eventManager.playerEvent.reviveEvent();
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

    private void reviveEvent()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        movable = true;
        hitbox.enabled = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = null;
        setAlpha(1);
        anim.ResetTrigger("Death");
        anim.SetTrigger("Revive");

        if (GameObject.FindGameObjectWithTag("Boss")!=null) //Boss태그를 단 오브젝트가 있는 스테이지에서 죽었고,
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

        /* 부활 무적 발동 - 'reviveInvincibleDelay' 시간만큼 무적 발동 */
        hitbox.enabled = false;
        setAlpha(0.5f);
        yield return reviveInvincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);
    }

    private IEnumerator activateInvincibility()
    {
        hitbox.enabled = false;
        setAlpha(0.5f);
        yield return invincibleDelay;
        hitbox.enabled = true;
        setAlpha(1f);
        invincibilityCoroutine = null; 
    }

    private void setAlpha(float a)
    {
        const int exceptSortingIndex = 100;
        Color c;

        for(int i=0; i< spriteList.Length; i++)
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
