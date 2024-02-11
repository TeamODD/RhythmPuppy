using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventManagement
{
    [Serializable]
    public struct PlayerEvent
    {
        [Header("대시 이벤트")]
        public UnityEvent onDash;
        [Header("투사체(뼈다귀) 발사 이벤트")]
        public UnityEvent onShoot;
        [Header("투사체(뼈다귀) 발사취소 이벤트")]
        public UnityEvent onShootCancel;
        [Header("투사체(뼈다귀) 위치로 순간이동 이벤트")]
        public UnityEvent onTeleport;
        [Header("피격 시 이벤트")]
        public UnityEvent onAttacked;
        [Header("사망 이벤트")]
        public UnityEvent onDeath;
        [Header("부활 이벤트")]
        public UnityEvent onRevive;
        [Header("경고 표식 활성화 이벤트")]
        public UnityEvent onMarkActivated;
        [Header("경고 표식 비활성화 이벤트")]
        public UnityEvent onMarkDeactivated;



        // 기존 소스코드 - delegate 이용 코드
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