using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace Stage_2
{
    public class Pattern_1a : MonoBehaviour
    {
        [SerializeField] GameObject cat;

        PatternManager patternManager;
        List<GameObject> catObjectList;
        WaitForSeconds warnDelay;

        void Awake()
        {
            patternManager = transform.parent.GetComponent<PatternManager>();
            catObjectList = new List<GameObject>();
            warnDelay = new WaitForSeconds(1f);

            StartCoroutine(runPattern());
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
            o.transform.SetParent(patternManager.obstacleManager);
            o.transform.position = new Vector3(x, o.transform.position.y, 0);
            o.SetActive(true);
            catObjectList.Add(o);
            return o;
        }

        private IEnumerator runPattern()
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

        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));
            patternManager.eventManager.warnWithBox(v, new Vector3(300, 1080, 0));
        }
    }
}