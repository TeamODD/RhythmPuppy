using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace World_2
{
    public class Pattern_3 : MonoBehaviour
    {
        ArtifactManager artfMgr;

        void OnEnable()
        {
            artfMgr = transform.parent.GetComponent<PatternManager>().artfMgr;

            StartCoroutine(activate());
        }

        private IEnumerator activate()
        {
            turnOff();
            yield return new WaitForSeconds(0.5f);
            turnOn();
            yield return new WaitForSeconds(0.2f);
            turnOff();
            yield return new WaitForSeconds(0.2f);
            turnOn();
            yield return new WaitForSeconds(0.1f);
            turnOff();
            yield return new WaitForSeconds(0.4f);
            turnOn();
            yield return new WaitForSeconds(0.1f);
            turnOff();

            yield return new WaitForSeconds(10f);
            turnOn();

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