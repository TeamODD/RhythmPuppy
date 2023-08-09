using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TimelineManager
{

    [Serializable]
    public class Playlist
    {
        public GameObject prefab;
        public PatternType type;
        [TimelineElementTitle()]
        public Timeline[] timeline;

        Action<Playlist, Timeline> PatternAction;

        public void defineAction(Action<Playlist, Timeline> action)
        {
            PatternAction = action;
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

        public override string ToString()
        {
            return type == PatternType.None ? prefab.name : prefab.name + type.ToString();
        }

        public IEnumerator Run()
        {
            float delay = 0;
            int i = 0, j = 0;

            for (; i < timeline.Length; i++)
            {
                if (i == 0)
                    delay = timeline[i].startAt;
                else
                    delay = timeline[i].startAt - timeline[i - 1].startAt + ((j - 1) * timeline[i - 1].detail.repeatDelayTime);
                yield return new WaitForSeconds(delay);
                
                j = 0;
                // it loops at least once
                while (j < timeline[i].detail.repeatNo + 1)
                {
                    PatternAction(this, timeline[i]);
                    if (timeline[i].detail.repeatNo <= ++j) break;
                    yield return new WaitForSeconds(timeline[i].detail.repeatDelayTime);
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
    }

    [AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class PlaylistElementNameAttribute : PropertyAttribute
    {
        public PlaylistElementNameAttribute() { }
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class TimelineElementTitleAttribute : PropertyAttribute
    {
        public TimelineElementTitleAttribute() { }
    }
}