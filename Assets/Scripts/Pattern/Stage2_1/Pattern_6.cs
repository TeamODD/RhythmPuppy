using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    public class Pattern_6 : MonoBehaviour
    {
        [SerializeField] GameObject cat;

        EventManager eventManager;
        List<GameObject> objectList;
        Coroutine coroutine;

        void Awake()
        {
            objectList = new List<GameObject>();
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onDeath.AddListener(deathEvent);

            coroutine = StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            eventManager.onMarkActivated.Invoke();
            yield return new WaitForSeconds(1);
            eventManager.onMarkDeactivated.Invoke();

            float r = UnityEngine.Random.Range(-8f, 8f);
            GameObject catObject = Instantiate(cat);
            catObject.transform.SetParent(transform.parent, false);
            catObject.transform.position = new Vector3(r, 5, 0);
            catObject.SetActive(true);
            objectList.Add(catObject);
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