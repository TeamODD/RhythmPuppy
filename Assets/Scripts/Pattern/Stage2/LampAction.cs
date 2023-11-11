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
            if (eventManager.uiEvent.onBlindEvent)  sp.sprite = lampOff;
            else  sp.sprite = lampOn;
        }
    }
}