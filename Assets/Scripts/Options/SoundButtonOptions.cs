using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    public Slider BGMSlider;
    public Slider SFSlider;

    private float volumeIncrement = 0.1f; // 볼륨 증가량 조절   

    public void IncreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume + volumeIncrement);
        BGMSlider.value = musicsource.volume;
    }

    public void DecreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume - volumeIncrement);
        BGMSlider.value = musicsource.volume;
    }

    public void IncreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume + volumeIncrement);
        SFSlider.value = btnsource.volume;
    }

    public void DecreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume - volumeIncrement);
        SFSlider.value = btnsource.volume;
    }
}
