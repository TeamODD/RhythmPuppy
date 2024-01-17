using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Patterns;
using UnityEngine;
using EventManagement;

namespace Stage_2
{
    public class Pattern_6 : MonoBehaviour
    {
        public Timeline patternPlaylist;
        public GameObject cat;

        EventManager eventManager;
        AudioSource audioSource;
        List<GameObject> objectList;

        void Start()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            this.objectList = new List<GameObject>();
        }

        public bool action(PatternInfo patterninfo)
        {
            try
            {
                StartCoroutine(runPattern());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IEnumerator runPattern()
        {
            eventManager.playerEvent.markActivationEvent();
            yield return new WaitForSeconds(1);
            eventManager.playerEvent.markInactivationEvent();

            float r = UnityEngine.Random.Range(-8f, 8f);
            GameObject catObject = Instantiate(cat);
            catObject.transform.SetParent(transform.parent, false);
            catObject.transform.position = new Vector3(r, 5, 0);
            catObject.SetActive(true);
            objectList.Add(catObject);
        }

        public void deathEvent()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
        }
    }
}