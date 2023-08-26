using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;
using System;

namespace Stage_2
{
    public class Stage_2_1 : MonoBehaviour
    {
        Transform patternManager;

        public Stage_2_1(Transform patternManager)
        {
            this.patternManager = patternManager;
        }
        
        public void setPatternAction(ref PatternPlaylist playlist)
        {
            switch(playlist.type)
            {
                case PatternType.pattern1a:
                    playlist.defineAction(runPattern_1a);
                    return;
                case PatternType.pattern1b:
                    playlist.defineAction(runPattern_1b);
                    return;
                case PatternType.pattern1c:
                    playlist.defineAction(runPattern_1c);
                    return;
                case PatternType.pattern1d:
                    playlist.defineAction(runPattern_1d);
                    return;
                case PatternType.pattern2:
                    playlist.defineAction(runPattern_2);
                    return;
                case PatternType.pattern3:
                    playlist.defineAction(runPattern_3);
                    return;
                case PatternType.pattern5:
                    playlist.defineAction(runPattern_5);
                    return;
                case PatternType.pattern6:
                    playlist.defineAction(runPattern_6);
                    return;

            }
        }

        private void runPattern_1a(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_1b(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_1c(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_1d(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_2(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_3(PatternPlaylist p, Timeline t)
        {
            float duration = 0;
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            if (t.detail.endAt != 0)
                o.GetComponent<Pattern_3>().setDuration(t.startAt, t.detail.endAt);
            else if (t.detail.duration != 0)
                o.GetComponent<Pattern_3>().setDuration(duration);
            o.SetActive(true);
        }

        private void runPattern_5(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_6(PatternPlaylist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }
    }
}