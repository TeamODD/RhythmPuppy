using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

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

    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        projectileRig2D = projectile.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        onDash = onJump = onShoot = onCancel = false;
        isProjectileFlying = false;
        isShootCooldown = false;
    }

    void Update()
    {
        mousePos = updateMousePos();
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
            shoot(mousePos);
        }
        move();
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");
        
        transform.Translate(new Vector3(h * speed * Time.fixedDeltaTime, 0, 0));
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
            rig2D.AddForce(transform.up * jumpForce * 3f/4f, ForceMode2D.Impulse);
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
        rig2D.velocity = new Vector2(dir * dashForce, rig2D.velocity.y);

        yield return new WaitForSeconds(dashTime);
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);

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
        else if (onCancel)
        {
            onCancel = false;
            projectileRig2D.velocity = Vector2.zero;
            isProjectileFlying = false;
        }
        else
        {
            projectileRig2D.velocity = Vector2.zero;
            transform.position = projectile.transform.position;
            isProjectileFlying = false;
        }
        StartCoroutine(shootCoolDownCoroutine());
    }

    private IEnumerator shootCoolDownCoroutine()
    {
        isShootCooldown = true;
        yield return new WaitForSeconds(shootCooldown);
        isShootCooldown = false;
        onShoot = false;
    }

    private bool isJump()
    {
        RaycastHit2D ray2D = Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if(Mathf.Abs(rig2D.velocity.y) < 0.001f)
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

    private Vector3 updateMousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    private void headToMousePos()
    {
        /* 
         * ����� �Ӹ� ȸ�� �ڵ� �߰� ���� 
         */


        if (!isProjectileFlying)
        {
            Vector3 projectileDir = (mousePos - transform.position);
            if (0.4f < projectileDir.magnitude)
                projectileDir = projectileDir.normalized * 0.4f;

            projectile.transform.position = transform.position + projectileDir;
        }
    }
}
