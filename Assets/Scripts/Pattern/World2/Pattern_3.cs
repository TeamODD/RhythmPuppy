using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace World_2
{
    public class Pattern_3 : MonoBehaviour
    {
        [SerializeField]
        float[] startDelay;
        [SerializeField] float duration;

        ArtifactManager artfMgr;
        UICanvas uiCanvas;

        void Start()
        {
            PatternManager patternManager = transform.parent.GetComponent<PatternManager>();
            artfMgr = patternManager.artfMgr;
            uiCanvas = patternManager.uiCanvas;

            StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration - startDelay.Sum();
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start;
        }

        private IEnumerator runPattern()
        {
            if (startDelay.Length % 2 == 0)
            {
                throw new Exception("월드2-1 '패턴3' 프리팹의 startDelay 배열을 검사해주세요.");
                yield break;
            }

            int i = 0;
            yield return new WaitForSeconds(startDelay[i++]);
            turnOff();
            while (i < startDelay.Length)
            {
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                turnOn();
                yield return new WaitForSeconds(startDelay[i] - startDelay[i - 1]);
                i++;
                turnOff();
            }
            yield return new WaitForSeconds(duration);
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
            uiCanvas.disableDarkEffect();
        }

        private void turnOff()
        {
            for (int i = 0; i < artfMgr.lampList.Count; i++)
            {
                artfMgr.lampList[i].GetComponent<SpriteRenderer>().sprite = artfMgr.lampOff;
            }
            uiCanvas.enableDarkEffect();
        }
    }
}