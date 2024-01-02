using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFK : MonoBehaviour
{
    public Animator animator;
    public Sprite normal;
    public Sprite Sleeping;
    public Sprite WakeUp;
    private float lastInputTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey || Input.GetMouseButtonDown(0))
        {
            // 입력이 감지되면 현재 시간으로 갱신
            lastInputTime = Time.time;
            animator.SetBool("IsAFK", false);
        }

        // 특정 시간(5초) 동안 입력이 없으면 특정 동작 수행
        if (Time.time - lastInputTime > 3f)
        {
            // 여기에 수행할 동작을 추가
            animator.SetBool("IsAFK", true);
        }
    }
}
