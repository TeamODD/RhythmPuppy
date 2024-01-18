/* 미사용 스크립트 (현 PatternList.cs) */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Patterns;
using EventManagement;
using System;

namespace Stage_2
{
    public class Stage_2_1 : MonoBehaviour
    {
#if false
        public GameObject[] patternPrefab;

        AudioSource audioSource;
        EventManager eventManager;
        Transform patternManager;
        GameObject[] patternList;

        public void init()
        {
            eventManager = FindObjectOfType<EventManager>();
            audioSource = FindObjectOfType<AudioSource>();
            patternManager = transform.parent;

            Run();
        }

        public void Run()
        {
            patternList = new GameObject[patternPrefab.Length];
            for (int i = 0; i < patternPrefab.Length; i++)
            {
                patternList[i] = Instantiate(patternPrefab[i]);
                patternList[i].transform.SetParent(transform);
                patternList[i].SetActive(true);
            }
        }

        /*public void deathEvent()
        {
            for (int i = 0; i < patternList.Length; i++)
            {
                Destroy(patternList[i]);
            }
            Destroy(gameObject);
        }*/
#endif
    }
}