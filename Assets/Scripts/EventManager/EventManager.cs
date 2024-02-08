using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    public class EventManager : MonoBehaviour
    {
        public StageEvent stageEvent;
        public PlayerEvent playerEvent;
        public UIEvent uiEvent;

        [HideInInspector]
        public float[] savePointTime;
    }
}