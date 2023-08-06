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

        void OnEnable()
        {
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
            musicManager.clip = BGM;
            runTimeline();
        }

        private void runTimeline()
        {
            setPatternArray();

            for (int i = 0; i < patternList.Length; i++)
            {
                StartCoroutine(patternList[i].Run());
            }
            musicManager.Play();
        }
        public void setPatternArray()
        {
            for (int i = 0; i < patternList.Length; i++)
            {
                patternList[i].sortTimeline();

                switch (patternList[i].prefab.name)
                {
                    case "Pattern_1":
                        patternList[i].actionFunc = new Action(runPattern_1);
                        break;

                    case "Pattern_2":
                        patternList[i].actionFunc = new Action(runPattern_2);
                        break;

                    case "Pattern_3":
                        patternList[i].actionFunc = new Action(runPattern_3);
                        break;

                    case "Pattern_5":
                        patternList[i].actionFunc = new Action(runPattern_5);
                        break;

                    case "Pattern_6":
                        patternList[i].actionFunc = new Action(runPattern_6);
                        break;

                    default:
                        break;
                }
            }
        }

        private void runPattern_1(Pattern p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(t.detail.detailType);
            o.SetActive(true);
        }

        private void runPattern_2(Pattern p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern_3(Pattern p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern_5(Pattern p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern_6(Pattern p, Timeline t)
        {
            GameObject o = Instantiate(p.prefab);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }
    }
}