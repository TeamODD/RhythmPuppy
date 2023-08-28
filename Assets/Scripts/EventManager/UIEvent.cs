using UnityEngine;

public struct UIEvent
{
    public delegate void EnableBlindEvent();
    public delegate void DisableBlindEvent();
    public delegate void EnableClearSpotlightEvent();
    public delegate void DisableClearSpotlightEvent();
    public delegate bool GetLampStatus();
    public delegate void FadeInEvent();
    public delegate void FadeOutEvent();

    [HideInInspector] public bool onBlindEvent;
    public EnableBlindEvent enableBlindEvent;
    public DisableBlindEvent disableBlindEvent;
    public EnableClearSpotlightEvent enableClearSpotlightEvent;
    public DisableClearSpotlightEvent disableClearSpotlightEvent;
    public GetLampStatus getLampStatus;
    public FadeInEvent fadeInEvent;
    public FadeOutEvent fadeOutEvent;

}