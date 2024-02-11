using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct UIEvent
    {
        [Header("암전패턴 활성화 이벤트")]
        public UnityEvent enableDarkening;
        [Header("암전패턴 비활성화 이벤트")]
        public UnityEvent disableDarkening;
        [Header("클리어 조명 활성화 이벤트")]
        public UnityEvent enableClearSpotlighting;
        [Header("클리어 조명 비활성화 이벤트")]
        public UnityEvent disableClearSpotlighting;
        [Header("페이드-인 이벤트")]
        public UnityEvent onFadeIn;
        [Header("페이드-아웃 이벤트")]
        public UnityEvent onFadeOut;
        [Header("해상도 변경 이벤트")]
        public UnityEvent onChangingResolution;



        // 기존 소스코드 - delegate 이용 코드
        /* public delegate void EnableBlindEvent();
        public delegate void DisableBlindEvent();
        public delegate void EnableClearSpotlightEvent();
        public delegate void DisableClearSpotlightEvent();
        public delegate bool GetLampStatus();
        public delegate void FadeInEvent();
        public delegate void FadeOutEvent();
        public delegate void ResolutionChangeEvent();

        [HideInInspector] public bool onBlindEvent;
        public EnableBlindEvent enableBlindEvent;
        public DisableBlindEvent disableBlindEvent;
        public EnableClearSpotlightEvent enableClearSpotlightEvent;
        public DisableClearSpotlightEvent disableClearSpotlightEvent;
        public GetLampStatus getLampStatus;
        public FadeInEvent fadeInEvent;
        public FadeOutEvent fadeOutEvent;
        public ResolutionChangeEvent resolutionChangeEvent; */
    }
}