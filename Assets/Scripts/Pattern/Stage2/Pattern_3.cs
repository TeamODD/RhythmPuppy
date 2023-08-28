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
using static EventManagement.StageEvent;

namespace Stage_2
{
    public class Pattern_3 : MonoBehaviour
    {
        public PatternPlaylist patternPlaylist;
        [SerializeField] float[] startDelay;
        [SerializeField] float duration;

        EventManager eventManager;
        AudioSource audioSource;
        Coroutine coroutine;

        void Awake()
        {
            init();
        }

        public void init()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            patternPlaylist.init(action);
            patternPlaylist.sortTimeline();

            coroutine = StartCoroutine(patternPlaylist.Run(audioSource.time));
        }
        public bool action(PatternPlaylist patternPlaylist, Timeline timeline)
        {
            try
            {
                if (!timeline.duration.Equals(0))
                    setDuration(timeline.duration - startDelay[startDelay.Length - 1]);
                else if (!timeline.endAt.Equals(0))
                    setDuration(timeline.startAt, timeline.endAt);

                StartCoroutine(runPattern(timeline));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay[startDelay.Length - 1];
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start - startDelay[startDelay.Length - 1];
        }

        private IEnumerator runPattern(Timeline timeline)
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("월드2-1 '패턴3' 프리팹의 startDelay 배열을 검사해주세요.");
            }

            int i = 0;
            yield return new WaitForSeconds(startDelay[i++]);
            eventManager.uiEvent.enableBlindEvent();
            while (i < startDelay.Length)
            {
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.uiEvent.disableBlindEvent();
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                eventManager.uiEvent.enableBlindEvent();
            }
            yield return new WaitForSeconds(duration);
            eventManager.uiEvent.disableBlindEvent();
        }

        void deathEvent()
        {
            StopAllCoroutines();
        }
    }
}