using System;
using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;

namespace TimelineManager
{
    public enum PatternType
    {
        pattern1a,
        pattern1b,
        pattern1c,
        pattern1d,
        pattern2,
        pattern3,
        pattern5,
        pattern6,
    }

    [Serializable, CreateAssetMenu(menuName="Create New Pattern Playlist")]
    public class PatternPlaylist : ScriptableObject
    {
        public GameObject prefab;
        [TimelineElementTitle()]
        public Timeline[] timeline;
        public PatternType type;

        Action<PatternPlaylist, Timeline> PatternAction;

        public void defineAction(Action<PatternPlaylist, Timeline> action)
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

        public IEnumerator Run(float startTime)
        {
            WaitForSeconds delay, repeatDelay;
            float delayTime, repeatDelayTime;
            int i = 0, j = 0, repeat;

            for (; i < timeline.Length; i++)
            {
                repeat = timeline[i].detail.repeatNo;
                delayTime = timeline[i].startAt;
                if (i == 0) delayTime -= startTime;
                else delayTime -= timeline[i - 1].startAt + ((j - 1) * timeline[i - 1].detail.repeatDelayTime);

                if (repeat <= 1)
                {
                    if (timeline[i].startAt < startTime) continue;
                    delay = new WaitForSeconds(delayTime);
                    yield return delay;
                    PatternAction(this, timeline[i]);
                }
                else
                {
                    repeatDelayTime = timeline[i].detail.repeatDelayTime;
                    repeatDelay = new WaitForSeconds(repeatDelayTime);
                    for (j = 0; j < repeat; j++)
                    {
                        if (timeline[i].startAt + j * repeatDelayTime < startTime) continue;
                        delay = new WaitForSeconds(delayTime + j * repeatDelayTime - startTime);
                        if (delay != null)
                        {
                            yield return delay;
                            delay = null;
                        }
                        else
                        {
                            yield return repeatDelay;
                        }
                        PatternAction(this, timeline[i]);
                    }
                }
            }
        }
    }
}