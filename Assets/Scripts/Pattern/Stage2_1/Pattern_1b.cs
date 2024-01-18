using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using EventManagement;
using static EventManagement.StageEvent;
using System.Collections;


namespace Stage_2
{
    public class Pattern_1b : MonoBehaviour
    {
        public GameObject cat;

        EventManager eventManager;
        List<GameObject> objectList;
        AudioSource audioSource;
        Coroutine coroutine;
        PatternInfo patternInfo;

        void Start()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            this.objectList = new List<GameObject>();
            patternInfo = GetComponent<PatternBase>().patternInfo;

            StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(createObjects());
        }

        private IEnumerator createObjects()
        {
            float r = UnityEngine.Random.Range(-8f, 8f);

            warn(r);
            yield return new WaitForSeconds(1);
            createCat(r);
        }

        private void createCat(float x)
        {
            GameObject o = Instantiate(cat);
            o.transform.SetParent(transform.parent);
            o.transform.position = new Vector3(x, o.transform.position.y, 0);
            o.SetActive(true);
            objectList.Add(o);
        }

        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));
            eventManager.stageEvent.warnWithBox(v, new Vector3(300, 1080, 0));
        }

        public void deathEvent()
        {
            StopCoroutine(coroutine);
            for (int i = 0; i < objectList.Count; i++)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
        }
    }
}