using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [Tooltip("대시 이벤트")]
        public UnityEvent onDash;
        [Tooltip("투사체(뼈다귀) 발사 이벤트")]
        public UnityEvent onShoot;
        [Tooltip("투사체(뼈다귀) 발사취소 이벤트")]
        public UnityEvent onShootCancel;
        [Tooltip("투사체(뼈다귀) 위치로 순간이동 이벤트")]
        public UnityEvent onTeleport;
        [Tooltip("피격 시 이벤트")]
        public UnityEvent onAttacked;
        [Tooltip("사망 이벤트")]
        public UnityEvent onDeath;
        [Tooltip("부활 이벤트")]
        public UnityEvent onRevive;
        [Tooltip("경고 표식 활성화 이벤트")]
        public UnityEvent onMarkActivated;
        [Tooltip("경고 표식 비활성화 이벤트")]
        public UnityEvent onMarkDeactivated;

        [Space(20)]

        [Header("Stage 관련 글로벌 이벤트")]
        [Tooltip("게임시작 이벤트")]
        public UnityEvent onGameStart;
        [Tooltip("게임 클리어 이벤트")]
        public UnityEvent onGameClear;
        [Tooltip("게임 일시정지 이벤트")]
        public UnityEvent onPause;
        [Tooltip("게임재개 이벤트")]
        public UnityEvent onResume;
        [Tooltip("부활 후 패턴 되감기 이벤트")]
        public UnityEvent onRewind;
        [Tooltip("패턴 경고등 표시 이벤트")]
        public UnityEvent<GameObject, Vector3, Vector3> onWarning;    // new UnityEvent<GameObject warningType, Vector3 pos, Vector3 size>;
        public bool isGameCleared { get; set; }

        [Space(20)]

        [Header("UI 관련 글로벌 이벤트")]
        [Tooltip("암전패턴 활성화 이벤트")]
        public UnityEvent enableDarkening;
        [Tooltip("암전패턴 비활성화 이벤트")]
        public UnityEvent disableDarkening;
        [Tooltip("클리어 조명 활성화 이벤트")]
        public UnityEvent enableClearSpotlighting;
        [Tooltip("클리어 조명 비활성화 이벤트")]
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
    }
}