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
using static EventManagement.StageEvent;

namespace Stage_2
{
    public class Pattern_5 : MonoBehaviour
    {
        public Timeline patternPlaylist;
        public GameObject ratSwarm;
        public float startDelay;
        public float duration;

        EventManager eventManager;
        Transform parent;
        List<GameObject> objectList;
        AudioSource audioSource;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            this.objectList = new List<GameObject>();
        }

        public bool action(PatternInfo patterninfo)
        {
            try
            {
                if (!patterninfo.duration.Equals(0))
                    setDuration(patterninfo.duration - startDelay);
                else if (!patterninfo.endAt.Equals(0))
                    setDuration(patterninfo.startAt, patterninfo.endAt);

                StartCoroutine(runPattern());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay;
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start - startDelay;
        }

        private IEnumerator runPattern()
        {
            bool r = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            warn(r);

            yield return new WaitForSeconds(1);

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

            yield return new WaitForSeconds(duration);
            Destroy(o);
            objectList.Clear();
        }

        private void warn(bool dir)
        {
            Vector2 pos = new Vector2(0, -3.6f + 0.2f);
            if (dir)
                pos.x = 10f;
            else
                pos.x = -10f;
            pos = Camera.main.WorldToScreenPoint(pos);

            eventManager.stageEvent.warnWithBox(pos, new Vector3(700, 150, 0));
        }

        public void deathEvent()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
        }
    }
}