using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;
using System;

namespace World_2
{
    public class Part_1 : PatternBase
    {
        public override void bindPatternAction()
        {
            bind.Add("Pattern_1a", runPattern_1a);
            bind.Add("Pattern_1b", runPattern_1b);
            bind.Add("Pattern_1c", runPattern_1c);
            bind.Add("Pattern_1d", runPattern_1d);
            bind.Add("Pattern_2", runPattern_2);
            bind.Add("Pattern_3", runPattern_3);
            bind.Add("Pattern_5", runPattern_5);
            bind.Add("Pattern_6", runPattern_6);
        }

        private void runPattern_1a(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternType.a);
            o.SetActive(true);
        }

        private void runPattern_1b(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternType.b);
            o.SetActive(true);
        }

        private void runPattern_1c(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternType.c);
            o.SetActive(true);
        }

        private void runPattern_1d(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternType.d);
            o.SetActive(true);
        }

        private void runPattern_2(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern_3(Playlist p, Timeline t)
        {
            float duration = 0;
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            if (t.detail.endAt != 0)
                o.GetComponent<Pattern_3>().setDuration(t.startAt, t.detail.endAt);
            else if (t.detail.duration != 0)
                o.GetComponent<Pattern_3>().setDuration(duration);
            o.SetActive(true);
        }

        private void runPattern_5(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern_6(Playlist p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }
    }
}