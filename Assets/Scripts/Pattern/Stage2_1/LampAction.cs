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
        private bool UsedByOutScript = false;

        EventManager eventManager;
        SpriteRenderer sp;

        void Awake() 
        {
            init();
        }

        void init()
        {
            sp = GetComponentInChildren<SpriteRenderer>();
            eventManager = FindObjectOfType<EventManager>();
        }

        void Update()
        {
            if (UsedByOutScript) return;

            if (eventManager.uiEvent.onBlindEvent)  sp.sprite = lampOff;
            else  sp.sprite = lampOn;
        }

        public void LampControl(bool Status)
        {
            //true  : 램프 on
            //false : 램프 off
            switch(Status)
            {
                case false:
                    sp.sprite = lampOff;
                    UsedByOutScript = true;
                    break;
                case true:
                    sp.sprite = lampOn;
                    UsedByOutScript = false;
                    break;
            }
        }
    }
}