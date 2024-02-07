using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    public class EventManager : MonoBehaviour
    {
        public UnityEvent pauseEvent;

        [HideInInspector]
        public float[] savePointTime;
        public StageEvent stageEvent;
        public PlayerEvent playerEvent;
        public UIEvent uiEvent;

        void Start()
        {
            pauseEvent.Invoke();
        }
    }
}