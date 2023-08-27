using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static EventManager;

namespace Stage_2
{
    [Serializable]
    public struct Pattern_3
    {
        public PatternPlaylist patternPlaylist;
        [SerializeField] float[] startDelay;
        [SerializeField] float duration;

        EventManager eventManager;
        Transform parent;
        Camera mainCamera;
        CancellationTokenSource cancel;

        public void init(Transform parent, EventManager eventManager, Camera mainCamera)
        { 
            this.parent = parent;
            this.eventManager = eventManager;
            this.mainCamera = mainCamera;
            this.cancel = new CancellationTokenSource();
            patternPlaylist.init(action);
            patternPlaylist.sortTimeline();

            eventManager.deathEvent += deathEvent;
        }

        public void action(PatternPlaylist patternPlaylist, Timeline timeline)
        {
            if (!timeline.duration.Equals(0))
                setDuration(timeline.duration - startDelay[startDelay.Length - 1]);
            else if (!timeline.endAt.Equals(0))
                setDuration(timeline.startAt, timeline.endAt);

            runPattern(timeline).Forget();
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay[startDelay.Length - 1];
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start - startDelay[startDelay.Length - 1];
        }

        private async UniTask runPattern(Timeline timeline)
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("월드2-1 '패턴3' 프리팹의 startDelay 배열을 검사해주세요.");
            }

            int i = 0;
            await UniTask.Delay(System.TimeSpan.FromSeconds(startDelay[i++]));
            eventManager.isLampOn = false;
            while (i < startDelay.Length)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(startDelay[i] - startDelay[i - 1]));
                i++;
                eventManager.isLampOn = true;
                await UniTask.Delay(System.TimeSpan.FromSeconds(startDelay[i] - startDelay[i - 1]));
                i++;
                eventManager.isLampOn = false;
            }
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
            eventManager.isLampOn = true;
        }

        public void deathEvent()
        {
            cancel.Cancel();
        }
    }
}