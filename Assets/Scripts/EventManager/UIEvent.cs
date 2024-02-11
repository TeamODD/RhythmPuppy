using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct UIEvent
    {
        [Header("�������� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent enableDarkening;
        [Header("�������� ��Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent disableDarkening;
        [Header("Ŭ���� ���� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent enableClearSpotlighting;
        [Header("Ŭ���� ���� ��Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent disableClearSpotlighting;
        [Header("���̵�-�� �̺�Ʈ")]
        public UnityEvent onFadeIn;
        [Header("���̵�-�ƿ� �̺�Ʈ")]
        public UnityEvent onFadeOut;
        [Header("�ػ� ���� �̺�Ʈ")]
        public UnityEvent onChangingResolution;



        // ���� �ҽ��ڵ� - delegate �̿� �ڵ�
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