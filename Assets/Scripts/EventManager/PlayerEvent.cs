using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct PlayerEvent
    {
        public UnityEvent onDash;
        public UnityEvent onShoot;
        public UnityEvent onShootCancel;
        public UnityEvent onTeleport;
        public UnityEvent onAttacked;
        public UnityEvent onMarkActivated;
        public UnityEvent onMarkInactivated;
        public UnityEvent onDeath;
        public UnityEvent onRevive;
        public UnityEvent onRewind;



        // 기존 소스코드 - delegate 이용 코드
        public delegate void MarkActivationEvent();
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
        public ReviveEvent reviveEvent;
    }
}