using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UIManagement;

namespace EventManagement
{
    public class EventManager : MonoBehaviour
    {
        /* [Header("Player 관련 글로벌 이벤트")]
        public PlayerEvent playerEvent;
        [Header("Stage 관련 글로벌 이벤트")]
        public StageEvent stageEvent;
        [Header("UI 관련 글로벌 이벤트")]
        public UIEvent uiEvent; */

        [Header("Player 관련 글로벌 이벤트")]
        [Tooltip("대쉬")]
        public UnityEvent onDash;
        [Tooltip("투사체(뼈다귀) 발사")]
        public UnityEvent onShoot;
        [Tooltip("투사체(뼈다귀) 발사 취소")]
        public UnityEvent onShootCancel;
        [Tooltip("투사체(뼈다귀) 위치로 순간이동")]
        public UnityEvent onTeleport;
        [Tooltip("피격 이벤트")]
        public UnityEvent onAttacked;
        [Tooltip("사망 이벤트")]
        public UnityEvent onDeath;
        [Tooltip("부활 이벤트")]
        public UnityEvent onRevive;
        [Tooltip("표식 활성화")]
        public UnityEvent onMarkActivated;
        [Tooltip("표식 비활성화")]
        public UnityEvent onMarkDeactivated;

        [Space(20)]

        [Header("Stage 관련 글로벌 이벤트")]
        [Tooltip("게임시작")]
        public UnityEvent onGameStart;
        [Tooltip("게임 클리어")]
        public UnityEvent onGameClear;
        [Tooltip("게임 일시정지")]
        public UnityEvent onPause;
        [Tooltip("게임재개")]
        public UnityEvent onResume;
        [Tooltip("패턴 시간 되감기 (사망/부활 후)")]
        public UnityEvent onRewind;
        [Tooltip("패턴 경고등")]
        public UnityEvent<WarningType, Vector3, Vector3, Vector3> onWarning;
        public bool isGameCleared { get; set; }

        [Space(20)]

        [Header("UI 관련 글로벌 이벤트")]
        [Tooltip("암전 패턴 활성화")]
        public UnityEvent enableDarkening;
        [Tooltip("암전 패턴 비활성화")]
        public UnityEvent disableDarkening;
        [Tooltip("클리어 조명 활성화")]
        public UnityEvent enableClearSpotlighting;
        [Tooltip("클리어 조명 비활성화")]
        public UnityEvent disableClearSpotlighting;
        [Tooltip("페이드-인 이벤트")]
        public UnityEvent onFadeIn;
        [Tooltip("페이드-아웃 이벤트")]
        public UnityEvent onFadeOut;
        [Tooltip("해상도 변경 이벤트")]
        public UnityEvent onChangingResolution;


        // private area
        [HideInInspector]
        public float[] savePointTime;

        void Awake()
        {
            // Init Player Events
            onDash = new UnityEvent();
            onShoot = new UnityEvent();
            onShootCancel = new UnityEvent();
            onTeleport = new UnityEvent();
            onAttacked = new UnityEvent();
            onDeath = new UnityEvent();
            onRevive = new UnityEvent();
            onMarkActivated = new UnityEvent();
            onMarkDeactivated = new UnityEvent();
            // Init Stage Events
            onGameStart = new UnityEvent();
            onGameClear = new UnityEvent();
            onPause = new UnityEvent();
            onResume = new UnityEvent();
            onRewind = new UnityEvent();
            onWarning = new UnityEvent<WarningType, Vector3, Vector3, Vector3>();
            isGameCleared = false;
            // Init UI Events
            disableDarkening = new UnityEvent();
            enableClearSpotlighting = new UnityEvent();
            disableClearSpotlighting = new UnityEvent();
            onFadeIn = new UnityEvent();
            onFadeOut = new UnityEvent();
            onChangingResolution = new UnityEvent();
        }
    }
}