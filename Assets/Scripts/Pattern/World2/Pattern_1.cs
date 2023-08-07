using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace World_2
{
    public class Pattern_1 : MonoBehaviour
    {
        [SerializeField] PatternType detailType;
        [SerializeField] GameObject cat;

        GameObject ObstacleManager;
        List<GameObject> objectList;
        Coroutine coroutine;
        bool isRunning;

        void Start()
        {
            isRunning = false;
            ObstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            objectList = new List<GameObject>();

            runPattern();
        }

        void Update()
        {
            if (isRunning && objectList.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            if (!isRunning) return;
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i].transform.position.y < -1.5f)
                {
                    Destroy(objectList[i]);
                    objectList.RemoveAt(i);
                }
            }
        }

        private void runPattern()
        {
            switch(detailType)
            {
                case PatternType.a:
                    coroutine = StartCoroutine(runPatternA());
                    break;

                case PatternType.b:
                    coroutine = StartCoroutine(runPatternB());
                    break;

                case PatternType.c:
                    coroutine = StartCoroutine(runPatternC());
                    break;

                case PatternType.d:
                    coroutine = StartCoroutine(runPatternD());
                    break;
            }
            isRunning = true;
        }

        public void setDetailType(PatternType detail)
        {
            this.detailType = detail;
        }

        private GameObject createCat()
        {
            float r = Random.Range(-8f, 8f);

            GameObject o = Instantiate(cat) as GameObject;
            o.transform.SetParent(ObstacleManager.transform);
            o.transform.position = new Vector3(r, -1, 0);
            o.SetActive(true);
            return o;
        }

        private IEnumerator runPatternA()
        {
            objectList.Add(createCat());

            for(int i=0; i<objectList.Count; i++)
            {
                if(objectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternB()
        {
            objectList.Add(createCat());
            yield return new WaitForSeconds(0.4f);
            objectList.Add(createCat());
            yield return new WaitForSeconds(0.3f);
            objectList.Add(createCat());

            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternC()
        {
            objectList.Add(createCat());
            yield return new WaitForSeconds(0.5f);
            objectList.Add(createCat());

            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternD()
        {
            objectList.Add(createCat());
            yield return new WaitForSeconds(0.5f);
            objectList.Add(createCat());
            yield return new WaitForSeconds(0.7f);
            objectList.Add(createCat());

            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }
    }
}