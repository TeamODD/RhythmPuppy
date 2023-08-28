using UnityEngine;

namespace EventManagement
{
    public struct StageEvent
    {
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