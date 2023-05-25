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

    [SerializeField, Tooltip("��� ���� �� ���� ���ð�")]  
    protected WaitForSeconds dashCooldown;
    
    private Rigidbody2D rig2D;
    private Animator anim;
    private bool onDash, onJump;

    /* Basic Functions */
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        onDash = onJump = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (!onDash) 
                onDash = true;
        }
        if (Input.GetButtonDown("Jump")) 
        {
            onJump = true;
        }
    }

    void FixedUpdate()
    {
        checkJumpStatus();
        if (onDash) 
        {
            dash();
        }
        if (onJump) 
        {
            onJump = false;
            jump();
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
            rig2D.velocity = new Vector2(0, 0);
            rig2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (!anim.GetBool("bDoubleJump")) 
        {
            anim.SetBool("bDoubleJump", true);
            rig2D.velocity = new Vector2(0, 0);
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
        anim.SetTrigger("tDash");
        rig2D.velocity = new Vector2(dir * dashForce, 0);

        yield return dashCooldown;
        onDash = false;
    }

    private bool isJump()
    {
        RaycastHit2D ray2D = Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if(Mathf.Abs(rig2D.velocity.y) < 0.001f)
        {
            if(ray2D.collider != null)
            {
                if (ray2D.distance < 0.5f)
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
}
