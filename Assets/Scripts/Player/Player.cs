using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�⺻ ����")]
    [Tooltip("ü��")] public int health;

    [Tooltip("���¹̳�")] public float stamina;

    [SerializeField, Tooltip("�⺻ �̵��ӵ�")]
    float speed;

    [Header("���� ����")]
    [SerializeField, Tooltip("���� �Է� �� y������ ���� ��(Force)")]
    float jumpForce;

    [SerializeField, Tooltip("��� �Է� �� x������ ���� ��(Force)")]
    float dashForce;

    [SerializeField, Tooltip("��� �Ҹ� �ð�")]
    float dashTime;

    [SerializeField, Tooltip("��� ���� �� ���� ���ð�")]
    float dashCooldown;

    [Header("����ü ������Ʈ")]
    [SerializeField] GameObject projectile;

    [Header("�� ������Ʈ")]
    [SerializeField] GameObject neck;

    [SerializeField, Tooltip("����ü �߻� �� ����Ǵ� ��(Force)")]
    float shootForce;

    [SerializeField, Tooltip("�߻� ���� �� ���� ���ð�")]
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
            if(!onDash)
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
        if(isProjectileFlying) fixProjectilePos();
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
        Vector2 previousPosition = currentPosition - (Time.deltaTime * (Vector2)transform.right); // 1������ �� ��ǥ

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
        // else { }         // �̹� 2�� �������� �� ���
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

        if(Mathf.Abs(rig2D.velocity.y) < 0.01f)
        {
            if(ray2D.collider != null)
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
        if(health < 0)
        {
            // �÷��̾� ��� �� üũ����Ʈ Ȯ�� �� ���� �����
            PlayerDie();

            // �÷��̾� ��Ȱ��ȭ
            gameObject.SetActive(false);


        }
    }

@ -327,6 +328,8 @@ public class Player : MonoBehaviour
        {
            getDamage(10);
            //Debug.Log("��ֹ� �浹�� �����Ǿ����ϴ�.");
        }
    }
}