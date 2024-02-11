using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct StageEvent
    {
        [Header("게임시작 이벤트")]
        public UnityEvent onGameStart;
        [Header("게임 클리어 이벤트")]
        public UnityEvent onGameClear;
        [Header("게임 일시정지 이벤트")]
        public UnityEvent onPause;
        [Header("게임재개 이벤트")]
        public UnityEvent onResume;
        [Header("부활 후 패턴 되감기 이벤트")]
        public UnityEvent onRewind;
        [Header("패턴 경고등 표시 이벤트")]
        public UnityEvent<GameObject, Vector3, Vector3> onWarning;    // new UnityEvent<GameObject warningType, Vector3 pos, Vector3 size>;

        public bool isGameCleared { get; set; }


        // 기존 소스코드 - delegate 이용 코드
        /* public delegate void GameStartEvent();
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
        public WarnWithBox warnWithBox; */
    }
}