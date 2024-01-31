using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EventManagement;

public class Timer : MonoBehaviour
{
    [Header("Dash IFrame(措浆 公利)")]
    public Color dashTimerColor;
    [Header("Hit IFrame(乔拜 矫 公利)")]
    public Color hitTimerColor;
    [Header("Revive IFrame(何劝 饶 公利)")]
    public float reviveDelayTime;
    public Color reviveTimerColor;

    Camera mainCamera;
    EventManager eventManager;
    Transform player;
    Player playerScript;
    Image timer;
    WaitForSeconds reviveDelay;
    Vector3 pos;

    void Awake()
    {
        mainCamera = Camera.main;
        eventManager = FindObjectOfType<EventManager>();
        playerScript = transform.GetComponentInParent<Player>();
        player = playerScript.transform;
        timer = transform.GetComponent<Image>();
        reviveDelay = new WaitForSeconds(reviveDelayTime);

        eventManager.playerEvent.dashEvent += dashEvent;
        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.playerEvent.reviveEvent += reviveEvent;
    }

    void Update()
    {
        pos = mainCamera.WorldToScreenPoint((Vector2)player.transform.position);
        pos.y += 115;
        transform.position = pos;
    }

    public void dashEvent()
    {
        StartCoroutine(runTimer(dashTimerColor, playerScript.dashDuration));
    }
    
    public void playerHitEvent()
    {
        StartCoroutine(runTimer(hitTimerColor, playerScript.invincibleDuration));
    }

    private void reviveEvent()
    {
        StartCoroutine(reviveCoroutine());
    }

    private IEnumerator reviveCoroutine()
    {
        yield return reviveDelay;
        StartCoroutine(runTimer(reviveTimerColor, 3));
    }

    private IEnumerator runTimer(Color c, float t)
    {
        timer.color = c;
        timer.fillAmount = 1;

        while(0 < timer.fillAmount)
        {
            timer.fillAmount -= Time.deltaTime / t;
            yield return null;
        }
        timer.fillAmount = 0;
    }
}
