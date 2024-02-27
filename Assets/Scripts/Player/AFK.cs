using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AFK : MonoBehaviour
{
    [SerializeField] SpriteRenderer head;
    [SerializeField] Transform noseBubble;
    [SerializeField] Transform blownBubble;
    [SerializeField] Sprite normalFace;
    [SerializeField] Sprite sleepyFace;
    [SerializeField] Sprite wakeupFace;

    Animator animator;
    float lastInputTime;
    WaitForSeconds wakeupFaceDelay;

    void Awake()
    {
        animator = GetComponent<Animator>();
        wakeupFaceDelay = new WaitForSeconds(0.667f);
        blownBubble.gameObject.SetActive(false);    // Only Activated on WakeUp Animation 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey || Input.GetMouseButtonDown(0))
        {
            // �Է��� �����Ǹ� ���� �ð����� ����
            lastInputTime = Time.time;

            // ���� Sleep ���� ���̾��ٸ�
            if (animator.GetBool("IsAFK"))
            {
                animator.SetBool("IsAFK", false);
                blownBubble.gameObject.SetActive(true);
                StartCoroutine(wakeUpCoroutine());
            }
        }

        // Ư�� �ð�(3��) ���� �Է��� ������ Ư�� ���� ����
        if (Time.time - lastInputTime > 3f)
        {
            // ���⿡ ������ ������ �߰�
            head.sprite = sleepyFace;
            animator.SetBool("IsAFK", true);
        }
    }

    IEnumerator wakeUpCoroutine()
    {
        // While Running Wakeup Animator
        head.sprite = wakeupFace;
        yield return wakeupFaceDelay;

        // After Wakeup Animator 
        blownBubble.gameObject.SetActive(false);
        noseBubble.localScale = Vector3.zero;
        head.sprite = normalFace;
    }
}
