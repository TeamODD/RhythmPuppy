using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace World_2
{
    public class Pattern_1 : MonoBehaviour
    {
        [SerializeField] PatternDetail detailType;
        [SerializeField] GameObject cat;

        private GameObject ObstacleManager;
        private List<GameObject> objectList;
        private bool onExit = false;

        void OnEnable()
        {
            ObstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            objectList = new List<GameObject>();

            runPattern();
        }

        void Update()
        {
            if(onExit)
            {
                Destroy(gameObject);
            }
        }

        private void runPattern()
        {
            switch(detailType)
            {
                case PatternDetail.a:
                    runPatternA();
                    break;

                case PatternDetail.b:
                    runPatternB();
                    break;

                case PatternDetail.c:
                    runPatternC();
                    break;

                case PatternDetail.d:
                    runPatternD();
                    break;

                default:
                    onExit = true;
                    break;
            }
        }

        public void setDetailType(PatternDetail detail)
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