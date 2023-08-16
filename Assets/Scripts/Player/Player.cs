using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour
{
    enum PlayerAction
    {
        Dash,
        Jump,
        Shoot,
        ShootCancel,
    }

    [Header("기본 스탯")]
    [Tooltip("체력")] public int health;

    [Tooltip("스태미나")] public float stamina;

    [SerializeField, Tooltip("기본 이동속도")]
    float speed;

    [Header("점프")]
    [SerializeField, Tooltip("점프 입력 시 y축으로 받을 힘(Force)")]
    float jumpForce;

    [Header("대시")]
    [SerializeField, Tooltip("대시 입력 시 x축으로 받을 힘(Force)")]
    float dashForce;

    [SerializeField, Tooltip("대시 소모 시간")]
    float dashTime;

    [SerializeField, Tooltip("대시 종료 후 재사용 대기시간")]
    float dashCooltime;

    GameObject uiCanvas, projectile, mark, head, neck;
    Rigidbody2D rig2D;
    HPManager hpManager;
    Animator anim;

    bool onFired;
    float headCorrectFactor;
    Coroutine dashCoroutine, dashCooldownCoroutine;
    

    void Awake()
    {
        init();
    }

    void Update()
    {
        checkJumpStatus();

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
        flipBody();
        move();
    }

    void LateUpdate()
    {
        fixProjectilePos();
        fixPlayerPosition();
    }

    public void init()
    {
        uiCanvas = GameObject.Find("UICanvas");
        projectile = transform.Find("투사체").gameObject;
        mark = transform.Find("표식").gameObject;
        head = transform.Find("머리").gameObject;
        neck = head.GetComponent<SpriteSkin>().rootBone.gameObject;

        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hpManager = FindObjectOfType<HPManager>();

        onFired = false;
        dashCoroutine = dashCooldownCoroutine = null;
        headCorrectFactor = neck.transform.rotation.eulerAngles.z + head.transform.rotation.eulerAngles.z;
        
        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

        hpManager.updateHP(health);
    }

    public void activateMark()
    {
        mark.SendMessage("activate");
    }

    public void inactivateMark()
    {
        mark.SendMessage("inactivate");
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
        RaycastHit2D ray2D = Physics2D.Raycast(transform.position, Vector2.down, 1, LayerMask.GetMask("Ground"));
        const float margin = 0.05f;

        if (Mathf.Abs(rig2D.velocity.y) < margin)
        {
            if (ray2D.collider != null)
            {
                if (ray2D.distance < margin)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private IEnumerator dash()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) yield break;

        anim.SetTrigger("Dash");
        rig2D.velocity = new Vector2(dir * dashForce, rig2D.velocity.y);
        rig2D.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);
        rig2D.gravityScale = 1f;
        dashCooldownCoroutine = StartCoroutine(dashCooldown());
        dashCoroutine = null;
    }

    private IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(dashCooltime);
        anim.ResetTrigger("Dash");
        dashCooldownCoroutine = null;
    }

    private void shoot()
    {
        if (!isProjectileOnScreen()) return;
        projectile.SendMessage("shoot");
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

    private void fixPlayerPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void fixProjectilePos()
    {
        if (!onFired) return;

        Vector3 pos = Camera.main.WorldToViewportPoint(projectile.transform.position);
        Vector3 copy = pos;

        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;

        if (!pos.Equals(copy))
        {
            projectile.SendMessage("stop");
            projectile.transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
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

    public void getDamage()
    {
        if (dashCoroutine != null)
        {
            Debug.Log("회피!");
            return;
        }

        uiCanvas.SendMessage("hitEffect");
        health--;
        hpManager.updateHP(health);
        if (health < 0)
        {
            anim.SetTrigger("Death");
            gameObject.SetActive(false);
        }
    }
}
