using UnityEngine;

namespace EventManagement
{
    public struct StageEvent
    {
        public delegate void GameStartEvent();
        public delegate void ClearEvent();
        public delegate void RewindEvent();
        public delegate void WarnWithBox(Vector3 pos, Vector3 size);

        public bool onBlindEvent;
        public GameStartEvent gameStartEvent;
        public ClearEvent clearEvent;
        public RewindEvent rewindEvent;
        public WarnWithBox warnWithBox;
    }
}