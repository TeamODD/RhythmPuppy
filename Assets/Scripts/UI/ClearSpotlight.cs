using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class ClearSpotlight : MonoBehaviour
    {
        EventManager eventManager;
        UICanvas parent;
        Image clearSpotlightImage;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            parent = GetComponentInParent<UICanvas>();

            clearSpotlightImage = GetComponent<Image>();

            eventManager.stageEvent.clearEvent += enableClearSpotlight;
            eventManager.uiEvent.enableClearSpotlightEvent += enableClearSpotlight;
            eventManager.uiEvent.disableClearSpotlightEvent += disableClearSpotlight;
        }

        public void disableClearSpotlight()
        {
            parent.setImageAlpha(ref clearSpotlightImage, 0);
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
                parent.setImageAlpha(ref clearSpotlightImage, a);
                a += Time.deltaTime;
                yield return null;
            }
            parent.setImageAlpha(ref clearSpotlightImage, 1);
        }
    }
}
