using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventManagement
{
    public class EventManager : MonoBehaviour
    {
        [HideInInspector]
        public float[] savePointTime;

        public StageEvent stageEvent;
        public PlayerEvent playerEvent;
        public UIEvent uiEvent;
    }
}