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

        /* Sorting Pattern List Func - ���� ����Ʈ�� �����ϴ� �Լ��� */
        [ContextMenu("Sort By BeginTime(�ð���)")]
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

        [ContextMenu("Sort By Same Pattern(���� ���ϳ���)")]
        public void sortPatternWithIdentical()
        {
            /* 
             * �ٽ� �����ؾ� ��!!!
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

        /* Running Pattern List Func - ���� ����Ʈ�� �����ϴ� �Լ��� */
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
                    /* Delay Until StartAt - ���� ���۽ð����� ������ */
                    yield return new WaitForSeconds(patternInfo[i].startAt - audioSource.time - 1);
                    /* Run Pattern - ���� ���� */
                    pattern = Instantiate(patternInfo[i].prefab);
                    pattern.transform.SetParent(this.transform);
                    /* Send PatetrnInfo - ���� ���� ���� */
                    pattern.GetComponent<PatternBase>().patternInfo = patternInfo[i];
                    pattern.SetActive(true);
                }
                else
                {
                    /* If Repeating Pattern - �ݺ��Ǵ� ������ ��� */
                    coroutineList.Add(
                        StartCoroutine(
                            RunRepeating(startTime, patternInfo[i], new WaitForSeconds(patternInfo[i].repeatDelayTime))
                            )
                        );

                }
            }
        }

        /* Running Only Repeating Patterns - �ݺ� ���ϸ��� �����ϴ� �Լ� */
        private IEnumerator RunRepeating(float startTime, PatternInfo patternInfo, WaitForSeconds delay)
        {
            yield return new WaitForSeconds(patternInfo.startAt - audioSource.time - 1);
            for (int i = 0; i < patternInfo.repeatNo; i++)
            {
                /* 
                 * If Pattern[i]'s StartTime is Before Current StageTime, Skip to Next Loop.
                 * i��°(�ݺ�����)�� ���۽ð��� ���� �������� �ð����� �����̶��, ��ŵ�ϰ� ���� ������ ����. (continue;)
                 */
                if (patternInfo.startAt + i * patternInfo.repeatDelayTime < startTime) continue;

                /* Run Pattern - ���� ���� */
                GameObject pattern = Instantiate(patternInfo.prefab);
                pattern.transform.SetParent(this.transform);

                /* Send PatetrnInfo - ���� ���� ���� */
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