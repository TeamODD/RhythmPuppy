using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UIManagement;

namespace EventManagement
{
    public class EventManager : MonoBehaviour
    {
        /* [Header("Player ���� �۷ι� �̺�Ʈ")]
        public PlayerEvent playerEvent;
        [Header("Stage ���� �۷ι� �̺�Ʈ")]
        public StageEvent stageEvent;
        [Header("UI ���� �۷ι� �̺�Ʈ")]
        public UIEvent uiEvent; */

        [Header("Player ���� �۷ι� �̺�Ʈ")]
        [Tooltip("�뽬")]
        public UnityEvent onDash;
        [Tooltip("����ü(���ٱ�) �߻�")]
        public UnityEvent onShoot;
        [Tooltip("����ü(���ٱ�) �߻� ���")]
        public UnityEvent onShootCancel;
        [Tooltip("����ü(���ٱ�) ��ġ�� �����̵�")]
        public UnityEvent onTeleport;
        [Tooltip("�ǰ� �̺�Ʈ")]
        public UnityEvent onAttacked;
        [Tooltip("��� �̺�Ʈ")]
        public UnityEvent onDeath;
        [Tooltip("��Ȱ �̺�Ʈ")]
        public UnityEvent onRevive;
        [Tooltip("ǥ�� Ȱ��ȭ")]
        public UnityEvent onMarkActivated;
        [Tooltip("ǥ�� ��Ȱ��ȭ")]
        public UnityEvent onMarkDeactivated;

        [Space(20)]

        [Header("Stage ���� �۷ι� �̺�Ʈ")]
        [Tooltip("���ӽ���")]
        public UnityEvent onGameStart;
        [Tooltip("���� Ŭ����")]
        public UnityEvent onGameClear;
        [Tooltip("���� �Ͻ�����")]
        public UnityEvent onPause;
        [Tooltip("�����簳")]
        public UnityEvent onResume;
        [Tooltip("���� �ð� �ǰ��� (���/��Ȱ ��)")]
        public UnityEvent onRewind;
        [Tooltip("���� ����")]
        public UnityEvent<WarningType, Vector3, Vector3, Vector3> onWarning;
        public bool isGameCleared { get; set; }

        [Space(20)]

        [Header("UI ���� �۷ι� �̺�Ʈ")]
        [Tooltip("���� ���� Ȱ��ȭ")]
        public UnityEvent enableDarkening;
        [Tooltip("���� ���� ��Ȱ��ȭ")]
        public UnityEvent disableDarkening;
        [Tooltip("Ŭ���� ���� Ȱ��ȭ")]
        public UnityEvent enableClearSpotlighting;
        [Tooltip("Ŭ���� ���� ��Ȱ��ȭ")]
        public UnityEvent disableClearSpotlighting;
        [Tooltip("���̵�-�� �̺�Ʈ")]
        public UnityEvent onFadeIn;
        [Tooltip("���̵�-�ƿ� �̺�Ʈ")]
        public UnityEvent onFadeOut;
        [Tooltip("�ػ� ���� �̺�Ʈ")]
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