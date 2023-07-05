using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField, Tooltip("ü��")]
    protected int health;

    [SerializeField, Tooltip("���¹̳�")]
    protected float stamina;

    [SerializeField, Tooltip("�⺻ �̵��ӵ�")]
    protected float speed;

    [SerializeField, Tooltip("���� �Է� �� y������ ���� ��(Force)")] 
    protected float jumpForce;

    [SerializeField, Tooltip("��� �Է� �� x������ ���� ��(Force)")]  
    protected float dashForce;

    [SerializeField, Tooltip("��� �Ҹ� �ð�")]  
    protected float dashTime;

    [SerializeField, Tooltip("��� ���� �� ���� ���ð�")]  
    protected float dashCooldown;
    
    [SerializeField, Header("����ü ������Ʈ")]
    protected GameObject projectile;

    [SerializeField, Header("�� ������Ʈ")]
    protected GameObject neck;

    [SerializeField, Tooltip("����ü �߻� �� ����Ǵ� ��(Force)")]
    protected float shootForce;

    [SerializeField, Tooltip("�߻� ���� �� ���� ���ð�")]
    protected float shootCooldown;

    private Rigidbody2D rig2D;
    private Rigidbody2D projectileRig2D;
    private Animator anim;
    private bool onDash, onJump, onShoot, onCancel;
    private bool isProjectileFlying, isShootCooldown;
    private Vector3 mousePos;
    private Vector3 neckPos;

    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        projectileRig2D = projectile.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        onDash = onJump = onShoot = onCancel = false;
        isProjectileFlying = false;
        isShootCooldown = false;

        rig2D.gravityScale = 1;
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
}
