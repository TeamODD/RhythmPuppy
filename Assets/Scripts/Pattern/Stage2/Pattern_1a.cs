using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    [Serializable]
    public struct Pattern_1a
    {
        public PatternPlaylist patternPlaylist;
        public GameObject cat;

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
            createObjects().Forget();
        }
        private async UniTask createObjects()
        {
            float r = UnityEngine.Random.Range(-8f, 8f);

            warn(r);
            await UniTask.Delay(System.TimeSpan.FromSeconds(1));
            createCat(r);
        }

        private void createCat(float x)
        {
            GameObject o = MonoBehaviour.Instantiate(cat);
            o.transform.SetParent(parent);
            o.transform.position = new Vector3(x, o.transform.position.y, 0);
            o.SetActive(true);
            objectList.Add(o);
        }

        private void warn(float x)
        {
            Vector2 v = mainCamera.WorldToScreenPoint(new Vector2(x, 0));
            eventManager.stageEvent.warnWithBox(v, new Vector3(300, 1080, 0));
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