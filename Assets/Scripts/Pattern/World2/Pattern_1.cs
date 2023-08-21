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

        PatternManager patternManager;
        Transform obstacleManager;
        List<GameObject> catObjectList;
        GameObject warningBox;
        WaitForSeconds warnDelay;

        void Awake()
        {
            patternManager = transform.parent.GetComponent<PatternManager>();
            obstacleManager = patternManager.obstacleManager;
            catObjectList = new List<GameObject>();
            warningBox = patternManager.warningBox;
            warnDelay = new WaitForSeconds(1f);

            runPattern();
        }

        void Update()
        {
            if (catObjectList.Count <= 0) return;

            for (int i = 0; i < catObjectList.Count; i++)
            {
                if (catObjectList[i] != null) return;
            }
            Destroy(gameObject);
        }

        private void runPattern()
        {
            switch(detailType)
            {
                case PatternType.a:
                    StartCoroutine(runPatternA());
                    break;

                case PatternType.b:
                    StartCoroutine(runPatternB());
                    break;

                case PatternType.c:
                    StartCoroutine(runPatternC());
                    break;

                case PatternType.d:
                    StartCoroutine(runPatternD());
                    break;
            }
        }

        public void setDetailType(PatternType detail)
        {
            this.detailType = detail;
        }

        private IEnumerator createObjects()
        {
            float r = Random.Range(-8f, 8f);

            warn(r);
            yield return warnDelay;
            createCat(r);
        }

        private GameObject createCat(float x)
        {
            GameObject o = Instantiate(cat);
            o.transform.SetParent(obstacleManager);
            o.transform.position = new Vector3(x, o.transform.position.y, 0);
            o.SetActive(true);
            catObjectList.Add(o);
            return o;
        }

        private IEnumerator runPatternA()
        {
            StartCoroutine(createObjects());

            for (int i=0; i<catObjectList.Count; i++)
            {
                if(catObjectList[i] != null)
                {
                    yield return null ;
                    i = -1;
                }
            }
            yield break;
        }

        private IEnumerator runPatternB()
        {
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(createObjects());

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
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(createObjects());

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
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(createObjects());
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(createObjects());

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

        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));

            GameObject o = Instantiate(warningBox);
            o.transform.SetParent(patternManager.overlayCanvas);
            o.transform.position = v;
            o.transform.localScale = new Vector3(300, 1080, 0);
            o.SetActive(true);
        }
    }
}