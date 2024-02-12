using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class ScreenEffectManager : MonoBehaviour
    {
        [Header("Effect Transform")]
        public Transform darkEffect, redEffect;

        Image darkImage, redImage;
        Color c;
        EventManager eventManager;
        WaitForSeconds loopDelay;

        void Awake()
        {
            darkImage = darkEffect.GetComponent<Image>();
            redImage = redEffect.GetComponent<Image>();
            loopDelay = new WaitForSeconds(0.02f);
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            /* UI Events */
            //eventManager.uiEvent.onBlindEvent = false;
            eventManager.enableDarkening.AddListener(enableDarkEffect);
            //eventManager.uiEvent.enableDarkening.AddListener(enableBlindEvent);  // Lamp Effect in Stage2
            eventManager.disableDarkening.AddListener(disableDarkEffect);
            //eventManager.uiEvent.disableDarkening.AddListener(disableBlindEvent);  // Lamp Effect in Stage2
            eventManager.onFadeIn.AddListener(fadeIn);
            eventManager.onFadeOut.AddListener(fadeOut);
            /* Player Events */
            eventManager.onAttacked.AddListener(playerHitEvent);
            eventManager.onDeath.AddListener(deathEvent);
            eventManager.onRevive.AddListener(reviveEvent);
            /* Stage Events */
            eventManager.onPause.AddListener(enableDarkEffect);
            eventManager.onResume.AddListener(disableDarkEffect);
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
        }

        private IEnumerator runHitEffect()
        {
            float a = 0f;
            setImageAlpha(ref redImage, a);

            while (c.a < 0.6f)
            {
                a += 0.1f;
                setImageAlpha(ref redImage, a);
                yield return loopDelay; // 0.02f
            }
            a = 1f;
            yield return new WaitForSeconds(0.1f);
            while (0 < c.a)
            {
                a -= 0.1f;
                setImageAlpha(ref redImage, a);
                yield return loopDelay; // 0.02f
            }
            a = 0f;
            setImageAlpha(ref redImage, a);
        }

        private void setImageAlpha(ref Image i, float a)
        {
            c = i.color;
            c.a = a;
            i.color = c;
        }

        public void deathEvent()
        {
            StopAllCoroutines();
            setImageAlpha(ref redImage, 0);
            setImageAlpha(ref darkImage, 0);
        }

        public void reviveEvent()
        {
            setImageAlpha(ref redImage, 0);
        }

        /* public void enableBlindEvent()
        {
            eventManager.uiEvent.onBlindEvent = true;
        } */

        /* public void disableBlindEvent()
        {
            eventManager.uiEvent.onBlindEvent = true;
        } */
    }
}