using System;
using System.Collections;
using System.Collections.Generic;
using EventManagement;
using Patterns;
using UnityEngine;

namespace Patterns
{
    /*public delegate bool PatternAction(Timeline timeline, PatternInfo patterninfo);
    [Serializable, CreateAssetMenu(menuName="Create New Timeline")]*/
    public class PatternList : MonoBehaviour
    {
        [PatternInfoElementTitle()]
        public PatternInfo[] patterninfo;
        public float[] savePointTime;
        [SerializeField] float startDelayTime;
        [SerializeField] AudioClip music;
        [SerializeField] AudioSource audioSource;

        PatternAction action;
        [HideInInspector]
        public EventManager eventManager;
        //GameObject stage;
        List<Coroutine> coroutineList;
        bool isPuppyShown;

        void Start()
        {

        }

        public void init(PatternAction action)
        {
            this.action += action;
        }

        public void sortPatternInfo()
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

        public IEnumerator Run(float startTime)
        {
            WaitForSeconds delay, repeatDelay;
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
                    if (!this.action(this, patterninfo[i])) yield break;
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
                        if (!this.action(this, patterninfo[i])) yield break;
                    }
                }
            }
        }
    }
}