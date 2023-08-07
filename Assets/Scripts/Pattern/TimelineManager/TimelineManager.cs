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
            if(PatternAction == null)
            {
                Debug.Log("Current Scene Name : " + SceneManager.GetActiveScene().name);
                string name = type == PatternType.None ? prefab.name : prefab.name + "-" + type.ToString();
                string log = "Fatal Error : can't execute " + name + " because PattenAction is null.";
                Debug.Log(log);
                Application.Quit();
            }

            float sum = 0f;

            for (int i = 0; i < timeline.Length; i++) 
            {
                float startAt = timeline[i].startAt;
                yield return new WaitForSeconds(startAt - sum);
                sum += startAt;

                if (timeline[i].detail.repeatNo == 0)
                {
                    PatternAction(this, timeline[i]);
                }
                else
                {
                    int n = timeline[i].detail.repeatNo;
                    float delay = timeline[i].detail.repeatDelayTime;
                    for (int j = 0; j < n; j++) 
                    {
                        PatternAction(this, timeline[i]);
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