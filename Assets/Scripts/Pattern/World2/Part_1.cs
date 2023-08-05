using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static World_2.Pattern_1;

namespace World_2
{
    public class Part_1 : MonoBehaviour
    {
        [SerializeField] AudioClip BGM;
        [SerializeField] List<GameObject> patternList;

        AudioSource musicManager;
        Transform patternManager;
        float delay = 1f;

        void OnEnable()
        {
            patternManager = transform.parent;
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
            musicManager.clip = BGM;
            Invoke("timeline", delay);
        }

        private void timeline()
        {
            string pattern_1 = "runPattern_1",
                pattern_2 = "runPattern_2",
                pattern_3 = "runPattern_3",
                pattern_5 = "runPattern_5",
                pattern_6 = "runPattern_6";

            Invoke(pattern_2, 0.6f);
            Invoke(pattern_2, 0.6f);
        }

        private void runPattern_1()
        {
            GameObject o = Instantiate(patternList[0]);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_2()
        {
            GameObject o = Instantiate(patternList[1]);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_3()
        {
            GameObject o = Instantiate(patternList[2]);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_5()
        {
            GameObject o = Instantiate(patternList[3]);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }

        private void runPattern_6()
        {
            GameObject o = Instantiate(patternList[4]);
            o.transform.SetParent(patternManager);
            o.SetActive(true);
        }
    }
}