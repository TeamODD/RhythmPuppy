using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace World_2
{
    public class Pattern_1 : MonoBehaviour
    {
        [SerializeField] GameObject warningBox;
        [SerializeField] GameObject warningArrow;
        [SerializeField] PatternType detailType;
        [SerializeField] GameObject cat;

        GameObject ObstacleManager;
        List<GameObject> catObjectList;
        Coroutine coroutine;
        bool isRunning;

        void Start()
        {
            isRunning = false;
            ObstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            catObjectList = new List<GameObject>();

            runPattern();
        }

        void FixedUpdate()
        {
            if (!isRunning) return;
            if (catObjectList.Count <= 0) Destroy(gameObject);
            for (int i = 0; i < catObjectList.Count; i++)
            {
                if (catObjectList[i] == null)
                {
                    continue;
                }

                if (catObjectList[i].transform.position.y < -4f)
                {
                    Destroy(catObjectList[i]);
                    
                    catObjectList[i] = null;
                }
                else
                { 
                    return;
                }
            }
            Destroy(gameObject);
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
            o.transform.position = new Vector3(r, o.transform.position.y, 0);
            o.SetActive(true);
            return o;
        }

        private IEnumerator runPatternA()
        {
            catObjectList.Add(createCat());

            for(int i=0; i<catObjectList.Count; i++)
            {
                if(catObjectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternB()
        {
            catObjectList.Add(createCat());
            yield return new WaitForSeconds(0.4f);
            catObjectList.Add(createCat());
            yield return new WaitForSeconds(0.3f);
            catObjectList.Add(createCat());

            for (int i = 0; i < catObjectList.Count; i++)
            {
                if (catObjectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternC()
        {
            catObjectList.Add(createCat());
            yield return new WaitForSeconds(0.5f);
            catObjectList.Add(createCat());

            for (int i = 0; i < catObjectList.Count; i++)
            {
                if (catObjectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternD()
        {
            catObjectList.Add(createCat());
            yield return new WaitForSeconds(0.5f);
            catObjectList.Add(createCat());
            yield return new WaitForSeconds(0.7f);
            catObjectList.Add(createCat());

            for (int i = 0; i < catObjectList.Count; i++)
            {
                if (catObjectList[i] != null)
                {
                    yield return new WaitForEndOfFrame();
                    i = -1;
                }
            }
            yield break;
        }
    }
}