using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using UnityEngine;

namespace Stage_2
{
    [Serializable]
    public struct Pattern_6 
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

            eventManager.deathEvent += deathEvent;
        }

        public void action(PatternPlaylist patternPlaylist, Timeline timeline)
        {
            runPattern().Forget();
        }

        private async UniTask runPattern()
        {
            eventManager.playerEvent.markActivationEvent();
            await UniTask.Delay(System.TimeSpan.FromSeconds(1));
            eventManager.playerEvent.markInactivationEvent();

            float r = UnityEngine.Random.Range(-8f, 8f);
            GameObject catObject = MonoBehaviour.Instantiate(cat);
            catObject.transform.SetParent(parent, false);
            catObject.transform.position = new Vector3(r, 5, 0);
            catObject.SetActive(true);
            objectList.Add(catObject);
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