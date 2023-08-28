using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    [Serializable]
    public struct Pattern_2
    {
        public PatternPlaylist patternPlaylist;
        public GameObject paw;

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
            runPattern().Forget();
        }

        private async UniTask runPattern()
        {
            float r = UnityEngine.Random.Range(-8f, 8f);

            warn(r);
            await UniTask.Delay(System.TimeSpan.FromSeconds(1));

            GameObject paw = MonoBehaviour.Instantiate(this.paw);
            paw.transform.position = new Vector3(r, paw.transform.position.y, paw.transform.position.z);
            paw.transform.SetParent(parent);
            paw.SetActive(true);
            objectList.Add(paw);
        }

        private void warn(float x)
        {
            Vector2 v = mainCamera.WorldToScreenPoint(new Vector2(x, -4.3f - 0.5f));
            eventManager.stageEvent.warnWithBox(v, new Vector3(200, 500, 0));
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