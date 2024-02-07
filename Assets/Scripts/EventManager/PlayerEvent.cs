
namespace EventManagement
{
    public struct PlayerEvent
    {
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