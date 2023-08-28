using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] GameObject warningBoxPrefab, warningArrowPrefab;

    Player player;
    EventManager eventManager;
    Transform overlayCanvas, worldSpaceCanvas;
    Transform darkEffect, redEffect, dashTimer, hitTimer, progressBar;
    Image darkImage, redImage, dashTimerImage, hitTimerImage;
    Color c;

    void Awake()
    {
        init();
    }

    public void init()
    {
        player = FindObjectOfType<Player>();
        eventManager = FindObjectOfType<EventManager>();
        overlayCanvas = transform.Find("OverlayCanvas");
        worldSpaceCanvas = transform.Find("WorldSpaceCanvas");

        darkEffect = overlayCanvas.Find("DarkEffect");
        darkImage = darkEffect.GetComponent<Image>();
        redEffect = overlayCanvas.Find("RedEffect");
        redImage= redEffect.GetComponent<Image>();
        dashTimer = worldSpaceCanvas.Find("DashTimer");
        dashTimerImage = dashTimer.GetComponent<Image>();
        hitTimer = worldSpaceCanvas.Find("HitTimer");
        hitTimerImage = hitTimer.GetComponent<Image>();
        progressBar = overlayCanvas.Find("ProgressBar");

        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += reviveEvent;
        eventManager.playerEvent.dashEvent += dashEvent;
        eventManager.uiEvent.fadeInEvent += fadeIn;
        eventManager.uiEvent.fadeOutEvent += fadeOut;
        eventManager.stageEvent.warnWithBox += warnWithBox;
        eventManager.uiEvent.enableBlindEvent += enableBlindEvent;
        eventManager.uiEvent.enableBlindEvent += enableDarkEffect;
        eventManager.uiEvent.disableBlindEvent += disableBlindEvent;
        eventManager.uiEvent.disableBlindEvent += disableDarkEffect;
        eventManager.uiEvent.onBlindEvent = false;
    }

    public void enableBlindEvent()
    {
        eventManager.uiEvent.onBlindEvent = true;
    }

    public void disableBlindEvent()
    {
        eventManager.uiEvent.onBlindEvent = true;
    }

    public void enableDarkEffect()
    {
        setImageAlpha(ref darkImage, 0.8f);
    }

    public void disableDarkEffect()
    {
        setImageAlpha(ref darkImage, 0f);
    }

    public void fadeIn()
    {
        StartCoroutine(runFadeIn());
    }

    private IEnumerator runFadeIn()
    {
        float a = 0;

        while (a < 1)
        {
            setImageAlpha(ref darkImage, a);
            a += Time.deltaTime;
            yield return null;
        }
        setImageAlpha(ref darkImage, 1);
    }

    public void fadeOut()
    {
        StartCoroutine(runFadeOut());
    }

    private IEnumerator runFadeOut()
    {
        float a = 1;

        while (0 < a)
        {
            setImageAlpha(ref darkImage, a);
            a -= Time.deltaTime;
            yield return null;
        }
        setImageAlpha(ref darkImage, 0);
    }

    public void playerHitEvent()
    {
        setImageAlpha(ref redImage, 1f);
        StartCoroutine(runHitEffect());
        StartCoroutine(runTimer(hitTimerImage, player.invincibleDuration));
    }

    private IEnumerator runHitEffect()
    {
        float a = 0f;
        setImageAlpha(ref redImage, a);

        while (c.a < 0.6f)
        {
            a += 0.1f;
            setImageAlpha(ref redImage, a);
            yield return new WaitForSeconds(0.02f);
        }
        a = 1f;
        yield return new WaitForSeconds(0.1f);
        while (0 < c.a)
        {
            a -= 0.1f;
            setImageAlpha(ref redImage, a);
            yield return new WaitForSeconds(0.02f);
        }
        a = 0f;
        setImageAlpha(ref redImage, a);
    }

    public void dashEvent()
    {
        StartCoroutine(runTimer(dashTimerImage, player.dashDuration));
    }

    private IEnumerator runTimer(Image timer, float t)
    {
        timer.fillAmount = 1;

        while(0 < timer.fillAmount)
        {
            timer.fillAmount -= Time.deltaTime / t;
            yield return null;
        }
        timer.fillAmount = 0;
    }

    private void setImageAlpha(ref Image i, float a)
    {
        c = i.color;
        c.a = a;
        i.color = c;
    }

    private void deathEvent()
    {
        StopAllCoroutines();
        StartCoroutine(deathEventCoroutine());
    }

    private IEnumerator deathEventCoroutine()
    {
        hitTimerImage.fillAmount = 0;
        setImageAlpha(ref redImage, 0);
        setImageAlpha(ref darkImage, 0);
        worldSpaceCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventManager.uiEvent.fadeInEvent();
    }

    private void reviveEvent()
    {
        worldSpaceCanvas.gameObject.SetActive(true);
        hitTimerImage.fillAmount = 0;
        setImageAlpha(ref redImage, 0);
        eventManager.uiEvent.fadeOutEvent();
    }

    public void warnWithBox(Vector3 pos, Vector3 size)
    {
        GameObject o = Instantiate(warningBoxPrefab);
        o.transform.SetParent(overlayCanvas);
        o.transform.position = pos;
        o.transform.localScale = size;
        o.SetActive(true);
    }
}
