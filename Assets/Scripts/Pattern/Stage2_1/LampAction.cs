using EventManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stage_2
{
    public class LampAction : MonoBehaviour
    {
        public Sprite lampOn;
        public Sprite lampOff;
        /*private bool UsedByOutScript = false;*/

        EventManager eventManager;
        SpriteRenderer sp;

        void Awake() 
        {
            sp = GetComponentInChildren<SpriteRenderer>();
            eventManager = FindObjectOfType<EventManager>();

            eventManager.uiEvent.enableBlindEvent += enableBlindEvent;
            eventManager.uiEvent.disableBlindEvent += disableBlindEvent;
        }

        /*void Update()
        {
            if (UsedByOutScript) return;

            if (eventManager.uiEvent.onBlindEvent) sp.sprite = lampOff;
            else sp.sprite = lampOn;
        }*/

        public void LampControl(bool Status)
        {
            //true  : 램프 on
            //false : 램프 off
            switch(Status)
            {
                case false:
                    sp.sprite = lampOff;
                    /*UsedByOutScript = true;*/
                    break;
                case true:
                    sp.sprite = lampOn;
                    /*UsedByOutScript = false;*/
                    break;
            }
        }

        private void enableBlindEvent()
        {
            sp.sprite = lampOff;
        }

        private void disableBlindEvent()
        {
            sp.sprite = lampOn;
        }
    }
}