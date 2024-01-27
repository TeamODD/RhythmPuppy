using EventManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventManagement.StageEvent;
using static UIEvent;

public class UICanvas : MonoBehaviour
{
    [Serializable]
    struct OverlayCanvas
    {
        [Header("Main Transform")]
        public Transform overlayCanvas;
        [Header("Sub Transform")]
        public Transform darkEffect;
        public Transform redEffect;
        public Transform clearSpotlight;
        public Transform progressBar;
    }
    [Serializable]
    struct WorldSpaceCanvas
    {
        [Header("Main Transform")]
        public Transform worldSpaceCanvas;
        [Header("Sub Transform")]
        public Transform dashTimer;
        public Transform hitTimer;
        public Transform basicTimer;
    }

    [SerializeField] GameObject warningBoxPrefab, warningArrowPrefab;
    [SerializeField] OverlayCanvas overlayCanvas;
    [SerializeField] WorldSpaceCanvas worldSpaceCanvas;

    Player player;
    EventManager eventManager;
    Image darkImage, redImage, clearSpotlightImage, dashTimerImage, hitTimerImage, basicTimerImage;
    Color c;
    CanvasScaler overlayCanvasScaler;
    WaitForSeconds reviveDelay;
    Vector2 baseResolution, currentResolution;

    void Awake()
    {
        baseResolution = new Vector2(1920, 1080);
        currentResolution = new Vector2(Screen.width, Screen.height);
        player = FindObjectOfType<Player>();
        eventManager = FindObjectOfType<EventManager>();
        overlayCanvasScaler = overlayCanvas.overlayCanvas.GetComponent<CanvasScaler>();

        darkImage = overlayCanvas.darkEffect.GetComponent<Image>();
        redImage = overlayCanvas.redEffect.GetComponent<Image>();
        clearSpotlightImage = overlayCanvas.clearSpotlight.GetComponent<Image>();
        dashTimerImage = worldSpaceCanvas.dashTimer.GetComponent<Image>();
        hitTimerImage = worldSpaceCanvas.hitTimer.GetComponent<Image>();
        basicTimerImage = worldSpaceCanvas.basicTimer.GetComponent<Image>();
        reviveDelay = new WaitForSeconds(2);

        eventManager.stageEvent.warnWithBox += warnWithBox;
        eventManager.stageEvent.pauseEvent += enableBlindEvent;
        eventManager.stageEvent.resumeEvent += disableBlindEvent;
        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += reviveEvent;
        eventManager.playerEvent.dashEvent += dashEvent;
        eventManager.uiEvent.fadeInEvent += fadeIn;
        eventManager.uiEvent.fadeOutEvent += fadeOut;
        eventManager.uiEvent.enableBlindEvent += enableBlindEvent;
        eventManager.uiEvent.enableBlindEvent += enableDarkEffect;
        eventManager.uiEvent.disableBlindEvent += disableBlindEvent;
        eventManager.uiEvent.disableBlindEvent += disableDarkEffect;
        eventManager.stageEvent.clearEvent += enableClearSpotlight;
        eventManager.uiEvent.enableClearSpotlightEvent += enableClearSpotlight;
        eventManager.uiEvent.disableClearSpotlightEvent += disableClearSpotlight;
        eventManager.uiEvent.onBlindEvent = false;
        eventManager.stageEvent.onClear = false;
    }

    void Update()
    {
        if (1 < Time.time % 2)  return;
        currentResolution.x = Screen.width;
        currentResolution.y = Screen.height;
        if (!currentResolution.Equals(baseResolution))
        {
            eventManager.uiEvent.resolutionChangeEvent();
            overlayCanvasScaler.scaleFactor = currentResolution.x / baseResolution.x;
        }
    }

    void restartEvent()
    {
        return;
    }

    public void disableClearSpotlight()
    {
        setImageAlpha(ref clearSpotlightImage, 0);
    }

    public void enableClearSpotlight()
    {

        StartCoroutine(runClearSpotlightFadeIn());
    }

    private IEnumerator runClearSpotlightFadeIn()
    {
        float a = 0;

        while (a < 1)
        {
            setImageAlpha(ref clearSpotlightImage, a);
            a += Time.deltaTime;
            yield return null;
        }
        setImageAlpha(ref clearSpotlightImage, 1);
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
        worldSpaceCanvas.worldSpaceCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        eventManager.uiEvent.fadeInEvent();
    }

    private void reviveEvent()
    {
        worldSpaceCanvas.worldSpaceCanvas.gameObject.SetActive(true);
        hitTimerImage.fillAmount = 0;
        setImageAlpha(ref redImage, 0);
        eventManager.uiEvent.fadeOutEvent();

        StartCoroutine(reviveCoroutine());
    }

    private IEnumerator reviveCoroutine()
    {
        yield return reviveDelay;
        StartCoroutine(runTimer(basicTimerImage, 3));
    }

    public void warnWithBox(Vector3 pos, Vector3 size)
    {
        GameObject o = Instantiate(warningBoxPrefab);
        o.transform.SetParent(overlayCanvas.overlayCanvas);
        o.transform.position = pos;
        o.transform.localScale = size;
        o.SetActive(true);
    }
}
