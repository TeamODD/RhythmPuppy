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
            // 입력이 감지되면 현재 시간으로 갱신
            lastInputTime = Time.time;

            // 만약 Sleep 동작 중이었다면
            if (animator.GetBool("IsAFK"))
            {
                animator.SetBool("IsAFK", false);
                blownBubble.gameObject.SetActive(true);
                StartCoroutine(wakeUpCoroutine());
            }
        }

        // 특정 시간(3초) 동안 입력이 없으면 특정 동작 수행
        if (Time.time - lastInputTime > 3f)
        {
            // 여기에 수행할 동작을 추가
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
