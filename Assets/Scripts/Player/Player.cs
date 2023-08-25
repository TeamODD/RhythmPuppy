using Autodesk.Fbx;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using static EventManager.PlayerEvent;
using static UnityEngine.GraphicsBuffer;

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


    bool onFired, isAlive;
    [HideInInspector] public float currentHP, currentStamina;
    float headCorrectFactor, deathCount;
    Vector3 velocity;
    Coroutine dashCoroutine, dashCooldownCoroutine, shootCooldownCoroutine, invincibilityCoroutine;

    WaitForSeconds invincibleDelay, dashDelay, dashCooldownDelay, shootCooldownDelay;

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
        dashDelay = new WaitForSeconds(dashDuration);
        dashCooldownDelay = new WaitForSeconds(dashCooltime);

        onFired = false;
        isAlive = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = shootCooldownCoroutine = null;
        headCorrectFactor = neck.transform.rotation.eulerAngles.z + head.transform.rotation.eulerAngles.z;
        deathCount = 0;
        currentHP = maxHP;
        currentStamina = maxStamina;

        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

        eventManager.playerHitEvent += playerHitEvent;
        eventManager.deathEvent += deathEvent;
        eventManager.reviveEvent += reviveEvent;
        eventManager.playerEvent.dashEvent += dashEvent;
        eventManager.playerEvent.shootEvent += shootEvent;
        eventManager.playerEvent.teleportEvent += teleportEvent;
        eventManager.playerEvent.shootCancelEvent += shootCancelEvent;
    }

    void Update()
    {
        if (!isAlive) return;
        checkJumpStatus();
        passiveStaminaGen();

        if (Input.GetButtonDown("Dash"))
        {
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
            if (!onFired)
            {
                if (currentStamina < shootStaminaCost)
                    Debug.Log("not enough stamina!");
                else if (shootCooldownCoroutine == null)
                    eventManager.playerEvent.shootEvent();
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

        if (Input.GetKeyDown(KeyCode.U))    // direct hit on player
        {
            if (invincibilityCoroutine == null) eventManager.playerHitEvent();
        }
        if (Input.GetKeyDown(KeyCode.I))     // developer mode (inactive hitbox)
        {
            hitbox.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
        velocity = rig2D.velocity;
        flipBody();
        move();
    }

    void LateUpdate()
    {
        if (!isAlive) return;
        fixPositionIntoScreen();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            rig2D.velocity = velocity;
            if (!isCollisionVisibleOnTheScreen(c)) return;

            if (dashCoroutine != null) evade(c.collider);
            else if (invincibilityCoroutine == null) eventManager.playerHitEvent();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            if (dashCoroutine != null) evade(c);
            else if (invincibilityCoroutine == null) eventManager.playerHitEvent();
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
        const float margin = 0.05f;

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
        rig2D.gravityScale = 0f;
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
        const float detailCorrFactor = 16f;
        Vector3 flip = transform.localScale;

        float rot = neck.transform.localRotation.eulerAngles.z - headCorrectFactor + detailCorrFactor;
        rot = 0 <= rot ? rot % 360 : rot % 360 + 360;
        if (110 < rot && rot < 250)
        {
            flip.x = flip.x * -1;
        }
        else if (70 < rot && rot < 290)
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

    private void playerHitEvent()
    {
        currentHP--;
        if (currentHP < 0)
        {
            deathCount++;
            eventManager.deathEvent();
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
        isAlive = false;
        hitbox.enabled = false;
        setAlpha(1);
        anim.SetTrigger("Death");

        yield return new WaitForSeconds(5f);

        if (deathCount < 3)
        {
            eventManager.rewindEvent();
            eventManager.reviveEvent();
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void reviveEvent()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        isAlive = true;
        hitbox.enabled = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = null;
        setAlpha(1);
        anim.ResetTrigger("Death");
        anim.SetTrigger("Revive");
        StartCoroutine(reviveEventCoroutine());
    }

    private IEnumerator reviveEventCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.ResetTrigger("Revive");
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
        LayerMask layerMask = -1;
        layerMask &= ~LayerMask.GetMask("Player");
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
        
        if (s1Layer < s2Layer)
            return ref s2;
        else if (s1Layer > s2Layer)
            return ref s1;

        if (s1SortingOrder < s2SortingOrder)
            return ref s2;
        else if (s1SortingOrder > s2SortingOrder)
            return ref s1;

        return ref s1;
    }
}
