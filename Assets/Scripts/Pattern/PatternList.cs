using System;
using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using static EventManagement.StageEvent;
using static PlayerEvent;

namespace Patterns
{
    public delegate bool PatternAction(PatternInfo patterninfo);
    public class PatternList : MonoBehaviour
    {
        public float[] savePointTime;
        [SerializeField] float startDelayTime;
        [SerializeField] AudioClip music;
        [SerializeField] AudioSource audioSource;
        [PatternInfoElementTitle()]
        public PatternInfo[] patterninfo;

        PatternAction action;
        [HideInInspector]
        public EventManager eventManager;
        [HideInInspector]
        public GetPatternInfo getPatternInfo;
        List<Coroutine> coroutineList;
        bool isPuppyShown;

        void Start()
        {
            isPuppyShown = false;
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            audioSource.clip = music;

            eventManager.stageEvent.gameStartEvent += gameStartEvent;
            //eventManager.playerEvent.deathEvent += deathEvent;
            eventManager.playerEvent.reviveEvent += gameStartEvent;
            eventManager.savePointTime = savePointTime;

            StartCoroutine(StartWithDelay());
        }

        private IEnumerator StartWithDelay()
        {
            sortPatternByTime();
            yield return new WaitForSeconds(startDelayTime);
            eventManager.stageEvent.gameStartEvent();
        }

        public void init(PatternAction action)
        {
            this.action += action;
        }


        private void gameStartEvent()
        {
            this.Run(audioSource.time);
        }

        void Update()
        {
            if (!isPuppyShown && audioSource.clip.length - 3f < audioSource.time)
            {
                isPuppyShown = true;
                GameObject.Find("puppy").GetComponent<GameClear>().CommingOutFunc();
            }
        }

        /* Sorting Pattern List Func - 패턴 리스트를 정렬하는 함수들 */
        [ContextMenu("Sort By BeginTime(시간순)")]
        public void sortPatternByTime()
        {
            int i, j;
            PatternInfo key;
            for (i = 1; i < patterninfo.Length; i++)
            {
                key = patterninfo[i];
                for (j = i - 1; 0 <= j && key.startAt < patterninfo[j].startAt; j--)
                {
                    patterninfo[j + 1] = patterninfo[j];
                }
                patterninfo[j + 1] = key;
            }
        }

        [ContextMenu("Sort By Same Pattern(같은 패턴끼리)")]
        public void sortPatternWithIdentical()
        {
            /* 
             * 다시 구현해야 함!!!
             * 
             * 
             * 
             * 
            int i, j;
            PatternInfo key;
            for (i = 1; i < patterninfo.Length; i++)
            {
                key = patterninfo[i];
                for (j = i - 1; 0 <= j && i < j; j--)
                {
                    patterninfo[j + 1] = patterninfo[j];
                }
                patterninfo[j + 1] = key;
            }*/
        }

        /* Running Pattern List Func - 패턴 리스트를 실행하는 함수들 */
        public IEnumerator Run(float startTime)
        {
            WaitForSeconds delay, repeatDelay;
            GameObject pattern;
            float delayTime, repeatDelayTime;
            int i = 0, j = 0, repeat;

            for (; i < patterninfo.Length; i++)
            {
                repeat = patterninfo[i].repeatNo;
                delayTime = patterninfo[i].startAt;
                if (i == 0) delayTime -= startTime;
                else delayTime -= patterninfo[i - 1].startAt + ((j - 1) * patterninfo[i - 1].repeatDelayTime);

                if (repeat <= 1)
                {
                    if (patterninfo[i].startAt < startTime) continue;
                    delay = new WaitForSeconds(delayTime);
                    yield return delay;
                    /* Run Pattern - 패턴 실행 */
                    pattern = Instantiate(patterninfo[i].prefab);
                    pattern.transform.SetParent(this.transform);
                    pattern.SetActive(true);
                }
                else
                {
                    repeatDelayTime = patterninfo[i].repeatDelayTime;
                    repeatDelay = new WaitForSeconds(repeatDelayTime);
                    bool isFirstAction = true;
                    for (j = 0; j < repeat; j++)
                    {
                        if (patterninfo[i].startAt + j * repeatDelayTime < startTime) continue;
                        if (isFirstAction)
                        {
                            yield return new WaitForSeconds(delayTime + j * repeatDelayTime - startTime);
                            isFirstAction = false;
                        }
                        else
                        {
                            yield return repeatDelay;
                        }
                        /* Run Pattern - 패턴 실행 */
                        pattern = Instantiate(patterninfo[i].prefab);
                        pattern.transform.SetParent(this.transform);
                        pattern.SetActive(true);
                    }
                }
            }
        }


    }
}