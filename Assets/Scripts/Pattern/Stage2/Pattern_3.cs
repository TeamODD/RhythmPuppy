using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Stage_2
{
    public class Pattern_3 : MonoBehaviour
    {
        [SerializeField]
        float[] startDelay;
        [SerializeField] float duration;

        EventManager eventManager;

        void Start()
        {
            PatternManager patternManager = transform.parent.GetComponent<PatternManager>();
            eventManager = FindObjectOfType<EventManager>();

            StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay.Sum();
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start;
        }

        private IEnumerator runPattern()
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("월드2-1 '패턴3' 프리팹의 startDelay 배열을 검사해주세요.");
            }

            int i = 0;
            yield return new WaitForSeconds(startDelay[i++]);
            eventManager.lampOffEvent();
            while (i < startDelay.Length)
            {
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.lampOnEvent();
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.lampOffEvent();
            }
            yield return new WaitForSeconds(duration);
            eventManager.lampOnEvent();

            yield return new WaitForSeconds(1);
            Destroy(gameObject);
            yield break;
        }
    }
}