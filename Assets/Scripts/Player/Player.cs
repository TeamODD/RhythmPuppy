using Autodesk.Fbx;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public int health;
    public float stamina;
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


    const float G = 9.8f;

    GameObject uiCanvas, projectile, mark, head, neck;
    Collider2D hitbox;
    Rigidbody2D rig2D;
    SpriteRenderer[] spriteList;
    Animator anim;

    bool onFired;
    float headCorrectFactor;
    Vector3 velocity;
    Coroutine dashCoroutine, dashCooldownCoroutine, invincibilityCoroutine;

    WaitForSeconds invincibleDelay, dashDelay;

    void Awake()
    {
        init();
    }

    void Update()
    {
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
    }

    void FixedUpdate()
    {
        velocity = rig2D.velocity;
        flipBody();
        move();
    }

    void LateUpdate()
    {
        fixPositionIntoScreen();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            rig2D.velocity = velocity;
            /*StartCoroutine(delayCollision(c.collider));*/
            if (!isCollisionVisibleOnTheScreen(c)) return;

            if (dashCoroutine != null) evade(c.collider);
            else StartCoroutine(hitEvent());
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(c.gameObject.layer))
        {
            /*StartCoroutine(delayCollision(c));*/

            if (dashCoroutine != null) evade(c);
            else StartCoroutine(hitEvent());
        }
    }

    public void init()
    {
        uiCanvas = GameObject.Find("UICanvas");
        projectile = transform.Find("Projectile").gameObject;
        mark = transform.Find("Mark").gameObject;
        head = transform.Find("Head").gameObject;
        neck = head.GetComponent<SpriteSkin>().rootBone.gameObject;

        rig2D = GetComponent<Rigidbody2D>();
        hitbox = transform.GetComponentInChildren<CapsuleCollider2D>();
        spriteList = transform.GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();

        invincibleDelay = new WaitForSeconds(invincibleDuration);
        dashDelay = new WaitForSeconds(dashDuration);

        onFired = false;
        dashCoroutine = dashCooldownCoroutine = invincibilityCoroutine = null;
        headCorrectFactor = neck.transform.rotation.eulerAngles.z + head.transform.rotation.eulerAngles.z;

        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);
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
        stamina = 100 < stamina + s ? 100 : stamina + s;
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
        if (stamina < dashStaminaCost)
        {
            Debug.Log("not enough stamina!");
            yield break;
        }
        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) yield break;

        stamina -= dashStaminaCost;
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
        stamina = 100 < stamina + dashEvasionGen ? 100 : stamina + dashEvasionGen;
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
        if (stamina < shootStaminaCost)
        {
            Debug.Log("not enough stamina!");
            return;
        }
        if (!isProjectileOnScreen()) return;
        projectile.SendMessage("shoot");
        stamina -= shootStaminaCost;
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
        stamina = 100 < stamina + shootCancelStaminaGen ? 100 : stamina + shootCancelStaminaGen;
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

    private IEnumerator hitEvent()
    {
        if (invincibilityCoroutine != null) yield break;

        invincibilityCoroutine = StartCoroutine(activateInvincibility());

        uiCanvas.SendMessage("hitEffect");
        head.SendMessage("setSadFace");

        health--;
        if (health < 0)
        {
            head.SendMessage("setDeadFace");
            anim.SetTrigger("Death");
            gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(invincibleDuration);
        head.SendMessage("setNormalFace");
    }

    private IEnumerator activateInvincibility()
    {
        /*int obsLayer = LayerMask.NameToLayer("Obstacle");

        Physics2D.IgnoreLayerCollision(gameObject.layer, obsLayer, true);*/
        hitbox.enabled = false;
        setAlpha(0.5f);
        yield return invincibleDelay;
        /*Physics2D.IgnoreLayerCollision(gameObject.layer, obsLayer, false);*/
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
