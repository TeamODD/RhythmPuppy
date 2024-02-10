using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using EventManagement;

namespace UIManagement
{
    public class Timer : MonoBehaviour
    {
        public enum TimerType
        {
            Dash,
            Hit,
            Revive,
        }

        [Header("Type (타이머 종류)")]
        public TimerType timerType;
        [Header("Dash IFrame(대쉬 무적)")]
        public Color dashTimerColor;
        [Header("Hit IFrame(피격 시 무적)")]
        public Color hitTimerColor;
        [Header("Revive IFrame(부활 무적)")]
        public float reviveDelayTime;
        public Color reviveTimerColor;

        Camera mainCamera;
        EventManager eventManager;
        Transform player;
        Player playerScript;
        Image timer;
        RectTransform rectTransform;
        WaitForSeconds reviveDelay;
        Vector3 pos, scale;
        Coroutine coroutine;

        void Awake()
        {
            mainCamera = Camera.main;
            eventManager = FindObjectOfType<EventManager>();
            playerScript = transform.GetComponentInParent<Player>();
            player = playerScript.transform;
            timer = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            scale = rectTransform.localScale;
            coroutine = null;

            eventManager.playerEvent.deathEvent += deathEvent;

            /* Timer의 종류에 따라 감지하는 global event의 종류가 다름 */
            switch (timerType)
            {
                case TimerType.Dash:
                    rectTransform.localScale = scale * 1.4f;    // 크기 조절
                    eventManager.playerEvent.dashEvent += dashEvent;
                    break;
                case TimerType.Hit:
                    rectTransform.localScale = scale * 1.0f;    // 크기 조절
                    eventManager.playerEvent.playerHitEvent += playerHitEvent;
                    break;
                case TimerType.Revive:
                    rectTransform.localScale = scale * 0.7f;    // 크기 조절
                    reviveDelay = new WaitForSeconds(reviveDelayTime);
                    playerScript.playerEvent.onRevive.AddListener(reviveEvent);
                    break;
            }
        }

        void Update()
        {
            pos = mainCamera.WorldToScreenPoint((Vector2)player.transform.position);
            pos.y += 115;
            transform.position = pos;
        }

        public void dashEvent()
        {
            coroutine = StartCoroutine(runTimer(dashTimerColor, playerScript.dashDuration));
        }

        public void playerHitEvent()
        {
            coroutine = StartCoroutine(runTimer(hitTimerColor, playerScript.hitIFrame));
        }

        public void reviveEvent()
        {
            coroutine = StartCoroutine(reviveCoroutine());
        }

        private IEnumerator reviveCoroutine()
        {
            yield return reviveDelay;
            coroutine = StartCoroutine(runTimer(reviveTimerColor, playerScript.reviveIFrame));
        }

        private IEnumerator runTimer(Color c, float t)
        {
            timer.color = c;
            timer.fillAmount = 1;

            while (0 < timer.fillAmount)
            {
                timer.fillAmount -= Time.deltaTime / t;
                yield return null;
            }
            timer.fillAmount = 0;
        }

        private void deathEvent()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            timer.fillAmount = 0;

        }
    }
}