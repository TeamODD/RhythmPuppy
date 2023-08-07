using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace World_2
{
    public class Pattern_3 : MonoBehaviour
    {

        [SerializeField]
        float[] startDelay;

        ArtifactManager artfMgr;
        float duration;

        void Start()
        {
            artfMgr = transform.parent.GetComponent<PatternManager>().artfMgr;

            StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration;
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start + startDelay.Sum();
        }

        private IEnumerator runPattern()
        {
            int i = 0;

            turnOff();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOn();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOff();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOn();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOff();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOn();
            yield return new WaitForSeconds(startDelay[i++]);
            turnOff();

            yield return new WaitForSeconds(duration - startDelay.Sum());
            turnOn();

            yield return new WaitForSeconds(1);
            Destroy(gameObject);
            yield break;
        }

        private void turnOn()
        {
            for (int i = 0; i < artfMgr.lampList.Count; i++) 
            {
                artfMgr.lampList[i].GetComponent<SpriteRenderer>().sprite = artfMgr.lampOn;
            }
        }

        private void turnOff()
        {
            for (int i = 0; i < artfMgr.lampList.Count; i++)
            {
                artfMgr.lampList[i].GetComponent<SpriteRenderer>().sprite = artfMgr.lampOff;
            }
        }
    }
}