using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace World_2
{
    public class Part_1 : MonoBehaviour
    {
        [SerializeField] AudioClip BGM;
        [PatternArrayElementTitle()]
        [SerializeField] Pattern[] patternList;

        AudioSource musicManager;
        Transform patternManager;
        float startDelay = 1f;

        void OnEnable()
        {
            patternManager = transform.parent;
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
            musicManager.clip = BGM;
            runTimeline();
        }

        private void runTimeline()
        {
        }

        private void runPattern_1(PatternDetail detail)
        {
            GameObject o = Instantiate(patternList[0].prefab);
            o.transform.SetParent(patternManager);
            o.GetComponent<Pattern_1>().setDetailType(detail);
            o.SetActive(true);
        }

        private void runPattern_2()
        {
            GameObject o = Instantiate(patternList[1].prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_3()
        {
            GameObject o = Instantiate(patternList[2].prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_5()
        {
            GameObject o = Instantiate(patternList[3].prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_6()
        {
            GameObject o = Instantiate(patternList[4].prefab);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }
    }
}