using System.Collections.Generic;
using Patterns;
using UnityEngine;
using EventManagement;
using System.Collections;

namespace Stage_2
{
    public class Pattern_1a : MonoBehaviour
    {
        public GameObject cat;
        public GameObject warnBox;

        EventManager eventManager;
        List<GameObject> objectList;
        AudioSource audioSource;
        Coroutine coroutine;
        PatternInfo patternInfo;
        Vector3 warnBoxPos, warnBoxSize;

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            objectList = new List<GameObject>();
            patternInfo = GetComponent<PatternBase>().patternInfo;
            warnBoxPos = new Vector3(0, 0, 0);
            warnBoxSize = new Vector3(200, 700, 0);

            eventManager.onDeath.AddListener(deathEvent);

            coroutine = StartCoroutine(createObjects());
        }

        private IEnumerator createObjects()
        {
            float r = Random.Range(-8f, 8f);

            if (gameObject != null) warn(r);
            yield return new WaitForSeconds(1);
            if (gameObject != null) createCat(r);
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
            warnBoxPos.x = x;
            warnBoxPos = Camera.main.WorldToScreenPoint(warnBoxPos);
            warnBoxPos.y = 830;
            eventManager.onWarning.Invoke(warnBox, warnBoxPos, warnBoxSize);
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