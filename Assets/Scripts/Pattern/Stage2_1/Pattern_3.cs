using System;
using System.Collections;
using Patterns;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    public class Pattern_3 : MonoBehaviour
    {
        [SerializeField] float[] startDelay;
        [SerializeField] float duration;

        EventManager eventManager;
        Coroutine coroutine;
        PatternInfo patternInfo;

        void Awake()
        {
            patternInfo = GetComponent<PatternBase>().patternInfo;

            if (!patternInfo.duration.Equals(0))
                setDuration(patternInfo.duration - startDelay[startDelay.Length - 1]);
            else if (!patternInfo.endAt.Equals(0))
                setDuration(patternInfo.startAt, patternInfo.endAt);
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onDeath.AddListener(deathEvent);

            coroutine = StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay[startDelay.Length - 1];
        }

        public void setDuration(float start, float end)
        {
            duration = end - start - startDelay[startDelay.Length - 1];
        }

        private IEnumerator runPattern()
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("월드2-1 '패턴3' 프리팹의 startDelay 배열을 검사해주세요.");
            }

            int i = 0;
            yield return new WaitForSeconds(startDelay[i++]);
            eventManager.enableDarkening.Invoke();
            while (i < startDelay.Length)
            {
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.disableDarkening.Invoke();
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.enableDarkening.Invoke();
            }
            yield return new WaitForSeconds(duration);
            eventManager.disableDarkening.Invoke();
        }

        void deathEvent()
        {
            StopCoroutine(coroutine);
            Destroy(gameObject);
        }
    }
}