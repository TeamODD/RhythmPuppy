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
        public List<GameObject> lampList;
        public Transform L_Manhole;
        public Transform R_Manhole;

        EventManager eventManager;

        void Awake() 
        {
            init();
        }

        void init()
        {
            eventManager = FindObjectOfType<EventManager>();
            eventManager.lampOnEvent += lampOnEvent;
            eventManager.lampOffEvent += lampOffEvent;
        }

        private void lampOnEvent()
        {
            for (int i = 0; i < lampList.Count; i++)
            {
                lampList[i].GetComponent<SpriteRenderer>().sprite = lampOn;
            }
        }

        private void lampOffEvent()
        {
            for (int i = 0; i < lampList.Count; i++)
            {
                lampList[i].GetComponent<SpriteRenderer>().sprite = lampOff;
            }
        }

        private IEnumerator shakeManhole(Transform manhole)
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
        }
    }
}