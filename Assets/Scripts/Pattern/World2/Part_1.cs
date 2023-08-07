using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimelineManager;

namespace World_2
{
    public class Part_1 : PatternBase
    {

        public void ant()
        {
            foreach (Playlist p in playlist)
            {
                foreach (Timeline t in p.timeline)
                {
                    definePatternAction(p, t);
                }
            }
        }

        /*public UnityEngine.Events.UnityAction bindPatternAction(PatternCode c)
        {
            switch (c)
            {
                case PatternCode.Pattern1_A:
                    return runPattern1_A;

                case PatternCode.Pattern1_B:
                    return runPattern1_B;

                case PatternCode.Pattern1_C:
                    return runPattern1_C;

                case PatternCode.Pattern1_D:
                    return runPattern1_D;

                case PatternCode.Pattern2:
                    return runPattern2;

                case PatternCode.Pattern3:
                    return runPattern3;

                case PatternCode.Pattern5:
                    return runPattern5;

                case PatternCode.Pattern6:
                    return runPattern6;
            }
            Playlist a = playlist[(int)c];
        }*/

        public override void definePatternAction(Playlist p, Timeline t)
        {
            switch (p.prefab.name)
            {
                case "Pattern_1":
                    switch (t.detail.detailType)
                    {
                        case PatternDetail.a:
                            t.defineAction(() => { });
                            break;

                        case PatternDetail.b:
                            t.defineAction(() => { });
                            break;

                        case PatternDetail.c:
                            t.defineAction(() => { });
                            break;

                        case PatternDetail.d:
                            t.defineAction(() => { });
                            break;

                        default:
                            break;
                    }
                    break;

                case "Pattern_2":
                    t.defineAction(() => { });
                    break;

                case "Pattern_3":
                    t.defineAction(() => { });
                    break;

                case "Pattern_5":
                    t.defineAction(() => { });
                    break;

                case "Pattern_6":
                    t.defineAction(() => { });
                    break;

                default:
                    break;
            }
        }

        private void runPattern1_a()
        {
            GameObject o = Instantiate(patternList[0]);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternDetail.a);
            o.SetActive(true);
        }

        private void runPattern1_b()
        {
            GameObject o = Instantiate(patternList[0]);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternDetail.b);
            o.SetActive(true);
        }

        private void runPattern1_c()
        {
            GameObject o = Instantiate(patternList[0]);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternDetail.c);
            o.SetActive(true);
        }

        private void runPattern1_d()
        {
            GameObject o = Instantiate(patternList[0]);
            o.transform.SetParent(transform.parent);
            o.GetComponent<Pattern_1>().setDetailType(PatternDetail.d);
            o.SetActive(true);
        }

        private void runPattern2()
        {
            GameObject o = Instantiate(patternList[1]);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern3()
        {
            GameObject o = Instantiate(patternList[2]);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern5()
        {
            GameObject o = Instantiate(patternList[3]);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }

        private void runPattern6()
        {
            GameObject o = Instantiate(patternList[4]);
            o.transform.SetParent(transform.parent);
            o.SetActive(true);
        }
    }
}