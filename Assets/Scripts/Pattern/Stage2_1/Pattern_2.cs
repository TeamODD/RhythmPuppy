using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using EventManagement;
using static EventManagement.StageEvent;

namespace Stage_2
{
    public class Pattern_2 : MonoBehaviour
    {
        public GameObject paw;

        EventManager eventManager;
        AudioSource audioSource;
        Coroutine coroutine;
        List<GameObject> objectList;
        PatternInfo patternInfo;

        void Start()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            this.objectList = new List<GameObject>();
            patternInfo = GetComponent<PatternBase>().patternInfo;

            eventManager.playerEvent.deathEvent += deathEvent;

            coroutine = StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            float r = UnityEngine.Random.Range(-8f, 8f);

            warn(r);
            yield return new WaitForSeconds(1);

            GameObject paw = MonoBehaviour.Instantiate(this.paw);
            paw.transform.position = new Vector3(r, paw.transform.position.y, paw.transform.position.z);
            paw.transform.SetParent(transform.parent);
            paw.SetActive(true);
            objectList.Add(paw);
        }

        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, -4.3f - 0.5f));
            eventManager.stageEvent.warnWithBox(v, new Vector3(200, 500, 0));
        }

        public void deathEvent()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
            Destroy(gameObject);
        }
    }
}