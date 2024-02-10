using System;
using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using static EventManagement.StageEvent;

namespace Patterns
{
    public class PatternList : MonoBehaviour
    {
        public float[] savePointTime;
        [SerializeField] float startDelayTime;
        [SerializeField] AudioClip music;
        [SerializeField] AudioSource audioSource;
        [PatternInfoElementTitle()]
        public PatternInfo[] patternInfo;

        [HideInInspector]
        public EventManager eventManager;
        List<Coroutine> coroutineList;
        bool isPuppyShown;
        Player playerScript;

        void Awake()
        {
            isPuppyShown = false;
            coroutineList = new List<Coroutine>();
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            playerScript = FindObjectOfType<Player>();
            audioSource.clip = music;

            eventManager.stageEvent.gameStartEvent += gameStartEvent;
            eventManager.playerEvent.deathEvent += deathEvent;
            playerScript.playerEvent.onRevive.AddListener(gameStartEvent);
            eventManager.savePointTime = savePointTime;
        }

        void Start()
        {
            StartCoroutine(StartWithDelay());
        }

        private IEnumerator StartWithDelay()
        {
            sortPatternByTime();
            yield return new WaitForSeconds(startDelayTime - 1);
            eventManager.stageEvent.gameStartEvent();
        }

        private void gameStartEvent()
        {
            coroutineList.Add(StartCoroutine(Run(audioSource.time)));
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
            for (i = 1; i < patternInfo.Length; i++)
            {
                key = patternInfo[i];
                for (j = i - 1; 0 <= j && key.startAt < patternInfo[j].startAt; j--)
                {
                    patternInfo[j + 1] = patternInfo[j];
                }
                patternInfo[j + 1] = key;
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
            for (i = 1; i < patternInfo.Length; i++)
            {
                key = patternInfo[i];
                for (j = i - 1; 0 <= j && i < j; j--)
                {
                    patternInfo[j + 1] = patternInfo[j];
                }
                patternInfo[j + 1] = key;
            }*/
        }

        /* Running Pattern List Func - 패턴 리스트를 실행하는 함수들 */
        public IEnumerator Run(float startTime)
        {
            GameObject pattern;
            int repeat;

            for (int i = 0; i < patternInfo.Length; i++)
            {
                repeat = patternInfo[i].repeatNo;

                if (repeat <= 1)
                {
                    if (patternInfo[i].startAt < startTime) continue;
                    /* Delay Until StartAt - 패턴 시작시간까지 딜레이 */
                    yield return new WaitForSeconds(patternInfo[i].startAt - audioSource.time - 1);
                    /* Run Pattern - 패턴 실행 */
                    pattern = Instantiate(patternInfo[i].prefab);
                    pattern.transform.SetParent(this.transform);
                    /* Send PatetrnInfo - 패턴 정보 전달 */
                    pattern.GetComponent<PatternBase>().patternInfo = patternInfo[i];
                    pattern.SetActive(true);
                }
                else
                {
                    /* If Repeating Pattern - 반복되는 패턴일 경우 */
                    coroutineList.Add(
                        StartCoroutine(
                            RunRepeating(startTime, patternInfo[i], new WaitForSeconds(patternInfo[i].repeatDelayTime))
                            )
                        );

                }
            }
        }

        /* Running Only Repeating Patterns - 반복 패턴만을 실행하는 함수 */
        private IEnumerator RunRepeating(float startTime, PatternInfo patternInfo, WaitForSeconds delay)
        {
            yield return new WaitForSeconds(patternInfo.startAt - audioSource.time - 1);
            for (int i = 0; i < patternInfo.repeatNo; i++)
            {
                /* 
                 * If Pattern[i]'s StartTime is Before Current StageTime, Skip to Next Loop.
                 * i번째(반복패턴)의 시작시간이 현재 스테이지 시간보다 이전이라면, 스킵하고 다음 루프로 진행. (continue;)
                 */
                if (patternInfo.startAt + i * patternInfo.repeatDelayTime < startTime) continue;

                /* Run Pattern - 패턴 실행 */
                GameObject pattern = Instantiate(patternInfo.prefab);
                pattern.transform.SetParent(this.transform);

                /* Send PatetrnInfo - 패턴 정보 전달 */
                pattern.GetComponent<PatternBase>().patternInfo = patternInfo;
                pattern.SetActive(true);
                yield return delay;
            }
        }

        private void deathEvent()
        {
            /*StopAllCoroutines();*/
            foreach (Coroutine c in coroutineList)
            {
                StopCoroutine(c);
            }
        }

    }
}