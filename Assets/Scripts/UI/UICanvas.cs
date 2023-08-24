using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
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

    public void hitEffect()
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

    public void dashTimerEffect()
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

    public void disablePlayerUI()
    {
        worldSpaceCanvas.gameObject.SetActive(false);
        disableDarkEffect();
    }

    public void enablePlayerUI()
    {
        worldSpaceCanvas.gameObject.SetActive(true);
    }
}
