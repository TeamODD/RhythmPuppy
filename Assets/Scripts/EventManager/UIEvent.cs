using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct UIEvent
    {
        public UnityEvent enableDarkening;
        public UnityEvent disableDarkening;
        public UnityEvent enableClearSpotlighting;
        public UnityEvent disableClearSpotlighting;
        public UnityEvent onFadeIn;
        public UnityEvent onFadeOut;
        public UnityEvent onChangingResolution;



        // 기존 소스코드 - delegate 이용 코드
        public delegate void EnableBlindEvent();
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
        public ResolutionChangeEvent resolutionChangeEvent;
    }
}