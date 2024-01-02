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
            // �Է��� �����Ǹ� ���� �ð����� ����
            lastInputTime = Time.time;
            animator.SetBool("IsAFK", false);
        }

        // Ư�� �ð�(5��) ���� �Է��� ������ Ư�� ���� ����
        if (Time.time - lastInputTime > 3f)
        {
            // ���⿡ ������ ������ �߰�
            animator.SetBool("IsAFK", true);
        }
    }
}
