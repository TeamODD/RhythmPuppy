using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct StageEvent
    {
        public UnityEvent onGameStart;
        public UnityEvent onGameClear;
        public UnityEvent onPause;
        public UnityEvent onResume;
        public UnityEvent<GameObject, Vector3, Vector3> onWarning;    // new UnityEvent<GameObject warningType, Vector3 pos, Vector3 size>;



        // 기존 소스코드 - delegate 이용 코드
        public delegate void GameStartEvent();
        public delegate void PauseEvent();
        public delegate void ResumeEvent();
        public delegate void ClearEvent();
        public delegate void RewindEvent();
        public delegate void WarnWithBox(Vector3 pos, Vector3 size);

        [HideInInspector] public bool onClear;
        public GameStartEvent gameStartEvent;
        public PauseEvent pauseEvent;
        public ResumeEvent resumeEvent;
        public ClearEvent clearEvent;
        public RewindEvent rewindEvent;
        public WarnWithBox warnWithBox;
    }
}