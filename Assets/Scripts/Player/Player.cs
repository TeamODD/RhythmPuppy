using Autodesk.Fbx;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
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

    AudioSource audioSource;
    GameObject uiCanvas, projectile, mark, head, neck;
    Collider2D hitbox;
    Rigidbody2D rig2D;
    SpriteRenderer[] spriteList;
    Animator anim;
    EventManager eventManager;


    bool onFired, isAlive;
    [HideInInspector] public float currentHP, currentStamina;
    float headCorrectFactor, deathCount;
    Vector3 velocity;
    Coroutine dashCoroutine, dashCooldownCoroutine, invincibilityCoroutine;

    WaitForSeconds invincibleDelay, dashDelay;

    void Awake()
    {
        init();
    }

    public void init()
    {
        audioSource = FindObjectOfType<AudioSource>();
        uiCanvas = GameObject.Find("UICanvas");
        projectile = transform.Find("Projectile").gameObject;
        mark = transform.Find("Mark").gameObject;
        head = transform.Find("Head").gameObject;
        neck = head.GetComponent<SpriteSkin>().rootBone.gameObject;

        rig2D = GetComponent<Rigidbody2D>();
        hitbox = transform.GetComponentInChildren<CapsuleCollider2D>();
        spriteList = transform.GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        eventManager = FindObjectOfType<EventManager>();

        invincibleDelay = new WaitForSeconds(invincibleDuration);
        dashDelay = new WaitForSeconds(dashDuration);

        onFired = false;
        isAlive = true;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = null;
        headCorrectFactor = neck.transform.rotation.eulerAngles.z + head.transform.rotation.eulerAngles.z;
        deathCount = 0;
        currentHP = maxHP;
        currentStamina = maxStamina;

        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

        eventManager.playerHitEvent += playerHitEvent;
        eventManager.deathEvent += deathEvent;
        eventManager.reviveEvent += reviveEvent;
    }

    void Update()
    {
        if (!isAlive) return;
        checkJumpStatus();
        passiveStaminaGen();

        if (Input.GetButtonDown("Dash"))
        {
            if (dashCoroutine == null && dashCooldownCoroutine == null)
            {
                dashCoroutine = StartCoroutine(dash());
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            jump();
        }
        if (Input.GetButtonDown("Shoot"))
        {
            /*eventManager.playerHitEvent();*/
            if (!onFired) shoot();
            else teleport();
        }
        if (Input.GetButtonDown("ShootCancel"))
        {
            if (onFired)
            {
                shootCancel();
            }
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

    public void activateMark()
    {
        mark.SendMessage("activate");
    }

    public void inactivateMark()
    {
        mark.SendMessage("inactivate");
    }

    private void passiveStaminaGen()
    {
        float s = staminaGen * Time.deltaTime;
        currentStamina = 100 < currentStamina + s ? 100 : currentStamina + s;
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

    private IEnumerator dash()
    {
        if (currentStamina < dashStaminaCost)
        {
            Debug.Log("not enough stamina!");
            yield break;
        }
        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) yield break;

        currentStamina -= dashStaminaCost;
        setAlpha(0.5f);
        rig2D.velocity = new Vector2(dir * dashForce, rig2D.velocity.y);
        rig2D.gravityScale = 0f;
        anim.SetTrigger("Dash");
        uiCanvas.SendMessage("dashTimerEffect");

        yield return new WaitForSeconds(dashDuration);
        setAlpha(1f);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);
        rig2D.gravityScale = 1f;
        dashCooldownCoroutine = StartCoroutine(dashCooldown());
        dashCoroutine = null;
    }

    private void evade(Collider2D c)
    {
        Debug.Log(string.Format("[{0}] {1}", Time.time, "회피!"));
        currentStamina = 100 < currentStamina + dashEvasionGen ? 100 : currentStamina + dashEvasionGen;
        StartCoroutine(delayCollision(c, dashDelay));
    }

    private IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(dashCooltime);
        anim.ResetTrigger("Dash");
        dashCooldownCoroutine = null;
    }

    private void shoot()
    {
        if (currentStamina < shootStaminaCost)
        {
            Debug.Log("not enough stamina!");
            return;
        }
        if (!isProjectileOnScreen()) return;
        projectile.SendMessage("shoot");
        currentStamina -= shootStaminaCost;
        onFired = true;
    }

    private void teleport()
    {
        int count = anim.GetInteger("JumpCount");
        Vector3 pos = projectile.transform.position;
        projectile.SendMessage("stop");
        projectile.transform.position = neck.transform.position;
        transform.position = pos;
        if (2 <= count)
            anim.SetInteger("JumpCount", count - 1);
        onFired = false;
    }

    private void shootCancel()
    {
        projectile.SendMessage("stop");
        currentStamina = 100 < currentStamina + shootCancelStaminaGen ? 100 : currentStamina + shootCancelStaminaGen;
        projectile.transform.position = neck.transform.position;
        onFired = false;
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
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private bool isProjectileOnScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(projectile.transform.position);

        if (pos.x <= 0f || 1f <= pos.x || pos.y <= 0f || 1f <= pos.y) return false;
        return true;
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
            hit[i].transform.TryGetComponent(out tmp);

            if (tmp != null)
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
        if (s1.gameObject.layer < s2.gameObject.layer)
            return ref s2;
        else if (s1.gameObject.layer > s2.gameObject.layer)
            return ref s1;

        if (s1.sortingOrder < s2.sortingOrder)
            return ref s2;
        else if (s1.sortingOrder > s2.sortingOrder)
            return ref s1;

        return ref s1;
    }
}
