using System.Collections.Generic;
using Patterns;
using UnityEngine;
using EventManagement;
using System.Collections;

namespace Stage_2
{
    public class Pattern_1a : MonoBehaviour
    {
        public Timeline patternPlaylist;
        public GameObject cat;

        EventManager eventManager;
        List<GameObject> objectList;
        AudioSource audioSource;
        Coroutine coroutine;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            init();
        }

        public void init()
        {
            this.objectList = new List<GameObject>();
            patternPlaylist.init(action);
            patternPlaylist.sortPatternInfo();

            coroutine = StartCoroutine(patternPlaylist.Run(audioSource.time));
        }

        void OnDestroy()
        {
            StopCoroutine(coroutine);
        }

        public bool action(Timeline timeline, PatternInfo patterninfo)
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
            eventManager.stageEvent.warnWithBox(v, new Vector3(300, 1080, 0));
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