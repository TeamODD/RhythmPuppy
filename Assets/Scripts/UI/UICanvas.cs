using EventManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UICanvas : MonoBehaviour
    {
        [Serializable]
        struct OverlayCanvas
        {
            [Header("Main(Canvas) Transform")]
            public Transform overlayCanvas;
            [Header("Sub Transform")]
            public Transform clearSpotlight;
        }
        [Serializable]
        struct WorldSpaceCanvas
        {
            [Header("Main Transform")]
            public Transform worldSpaceCanvas;
        }

        [SerializeField] OverlayCanvas overlayCanvas;
        [SerializeField] WorldSpaceCanvas worldSpaceCanvas;

        Player player;
        EventManager eventManager;
        Image clearSpotlightImage;
        Color c;
        CanvasScaler overlayCanvasScaler;
        Vector2 baseResolution, currentResolution;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            player = FindObjectOfType<Player>();
            baseResolution = new Vector2(1920, 1080);
            currentResolution = new Vector2(Screen.width, Screen.height);
            overlayCanvasScaler = overlayCanvas.overlayCanvas.GetComponent<CanvasScaler>();
            clearSpotlightImage = overlayCanvas.clearSpotlight.GetComponent<Image>();

            eventManager.playerEvent.deathEvent += deathEvent;
            player.playerEvent.onRevive.AddListener(reviveEvent);
            eventManager.stageEvent.clearEvent += enableClearSpotlight;
            eventManager.uiEvent.enableClearSpotlightEvent += enableClearSpotlight;
            eventManager.uiEvent.disableClearSpotlightEvent += disableClearSpotlight;
            eventManager.stageEvent.onClear = false;
        }

        void Update()
        {
            if (1 < Time.time % 2) return;
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
            //StartCoroutine(runClearSpotlightFadeIn());
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
            yield return new WaitForSeconds(2f);
            eventManager.uiEvent.fadeInEvent();
        }

        private void reviveEvent()
        {
            eventManager.uiEvent.fadeOutEvent();
        }
    }
}