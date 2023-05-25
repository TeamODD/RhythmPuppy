using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("기본 이동속도")] 
    protected float speed;

    [SerializeField, Tooltip("점프 입력 시 y축으로 받을 힘(Force)")] 
    protected float jumpForce;

    [SerializeField, Tooltip("대시 입력 시 x축으로 받을 힘(Force)")]  
    protected float dashForce;

    [SerializeField, Tooltip("대시 종료 후 재사용 대기시간")]  
    protected float dashCooldown;

    private enum Status
    {
        Idle,
        Move,
        Jump,
        DoubleJump,
        Dash
    }
    
    private Rigidbody2D rig2D;
    private Animator anim;
    private Status currStatus;

    /* Basic Functions */
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currStatus = Status.Idle;
    }

    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            dash();
        }
        if (Input.GetButtonDown("Jump")) 
        {
            jump();
        }
    }

    void FixedUpdate()
    {
        checkJumpStatus();
        move();
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");
        
        transform.Translate(new Vector3(h * speed * Time.deltaTime, 0, 0));
    }

    private void jump()
    {
        if (anim.GetBool("bDoubleJump")) 
            return;

        if (!anim.GetBool("bJump"))
            StartCoroutine(jumpCoroutine(jumpForce));
        else 
            StartCoroutine(jumpCoroutine(jumpForce * 4f/5f));
        /*rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);*/
    }

    private IEnumerator jumpCoroutine(float force)
    {
        yield return new WaitForFixedUpdate();
        anim.SetBool("bJump", true);
        rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    }

    private void doubleJump()
    {
        anim.SetBool("bDoubleJump", true);
        rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, jumpForce * 3f/4f), ForceMode2D.Impulse);
    }

    private void dash()
    {
        anim.SetBool("bDash", true);
        StartCoroutine(dashCoroutine());
    }

    private IEnumerator dashCoroutine()
    {
        float unit = Input.GetAxisRaw("Horizontal");
        if (unit == 0) yield break;

        float s = dashForce;
        while (7f < s)
        {
            yield return new WaitForFixedUpdate();
            transform.position += new Vector3(unit * s * Time.deltaTime, 0, 0);
            s -= 0.5f;
        }

        anim.SetBool("bDash", false);
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
            currStatus = Status.Jump;
        }
        else
        {
            anim.SetBool("bJump", false);
            anim.SetBool("bDoubleJump", false);
            currStatus = Status.Idle;
        }
    }
}
