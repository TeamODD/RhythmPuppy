using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("기본 이동속도")] 
    protected float speed;

    [SerializeField, Tooltip("점프 입력 시 y축으로 받을 힘(Force)")] 
    protected float jumpPower;

    [SerializeField, Tooltip("대시 입력 시 x축으로 받을 힘(Force)")]  
    protected float dashPower;

    [SerializeField, Tooltip("대시 종료 후 재사용 대기시간")]  
    protected float dashCooldown;
    
    private Rigidbody2D rig2D;
    private Animator anim;

    /* Basic Functions */
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (!anim.GetBool("bDash"))
            {
                dash();
            }
        }
        if (Input.GetButtonDown("Jump")) 
        {
            if (!anim.GetBool("bJump"))
            {
                jump();
            }
            else if (!anim.GetBool("bDoubleJump")) 
            {
                doubleJump();
            }
        }
    }

    void FixedUpdate()
    {
        checkJumpStatus();
        if (!anim.GetBool("bDash"))
        {
            move();
        }
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");
        
        transform.Translate(new Vector3(h * speed * Time.deltaTime, 0, 0));
    }

    private void jump()
    {
        anim.SetBool("bJump", true);
        rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
    }

    private void doubleJump()
    {
        anim.SetBool("bDoubleJump", true);
        rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, jumpPower * 3f/4f), ForceMode2D.Impulse);
    }

    private void dash()
    {
        StartCoroutine(runDash());
    }

    private IEnumerator runDash()
    {
        anim.SetBool("bDash", true);
        float unit = Input.GetAxisRaw("Horizontal");
        if (unit == 0) yield break;

        float s = dashPower;
        while (7f < s)
        {
            yield return new WaitForFixedUpdate();
            transform.position += new Vector3(unit * s * Time.deltaTime, 0, 0);
            s -= 0.5f;
        }

        anim.SetBool("bDash", false);
        yield break;
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
