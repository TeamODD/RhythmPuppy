using Cysharp.Threading.Tasks;
using Obstacles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    [Serializable]
    public struct Pattern_5 
    {
        public PatternPlaylist patternPlaylist;
        public GameObject ratSwarm;
        public float startDelay;
        public float duration;

        EventManager eventManager;
        Transform parent;
        Camera mainCamera;
        CancellationTokenSource cancel;
        List<GameObject> objectList;

        public void init(Transform parent, EventManager eventManager, Camera mainCamera)
        {
            this.parent = parent;
            this.eventManager = eventManager;
            this.mainCamera = mainCamera;
            this.cancel = new CancellationTokenSource();
            this.objectList = new List<GameObject>();
            patternPlaylist.init(action);
            patternPlaylist.sortTimeline();

            eventManager.playerEvent.deathEvent += deathEvent;
        }

        public void action(PatternPlaylist patternPlaylist, Timeline timeline)
        {
            if (!timeline.duration.Equals(0))
                setDuration(timeline.duration - startDelay);
            else if (!timeline.endAt.Equals(0))
                setDuration(timeline.startAt, timeline.endAt);

            runPattern().Forget();
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay;
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start - startDelay;
        }

        private async UniTask runPattern()
        {
            bool r = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            warn(r);

            await UniTask.Delay(System.TimeSpan.FromSeconds(1));

            GameObject o = MonoBehaviour.Instantiate(ratSwarm);
            o.transform.SetParent(parent);
            o.GetComponent<RatSwarm>().setCooltime(startDelay);
            Vector2 pos = new Vector2(0, o.transform.position.y);
            // set spawn position of ratSwarm
            if (r)
            {
                // Right
                pos.x = 10f;
            }
            else
            {
                // Left
                pos.x = -10f;
                o.GetComponent<SpriteRenderer>().flipX = true;
            }
            o.transform.position = pos;
            o.SetActive(true);
            objectList.Add(o);

            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
            MonoBehaviour.Destroy(o);
            objectList.Clear();
        }

        private void warn(bool dir)
        {
            Vector2 pos = new Vector2(0, -3.6f + 0.2f);
            if (dir)
                pos.x = 10f;
            else
                pos.x = -10f;
            pos = mainCamera.WorldToScreenPoint(pos);

            eventManager.stageEvent.warnWithBox(pos, new Vector3(700, 150, 0));
        }

        public void deathEvent()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                MonoBehaviour.Destroy(objectList[i]);
            }
            objectList.Clear();
            cancel.Cancel();
        }
    }
}