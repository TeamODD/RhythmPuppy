using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    public Slider BGMSlider;
    public Slider SFSlider;
    public TMP_Text BgmText;
    public TMP_Text SfxText;

    private float volumeIncrement = 0.1f; // 볼륨 증가량 조절   

    public void IncreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume + volumeIncrement);
        BGMSlider.value = musicsource.volume;
        UpdateMusicText();
    }

    public void DecreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume - volumeIncrement);
        BGMSlider.value = musicsource.volume;
        UpdateMusicText();
    }

    public void IncreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume + volumeIncrement);
        SFSlider.value = btnsource.volume;
        UpdateSfxText();
    }

    public void DecreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume - volumeIncrement);
        SFSlider.value = btnsource.volume;
        UpdateSfxText();
    }

    private void UpdateMusicText()
    {
        BgmText.text = (musicsource.volume * 100f).ToString("0") + "%";
    }

    private void UpdateSfxText()
    {
        SfxText.text = (btnsource.volume * 100f).ToString("0") + "%";
    }
}