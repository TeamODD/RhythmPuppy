using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TimelineManager
{
    [Serializable]
    public class PatternAction
    {
        public PatternDetail type;
        public UnityAction action;
    }

    [Serializable]
    public class Playlist
    {
        public GameObject prefab;
        public PatternAction[] patternInfo;
        [TimelineElementTitle()]
        public Timeline[] timeline;

        public void defineAction(UnityAction func)
        {
            for (int i = 0; i < timeline.Length; i++)
            {
                timeline[i].defineAction(func);
            }
        }

        public void sortTimeline()
        {
            int i, j;
            Timeline key;
            for (i = 1; i < timeline.Length; i++)
            {
                key = timeline[i];
                for (j = i - 1; 0 <= j && key.startAt < timeline[j].startAt; j--)
                {
                    timeline[j + 1] = timeline[j];
                }
                timeline[j + 1] = key;
            }
        }

        public IEnumerator Run()
        {
            float sum = 0f;

            for (int i = 0; i < timeline.Length; i++) 
            {
                float startAt = timeline[i].startAt;
                yield return new WaitForSeconds(startAt - sum);
                sum += startAt;

                if (timeline[i].detail.repeatNo == 0)
                {
                    /*actionFunc(this, timeline[i]);*/
                    timeline[i].runAction();
                }
                else
                {
                    int n = timeline[i].detail.repeatNo;
                    float delay = timeline[i].detail.repeatDelayTime;
                    for (int j = 0; j < n; j++)
                    {
                        /*actionFunc(this, timeline[i]);*/
                        timeline[i].runAction();
                        if (j == n) break;
                        yield return new WaitForSeconds(delay);
                        sum += delay;
                    }
                }
            }
            yield break;
        }
    }

    [Serializable]
    public class Timeline
    {
        [Tooltip("패턴 시작 시간")]
        public float startAt;
        public Detail detail;

        UnityEvent action;

        public void defineAction(UnityAction func)
        {
            action = new UnityEvent();
            action.AddListener(func);
        }

        public void runAction()
        {
            action.Invoke();
        }
    }

    [AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class PatternArrayElementTitleAttribute : PropertyAttribute
    {
        public PatternArrayElementTitleAttribute() { }
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class TimelineElementTitleAttribute : PropertyAttribute
    {
        public TimelineElementTitleAttribute() { }
    }
}