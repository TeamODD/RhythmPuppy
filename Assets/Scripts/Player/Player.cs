using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour
{
    enum InputType
    {

        Dash,
        Jump,
        Shoot,
        ShootCancel,
    }

    [Header("기본 정보")]
    [Tooltip("체력")] public int health;

    [Tooltip("스태미나")] public float stamina;

    [SerializeField, Tooltip("기본 이동속도")]
    float speed;

    [Header("세부 정보")]
    [SerializeField, Tooltip("점프 입력 시 y축으로 받을 힘(Force)")]
    float jumpForce;

    [SerializeField, Tooltip("대시 입력 시 x축으로 받을 힘(Force)")]
    float dashForce;

    [SerializeField, Tooltip("대시 소모 시간")]
    float dashTime;

    [SerializeField, Tooltip("대시 종료 후 재사용 대기시간")]
    float dashCooldown;

    [Header("투사체 오브젝트")]
    [SerializeField] GameObject projectile;

    [Header("머리 오브젝트")]
    [SerializeField] GameObject head;

    [SerializeField, Tooltip("투사체 발사 시 적용되는 힘(Force)")]
    float shootForce;

    [SerializeField, Tooltip("발사 종료 후 재사용 대기시간")]
    float shootCooldown;

    Rigidbody2D rig2D, projectileRig2D;
    Transform neck;
    HPManager hpManager;
    Animator anim;

    /** head 각도 보정치*/
    float headCorrFactor { get; } = 52f;
    /** 투사체 각도 보정치 */
    float projCorrFactor { get; } = 11f;
    /** 투사체 - 목 사이의 간격 */
    float projRad { get; } = 0.7f;
    /** 키 입력 임시 버퍼 */
    bool onDash, onJump, onShoot, onCancel;
    bool isProjectileFlying, isShootCooldown;
    Vector3 mousePos;
    Coroutine dashCoroutine;

    void Update()
    {
        mousePos = updateMousePos();
        if (Input.GetAxisRaw("Horizontal").Equals(0))
            anim.SetBool("bAxisInput", false);
        else
            anim.SetBool("bAxisInput", true);
        headToMousePos();

        if (Input.GetButtonDown("Dash"))
        {
            onDash = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            onJump = true;
        }
        if (Input.GetButtonDown("Shoot"))
        {
            onShoot = true;
        }
        if (Input.GetButtonDown("ShootCancel"))
        {
            if (isProjectileFlying)
                onCancel = true;
        }
    }

    void FixedUpdate()
    {
        flipBody();
        checkJumpStatus();
        if (isProjectileFlying) fixProjectilePos();

        if (onDash && dashCoroutine == null)
        {
            onDash = false;
            dashCoroutine = StartCoroutine(dash());
        }

        if (onJump)
        {
            onJump = false;
            jump();
        }

        if (onShoot && !isShootCooldown)
        {
            onShoot = false;
            shoot(mousePos - head.transform.position);
        }
        else if (onCancel && isProjectileFlying)
        {
            onCancel = false;
            shootCancel();
        }

        move();
        moveIntoCamera();
    }

    public void init()
    {
        this.rig2D = GetComponent<Rigidbody2D>();
        this.projectileRig2D = projectile.GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.hpManager = FindObjectOfType<HPManager>();
        this.neck = head.GetComponent<SpriteSkin>().rootBone;

        onDash = onJump = onShoot = onCancel = false;
        isProjectileFlying = false;
        isShootCooldown = false;
        dashCoroutine = null;

        anim.ResetTrigger("Jump");
        anim.SetInteger("JumpCount", 0);

        hpManager.updateHP(health);
    }

    private void move()
    {
        float xInput = Input.GetAxis("Horizontal");
        if (xInput == 0) return;

        rig2D.velocity = new Vector2(xInput * speed, rig2D.velocity.y);
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
                /*rig2D.velocity = new Vector2(rig2D.velocity.x, 0);*/
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
        /*rig2D.velocity = new Vector2(dir * dashForce, 0);*/
        rig2D.velocity = new Vector2(dir * dashForce * Time.fixedDeltaTime, rig2D.velocity.y);
        rig2D.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);
        rig2D.gravityScale = 1f;

        yield return new WaitForSeconds(dashCooldown);
        anim.ResetTrigger("Dash");
        dashCoroutine = null;
        yield break;
    }

    private void shoot(Vector3 m)
    {
        if (!isProjectileFlying)
        {
            isProjectileFlying = true;
            projectileRig2D.velocity = new Vector2(m.normalized.x, m.normalized.y) * shootForce;
            return;
        }
        else
        {
            projectileRig2D.velocity = Vector2.zero;
            transform.position = projectile.transform.position;
            if (2 <= anim.GetInteger("JumpCount"))
                anim.SetInteger("JumpCount", 1);
            isProjectileFlying = false;
        }
        StartCoroutine(shootCoolDownCoroutine());
    }

    private void shootCancel()
    {
        onCancel = false;
        projectileRig2D.velocity = Vector2.zero;
        isProjectileFlying = false;
        StartCoroutine(shootCoolDownCoroutine());
    }

    private IEnumerator shootCoolDownCoroutine()
    {
        isShootCooldown = true;
        yield return new WaitForSeconds(shootCooldown);
        isShootCooldown = false;
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

    private void fixProjectilePos()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(projectile.transform.position);
        bool isFixed = false;

        if (pos.x <= 0f)
        {
            pos.x = 0f;
            isFixed = true;
        }
        if (1f <= pos.x)
        {
            pos.x = 1f;
            isFixed = true;
        }
        if (pos.y <= 0f)
        {
            pos.y = 0f;
            isFixed = true;
        }
        if (1f <= pos.y)
        {
            pos.y = 1f;
            isFixed = true;
        }

        if (isFixed)
        {
            projectileRig2D.velocity = Vector2.zero;
            projectile.transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
    }

    private Vector3 updateMousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    private void flipBody()
    {
        const float detailCorrFactor = 6f;
        Vector3 flip = transform.localScale;
        float rot = neck.localRotation.eulerAngles.z;

        rot = (rot - headCorrFactor + detailCorrFactor);
        if (90 < rot && rot < 270)
            flip.x *= -1;
        transform.localScale = flip;
    }

    private void headToMousePos()
    {
        Vector2 dir;
        float rot, headRot, projRot;

        dir = mousePos - head.transform.position;
        rot = 0 < dir.y ? Vector2.Angle(dir, Vector2.right) : 360f - Vector3.Angle(dir, Vector2.right);

        /* head rotation */
        headRot = rot + headCorrFactor;
        if (transform.localScale.x < 0) headRot = rot + (180 - headCorrFactor);
        neck.rotation = Quaternion.Euler(0, 0, headRot);

        /* projectile rotation */
        if (!isProjectileFlying)
        {
            projRot = rot + projCorrFactor;
            if (transform.localScale.x < 0) projRot = rot - projCorrFactor;
            Quaternion q = Quaternion.Euler(0, 0, projRot);
            dir = q * Vector2.right;

            if (projRad < dir.magnitude)
                dir = dir.normalized * projRad;
            projectile.transform.position = neck.transform.position + (Vector3)dir;
        }

    }

    public void getDamage(int damage)
    {
        health -= damage;
        hpManager.updateHP(health);
        if (health < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void moveIntoCamera()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
