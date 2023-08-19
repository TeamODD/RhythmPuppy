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

    [Header("�⺻ ����")]
    [Tooltip("ü��")] public int health;

    [Tooltip("���¹̳�")] public float stamina;

    [SerializeField, Tooltip("�⺻ �̵��ӵ�")]
    float speed;

    [Header("����")]
    [SerializeField, Tooltip("���� �Է� �� y������ ���� ��(Force)")]
    float jumpForce;

    [Header("���")]
    [SerializeField, Tooltip("��� �Է� �� x������ ���� ��(Force)")]
    float dashForce;

    [SerializeField, Tooltip("��� �Ҹ� �ð�")]
    float dashTime;

    [SerializeField, Tooltip("��� ���� �� ���� ���ð�")]
    float dashCooltime;

    GameObject uiCanvas, projectile, mark, head, neck;
    Rigidbody2D rig2D;
    CompositeCollider2D col2D;
    SpriteRenderer[] spriteList;
    Animator anim;

    bool onFired;
    float headCorrectFactor;
    Coroutine dashCoroutine, dashCooldownCoroutine, invincibilityCoroutine;
    

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
        RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 1f);
        /*if(raycast.collider != null) Debug.Log(raycast.collider.gameObject.name);*/
    }

    void LateUpdate()
    {
        fixProjectilePos();
        fixPlayerPosition();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        GameObject o = c.gameObject;
        if (LayerMask.NameToLayer("Obstacle").Equals(o.layer))
        {
            StartCoroutine(hitEvent());
        }
    }

    public void init()
    {
        uiCanvas = GameObject.Find("UICanvas");
        projectile = transform.Find("����ü").gameObject;
        mark = transform.Find("ǥ��").gameObject;
        head = transform.Find("�Ӹ�").gameObject;
        neck = head.GetComponent<SpriteSkin>().rootBone.gameObject;

        rig2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<CompositeCollider2D>();
        spriteList = transform.GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();

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
        if (invincibilityCoroutine != null) StopCoroutine(invincibilityCoroutine);
        invincibilityCoroutine = StartCoroutine(activateInvincibility(dashTime));
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

    private IEnumerator ignoreCollision(Collision2D target)
    {
        /*Physics2D.IgnoreLayerCollision(gameObject.layer, target.gameObject.layer, true);*/
        Debug.Log(string.Format("[{0}] {1}", Time.time, "ignore true"));
        yield return new WaitForSeconds(2f);
        /*Physics2D.IgnoreLayerCollision(gameObject.layer, target.gameObject.layer, false);*/
        Debug.Log(string.Format("[{0}] {1}", Time.time, "ignore false"));
        Physics2D.IgnoreCollision(col2D, target.collider, false);
    }

    public void getDamage()
    {
        if (invincibilityCoroutine != null) return;

        /*StartCoroutine(hitEvent());*/
    }

    private IEnumerator hitEvent()
    {
        if (invincibilityCoroutine != null) yield break;

        const float duration = 1.5f;
        invincibilityCoroutine = StartCoroutine(activateInvincibility(duration));

        uiCanvas.SendMessage("hitEffect");
        head.SendMessage("setSadFace");

        health--;
        if (health < 0)
        {
            head.SendMessage("setDeadFace");
            anim.SetTrigger("Death");
            gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(duration);
        head.SendMessage("setNormalFace");
    }

    private IEnumerator activateInvincibility(float duration)
    {
        int obsLayer = LayerMask.NameToLayer("Obstacle");

        Physics2D.IgnoreLayerCollision(gameObject.layer, obsLayer, true);
        setAlpha(0.6f);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, obsLayer, false);
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
}
