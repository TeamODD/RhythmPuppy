using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[CreateAssetMenu(menuName="Event Manager")]*/
public class EventManager : MonoBehaviour
{
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

    [HideInInspector]
    public float[] savePointTime;
    public delegate void GameStartEvent();
    public delegate void PlayerHitEvent();
    public delegate void DeathEvent();
    public delegate void RewindEvent();
    public delegate void ReviveEvent();
    public delegate void LampOnEvent();
    public delegate void LampOffEvent();
    public delegate void InitSavePointEvent();
    public delegate void WarnWithBox(Vector3 pos, Vector3 size);

    public GameStartEvent gameStartEvent;
    public PlayerHitEvent playerHitEvent;
    public DeathEvent deathEvent;
    public RewindEvent rewindEvent;
    public ReviveEvent reviveEvent;
    public LampOnEvent lampOnEvent;
    public LampOffEvent lampOffEvent;
    public WarnWithBox warnWithBox;

    public PlayerEvent playerEvent;
}
