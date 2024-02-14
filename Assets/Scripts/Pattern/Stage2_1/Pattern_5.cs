using Obstacles;
using System;
using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;
using EventManagement;
using UIManagement;

namespace Stage_2
{
    public class Pattern_5 : MonoBehaviour
    {
        [SerializeField] GameObject ratSwarm;
        [SerializeField] WarningType warningType;
        public float startDelay;
        public float duration;

        EventManager eventManager;
        Transform parent;
        List<GameObject> objectList;
        Coroutine coroutine;
        PatternInfo patternInfo;
        Vector3 warnBoxPos, warnBoxSize;

        void Awake()
        {
            objectList = new List<GameObject>();
            patternInfo = GetComponent<PatternBase>().patternInfo;
            warnBoxSize = new Vector3(300, 600, 0);

            if (!patternInfo.duration.Equals(0))
                setDuration(patternInfo.duration - startDelay);
            else if (!patternInfo.endAt.Equals(0))
                setDuration(patternInfo.startAt, patternInfo.endAt);
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onDeath.AddListener(deathEvent);

            coroutine = StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay;
        }

        public void setDuration(float start, float end)
        {
            duration = end - start - startDelay;
        }

        private IEnumerator runPattern()
        {
            bool r = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            warn(r);

            yield return new WaitForSeconds(1);

            GameObject o = Instantiate(ratSwarm);
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

        private void warn(bool isRight)
        {
            if (isRight)
                warnBoxPos = Camera.main.WorldToScreenPoint(new Vector3(8.5f, -3f, 0));
            else
                warnBoxPos = Camera.main.WorldToScreenPoint(new Vector3(-8.5f, -3f, 0));
            eventManager.onWarning.Invoke(warningType, warnBoxPos, warnBoxSize, Vector3.zero);
        }

        public void deathEvent()
        {
            StopCoroutine(coroutine);
            for (int i = 0; i < objectList.Count; i++)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
            Destroy(gameObject);
        }
    }
}