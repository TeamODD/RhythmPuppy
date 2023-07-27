using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World_2
{
    public class Pattern_1 : MonoBehaviour
    {
        [System.Serializable]
        public struct Patterns
        {
            public bool pattern_Random;
            public bool patternA;
            public bool patternB;
            public bool patternC;
            public bool patternD;
        }

        [SerializeField] Patterns patterns;
        [SerializeField] GameObject cat;

        private GameObject ObstacleManager;
        private List<GameObject> objectList;
        private bool onExit = false;

        void OnEnable()
        {
            ObstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            objectList = new List<GameObject>();

            if (patterns.pattern_Random)
            {
                int r = Random.Range(0, 4);
                switch (r)
                {
                    case 0:
                        StartCoroutine(runPatternA());
                        break;
                    case 1:
                        StartCoroutine(runPatternB());
                        break;
                    case 2:
                        StartCoroutine(runPatternC());
                        break;
                    case 3:
                        StartCoroutine(runPatternD());
                        break;
                }
            }
            else if (patterns.patternA)
                StartCoroutine(runPatternA());
            else if (patterns.patternB)
                StartCoroutine(runPatternB());
            else if (patterns.patternC)
                StartCoroutine(runPatternC());
            else if (patterns.patternD)
                StartCoroutine(runPatternD());
            else
            {
                int r = Random.Range(0, 4);
                switch (r)
                {
                    case 0:
                        StartCoroutine(runPatternA());
                        break;
                    case 1:
                        StartCoroutine(runPatternB());
                        break;
                    case 2:
                        StartCoroutine(runPatternC());
                        break;
                    case 3:
                        StartCoroutine(runPatternD());
                        break;
                }
            }
        }

        void Update()
        {
            if(onExit)
            {
                Destroy(gameObject);
            }
        }

        public GameObject createCat()
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

            onExit = true;
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

            onExit = true;
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

            onExit = true;
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

            onExit = true;
            yield break;
        }


    }
}