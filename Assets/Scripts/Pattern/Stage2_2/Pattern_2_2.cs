using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Patterns;
using EventManagement;
using System;

namespace Stage_2_2
{
    public class Pattern_2_2 : MonoBehaviour
    {
        public GameObject[] patternPrefab;

        AudioSource audioSource;
        EventManager eventManager;
        Transform patternManager;
        GameObject[] patternList;

        void Awake()
        {
            audioSource = FindObjectOfType<AudioSource>();
            patternManager = transform.parent;
        }

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onDeath.AddListener(deathEvent);

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

        public void deathEvent()
        {
            for (int i = 0; i < patternList.Length; i++)
            {
                Destroy(patternList[i]);
            }
            Destroy(gameObject);
        }
    }
}