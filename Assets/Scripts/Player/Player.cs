using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    [Header("목 오브젝트")]
    [SerializeField] GameObject neck;

    [SerializeField, Tooltip("투사체 발사 시 적용되는 힘(Force)")]
    float shootForce;

    [SerializeField, Tooltip("발사 종료 후 재사용 대기시간")]
    float shootCooldown;

    Rigidbody2D rig2D;
    Rigidbody2D projectileRig2D;
    Animator anim;
    bool onDash, onJump, onShoot, onCancel;
    bool isProjectileFlying, isShootCooldown;
    Vector3 mousePos;
    Vector3 neckPos;
    HPManager hpManager;

    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        projectileRig2D = projectile.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        onDash = onJump = onShoot = onCancel = false;
        isProjectileFlying = false;
        isShootCooldown = false;
        hpManager = FindObjectOfType<HPManager>();

        rig2D.gravityScale = 1;
        hpManager.updateHP(health);
    }

    void Update()
    {
        mousePos = updateMousePos();
        neckPos = transform.position + new Vector3(0.7f, 1.2f, 0);
        headToMousePos();

        if (Input.GetButtonDown("Dash"))
        {
            if (!onDash)
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
        if (Input.GetButtonDown("Cancel"))
        {
            if (isProjectileFlying)
                onCancel = true;
        }


    }

    void FixedUpdate()
    {

        checkJumpStatus();
        if (isProjectileFlying) fixProjectilePos();
        /*if (anim.GetBool("bDash"))
            return;*/

        if (onDash && !anim.GetBool("bDash"))
        {
            dash();
        }

        if (onJump)
        {
            onJump = false;
            jump();
        }

        if (onShoot && !isShootCooldown)
        {
            onShoot = false;
            shoot(mousePos - neckPos);
        }
        else if (onCancel && isProjectileFlying)
        {
            onCancel = false;
            shootCancel();
        }

        Vector2 currentPosition = transform.position;
        Vector2 previousPosition = currentPosition - (Time.deltaTime * (Vector2)transform.right); // 1프레임 전 좌표

        if (Vector2.Distance(currentPosition, previousPosition) > 0.03)
            anim.SetBool("IsWalking", true);
        else
            anim.SetBool("IsWalking", false);

        move();
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(h * speed * Time.fixedDeltaTime, 0, 0));

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void jump()
    {
        if (!anim.GetBool("bJump"))
        {
            anim.SetBool("bJump", true);
            rig2D.velocity = new Vector2(rig2D.velocity.x, 0);
            rig2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (!anim.GetBool("bDoubleJump"))
        {
            anim.SetBool("bDoubleJump", true);
            rig2D.velocity = new Vector2(rig2D.velocity.x, 0);
            rig2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        // else { }         // 이미 2단 점프까지 한 경우
    }

    private void dash()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) return;
        StartCoroutine(dashCoroutine(dir));
    }

    private IEnumerator dashCoroutine(float dir)
    {
        anim.SetBool("bDash", true);
        rig2D.velocity = new Vector2(dir * dashForce, 0);
        rig2D.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime);
        rig2D.velocity = new Vector2(0, 0);
        rig2D.gravityScale = 1f;

        yield return new WaitForSeconds(dashCooldown);
        anim.SetBool("bDash", false);
        onDash = false;
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
            anim.SetBool("bDoubleJump", false);
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

    private bool isJump()
    {
        RaycastHit2D ray2D = Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (Mathf.Abs(rig2D.velocity.y) < 0.01f)
        {
            if (ray2D.collider != null)
            {
                if (ray2D.distance < 0.7f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void checkJumpStatus()
    {
        if (isJump())
        {
            anim.SetBool("bJump", true);
        }
        else
        {
            anim.SetBool("bJump", false);
            anim.SetBool("bDoubleJump", false);
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

    private void headToMousePos()
    {
        /* head rotation */
        Vector3 neckDir = (mousePos - neckPos).normalized;
        neck.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(neckDir.x, neckDir.y) * Mathf.Rad2Deg * -1 + 120);

        /* projectile rotation */
        if (!isProjectileFlying)
        {
            Vector3 projectileDir = (mousePos - neckPos);
            if (0.4f < projectileDir.magnitude)
                projectileDir = projectileDir.normalized * 0.4f;

            projectile.transform.position = neckPos + projectileDir;
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
}
