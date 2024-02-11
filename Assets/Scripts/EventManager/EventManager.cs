using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [Tooltip("��� �̺�Ʈ")]
        public UnityEvent onDash;
        [Tooltip("����ü(���ٱ�) �߻� �̺�Ʈ")]
        public UnityEvent onShoot;
        [Tooltip("����ü(���ٱ�) �߻���� �̺�Ʈ")]
        public UnityEvent onShootCancel;
        [Tooltip("����ü(���ٱ�) ��ġ�� �����̵� �̺�Ʈ")]
        public UnityEvent onTeleport;
        [Tooltip("�ǰ� �� �̺�Ʈ")]
        public UnityEvent onAttacked;
        [Tooltip("��� �̺�Ʈ")]
        public UnityEvent onDeath;
        [Tooltip("��Ȱ �̺�Ʈ")]
        public UnityEvent onRevive;
        [Tooltip("��� ǥ�� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent onMarkActivated;
        [Tooltip("��� ǥ�� ��Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent onMarkDeactivated;

        [Space(20)]

        [Header("Stage ���� �۷ι� �̺�Ʈ")]
        [Tooltip("���ӽ��� �̺�Ʈ")]
        public UnityEvent onGameStart;
        [Tooltip("���� Ŭ���� �̺�Ʈ")]
        public UnityEvent onGameClear;
        [Tooltip("���� �Ͻ����� �̺�Ʈ")]
        public UnityEvent onPause;
        [Tooltip("�����簳 �̺�Ʈ")]
        public UnityEvent onResume;
        [Tooltip("��Ȱ �� ���� �ǰ��� �̺�Ʈ")]
        public UnityEvent onRewind;
        [Tooltip("���� ���� ǥ�� �̺�Ʈ")]
        public UnityEvent<GameObject, Vector3, Vector3> onWarning;    // new UnityEvent<GameObject warningType, Vector3 pos, Vector3 size>;
        public bool isGameCleared { get; set; }

        [Space(20)]

        [Header("UI ���� �۷ι� �̺�Ʈ")]
        [Tooltip("�������� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent enableDarkening;
        [Tooltip("�������� ��Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent disableDarkening;
        [Tooltip("Ŭ���� ���� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent enableClearSpotlighting;
        [Tooltip("Ŭ���� ���� ��Ȱ��ȭ �̺�Ʈ")]
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
    }
}