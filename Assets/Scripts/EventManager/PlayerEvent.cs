using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct PlayerEvent
    {
        [Header("��� �̺�Ʈ")]
        public UnityEvent onDash;
        [Header("����ü(���ٱ�) �߻� �̺�Ʈ")]
        public UnityEvent onShoot;
        [Header("����ü(���ٱ�) �߻���� �̺�Ʈ")]
        public UnityEvent onShootCancel;
        [Header("����ü(���ٱ�) ��ġ�� �����̵� �̺�Ʈ")]
        public UnityEvent onTeleport;
        [Header("�ǰ� �� �̺�Ʈ")]
        public UnityEvent onAttacked;
        [Header("��� �̺�Ʈ")]
        public UnityEvent onDeath;
        [Header("��Ȱ �̺�Ʈ")]
        public UnityEvent onRevive;
        [Header("��� ǥ�� Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent onMarkActivated;
        [Header("��� ǥ�� ��Ȱ��ȭ �̺�Ʈ")]
        public UnityEvent onMarkDeactivated;



        // ���� �ҽ��ڵ� - delegate �̿� �ڵ�
        /* public delegate void MarkActivationEvent();
        public delegate void MarkInactivationEvent();
        public delegate void DashEvent();
        public delegate void ShootEvent();
        public delegate void TeleportEvent();
        public delegate void ShootCancelEvent();
        public delegate void PlayerHitEvent();
        public delegate void DeathEvent();
        public delegate void ReviveEvent();

        public MarkActivationEvent markActivationEvent;
        public MarkInactivationEvent markInactivationEvent;
        public DashEvent dashEvent;
        public ShootEvent shootEvent;
        public TeleportEvent teleportEvent;
        public ShootCancelEvent shootCancelEvent;
        public PlayerHitEvent playerHitEvent;
        public DeathEvent deathEvent;
        public ReviveEvent reviveEvent; */
    }
}