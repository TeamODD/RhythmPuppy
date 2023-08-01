using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace World_2
{
    public class Pattern_3 : MonoBehaviour
    {
        GameObject globalLight;
        List<GameObject> lightList;
        Image image;
        Color c;
        ArtifactManager artfMgr;

        void OnEnable()
        {
            globalLight = GameObject.Find("GlobalLight");
            image = globalLight.GetComponent<Image>();
            lightList = new List<GameObject>();
            lightList.AddRange(GameObject.FindGameObjectsWithTag("LampLight"));
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
            for (int i = 0; i < lightList.Count; i++)
            {
                lightList[i].SetActive(true);
            }
            for (int i = 0; i < artfMgr.lampList.Count; i++) 
            {
                artfMgr.lampList[i].GetComponent<SpriteRenderer>().sprite = artfMgr.lampOn;
            }
            c = image.color;
            c.a = 0f;
            image.color = c;
        }

        private void turnOff()
        {
            for (int i = 0; i < lightList.Count; i++)
            {
                lightList[i].SetActive(false);
            }
            for (int i = 0; i < artfMgr.lampList.Count; i++)
            {
                artfMgr.lampList[i].GetComponent<SpriteRenderer>().sprite = artfMgr.lampOff;
            }
            c = image.color;
            c.a = 0.7f;
            image.color = c;
        }
    }
}