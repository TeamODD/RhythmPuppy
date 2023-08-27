using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Patterns;
using System;
using Cysharp.Threading.Tasks;

namespace Stage_2
{
    [Serializable]
    public struct Stage_2_1
    {
        public AudioClip music;
        public float[] savePointTime;
        public Pattern_1a pattern_1a;
        public Pattern_1b pattern_1b;
        public Pattern_1c pattern_1c;
        public Pattern_1d pattern_1d;
        public Pattern_2 pattern_2;
        public Pattern_3 pattern_3;
        public Pattern_5 pattern_5;
        public Pattern_6 pattern_6;

        EventManager eventManager;
        PatternManager patternManager;

        public void init(PatternManager patternManager, EventManager eventManager)
        {
            this.patternManager = patternManager;
            this.eventManager = eventManager;

            pattern_1a.init(patternManager.transform, eventManager, Camera.main);
            pattern_1b.init(patternManager.transform, eventManager, Camera.main);
            pattern_1c.init(patternManager.transform, eventManager, Camera.main);
            pattern_1d.init(patternManager.transform, eventManager, Camera.main);
            pattern_2.init(patternManager.transform, eventManager, Camera.main);
            pattern_3.init(patternManager.transform, eventManager, Camera.main);
            pattern_5.init(patternManager.transform, eventManager, Camera.main);
            pattern_6.init(patternManager.transform, eventManager, Camera.main);
        }

        public void Run(float startTime, out List<Coroutine> coroutineArray)
        {
            coroutineArray = new List<Coroutine>();
            coroutineArray.Add(patternManager.StartCoroutine(pattern_1a.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_1b.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_1c.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_1d.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_2.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_3.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_5.patternPlaylist.Run(startTime)));
            coroutineArray.Add(patternManager.StartCoroutine(pattern_6.patternPlaylist.Run(startTime)));
        }
    }
}