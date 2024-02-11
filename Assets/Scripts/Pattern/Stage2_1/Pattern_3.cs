using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using EventManagement;

namespace Stage_2
{
    public class Pattern_3 : MonoBehaviour
    {
        [SerializeField] float[] startDelay;
        [SerializeField] float duration;

        EventManager eventManager;
        AudioSource audioSource;
        Coroutine coroutine;
        PatternInfo patternInfo;

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            patternInfo = GetComponent<PatternBase>().patternInfo;

            eventManager.onDeath.AddListener(deathEvent);

            if (!patternInfo.duration.Equals(0))
                setDuration(patternInfo.duration - startDelay[startDelay.Length - 1]);
            else if (!patternInfo.endAt.Equals(0))
                setDuration(patternInfo.startAt, patternInfo.endAt);

            coroutine = StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay[startDelay.Length - 1];
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start - startDelay[startDelay.Length - 1];
        }

        private IEnumerator runPattern()
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("����2-1 '����3' �������� startDelay �迭�� �˻����ּ���.");
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