using System.Collections;
using UnityEngine;

namespace TimelineManager
{
   public delegate void Action(Pattern p, Timeline t);

    [System.Serializable]
    public class Pattern
    {
        public GameObject prefab;
        [TimelineElementTitle()]
        public Timeline[] timeline;
        public Action actionFunc;

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
                    actionFunc(this, timeline[i]);       
                }
                else
                {
                    float delay = timeline[i].detail.repeatDelayTime;
                    for (int j = 0; j < timeline[i].detail.repeatNo; j++)
                    {
                        actionFunc(this, timeline[i]);
                        yield return new WaitForSeconds(delay);
                        sum += delay;
                    }
                }
            }
            yield break;
        }
    }

    [System.Serializable]
    public class Timeline
    {
        [Tooltip("패턴 시작 시간")]
        public float startAt;
        public Detail detail;
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
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