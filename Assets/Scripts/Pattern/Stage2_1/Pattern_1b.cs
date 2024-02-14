using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using Unity.VisualScripting;
using UnityEngine;
using EventManagement;
using System.Collections;
using UIManagement;


namespace Stage_2
{
    public class Pattern_1b : MonoBehaviour
    {
        [SerializeField] GameObject cat;
        [SerializeField] WarningType warningType;

        EventManager eventManager;
        List<GameObject> objectList;
        Coroutine coroutine;
        Vector3 warnBoxPos, warnBoxSize;

        void Awake()
        {
            objectList = new List<GameObject>();
            warnBoxPos = Vector3.zero;
            warnBoxSize = new Vector3(200, 700, 0);
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onDeath.AddListener(deathEvent);

            coroutine = StartCoroutine(runPattern());
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
            warnBoxPos.x = x;
            warnBoxPos = Camera.main.WorldToScreenPoint(warnBoxPos);
            warnBoxPos.y = 830;
            eventManager.onWarning.Invoke(warningType, warnBoxPos, warnBoxSize, Vector3.up);
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