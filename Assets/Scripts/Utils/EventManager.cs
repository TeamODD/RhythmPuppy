using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[CreateAssetMenu(menuName="Event Manager")]*/
public class EventManager : MonoBehaviour
{
    public enum BackgroundSpeedType
    {
        Stop,
        Slow,
        Normal,
        Fast
    }

    public struct PlayerEvent
    {
        public delegate void MarkActivationEvent();
        public delegate void MarkInactivationEvent();
        public delegate void DashEvent();
        public delegate void ShootEvent();
        public delegate void TeleportEvent();
        public delegate void ShootCancelEvent();

        public MarkActivationEvent markActivationEvent;
        public MarkInactivationEvent markInactivationEvent;
        public DashEvent dashEvent;
        public ShootEvent shootEvent;
        public TeleportEvent teleportEvent;
        public ShootCancelEvent shootCancelEvent;
    }

    public delegate void GameStartEvent();
    public delegate void ClearEvent();
    public delegate void PlayerHitEvent();
    public delegate void DeathEvent();
    public delegate void RewindEvent();
    public delegate void ReviveEvent();
    public delegate void LampOnEvent();
    public delegate void LampOffEvent();
    public delegate void FadeInEvent();
    public delegate void FadeOutEvent();
    public delegate void WarnWithBox(Vector3 pos, Vector3 size);

    [HideInInspector]
    public float[] savePointTime;
    public GameStartEvent gameStartEvent;
    public ClearEvent clearEvent;
    public PlayerHitEvent playerHitEvent;
    public DeathEvent deathEvent;
    public RewindEvent rewindEvent;
    public ReviveEvent reviveEvent;
    public bool isLampOn;
    public FadeInEvent fadeInEvent;
    public FadeOutEvent fadeOutEvent;
    public WarnWithBox warnWithBox;

    public PlayerEvent playerEvent;
}
