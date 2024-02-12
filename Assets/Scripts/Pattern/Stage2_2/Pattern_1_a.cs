using System.Collections.Generic;
using Patterns;
using UnityEngine;
using EventManagement;
using System.Collections;
using UIManagement;

namespace Stage_2_2
{
    public class Pattern_1_a : MonoBehaviour
    {
        public GameObject cat;
        public GameObject catWarnBox;

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
        }

        public bool action(PatternInfo patterninfo)
        {
            try
            {
                StartCoroutine(createObjects());
                return true;
            }
            catch
            {
                return false;
            }
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
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));
            eventManager.onWarning.Invoke(WarningType.Box, v, new Vector3(300, 1080, 0), Vector3.zero);
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
