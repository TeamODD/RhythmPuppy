using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagement;
using UIManagement;

namespace Stage_2
{
    public class Pattern_2 : MonoBehaviour
    {
        [SerializeField] GameObject paw;
        [SerializeField] WarningType warningType;

        EventManager eventManager;
        Coroutine coroutine;
        List<GameObject> objectList;

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
            float r = UnityEngine.Random.Range(-8f, 8f);

            warn(r);
            yield return new WaitForSeconds(1);

            GameObject paw = Instantiate(this.paw);
            paw.transform.position = new Vector3(r, paw.transform.position.y, paw.transform.position.z);
            paw.transform.SetParent(transform.parent);
            paw.SetActive(true);
            objectList.Add(paw);
        }

        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, -4.3f - 0.5f));
            eventManager.onWarning.Invoke(warningType, v, new Vector3(200, 500, 0), Vector3.zero);
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