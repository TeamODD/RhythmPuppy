using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static EventManager;

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
            eventManager.isLampOn = true;
        }

        void Update()
        {
            if (eventManager.isLampOn)  sp.sprite = lampOn;
            else  sp.sprite = lampOff;
        }

    }
}