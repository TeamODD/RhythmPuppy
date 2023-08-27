using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static EventManager;

namespace Stage_2
{
    public class ArtifactManager : MonoBehaviour
    {
        public Sprite lampOn;
        public Sprite lampOff;
        public GameObject lampPrefab;
        public GameObject manholePrefab;

        [HideInInspector]
        public List<GameObject> lampList;
        [HideInInspector]
        public List<GameObject> manholeList;
        EventManager eventManager;
        SpriteRenderer sp;

        void Awake() 
        {
            init();
        }

        void init()
        {
            lampList = new List<GameObject>();
            GameObject o;
            eventManager = FindObjectOfType<EventManager>();
            eventManager.lampOnEvent += lampOnEvent;
            eventManager.lampOffEvent += lampOffEvent;

            o = Instantiate(lampPrefab);
            o.transform.position = new Vector3(-7, 0, 0);
            lampList.Add(o);
            o = Instantiate(lampPrefab);
            o.transform.position = new Vector3(7, 0, 0);
            lampList.Add(o);

            /*o = Instantiate(manholePrefab);
            o.transform.position = new Vector3(-4.5f, 0, 0);
            manholeList.Add(o);
            o = Instantiate(manholePrefab);
            o.transform.position = new Vector3(4.5f, 0, 0);
            manholeList.Add(o);*/
        }

        private void lampOnEvent()
        {
            for (int i = 0; i < lampList.Count; i++)
            {
                if (lampList[i] == null)
                {
                    lampList.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (lampList[i].transform.GetChild(0).TryGetComponent<SpriteRenderer>(out sp))
                    {
                        sp.sprite = lampOn;
                    }
                }
            }
        }

        private void lampOffEvent()
        {
            for (int i = 0; i < lampList.Count; i++)
            {
                if (lampList[i] == null)
                {
                    lampList.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (lampList[i].transform.GetChild(0).TryGetComponent<SpriteRenderer>(out sp))
                    {
                        sp.sprite = lampOff;
                    }
                }
            }
        }

        /*private IEnumerator shakeManhole(Transform manhole)
        {
            float angle = 60f;
            int i = 0, delta = 30;

            if (manhole.Equals(L_Manhole))
            {
                for (; i < angle; i += delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1f);
                for (; 0 < i; i -= delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                for (; -1 * angle < i; i -= delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1f);
                for (; i < 0; i += delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
            }

            manhole.rotation = Quaternion.Euler(0, 0, 0);
            yield break;
        }*/
    }
}