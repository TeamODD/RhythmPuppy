using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    public TMP_Text MusicVolumeText;
    public TMP_Text SoundVolumeText;

    private float volumeIncrement = 0.1f; // 볼륨 증가량 조절   

    public void IncreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume + volumeIncrement);
        UpdateMusicText();
    }

    public void DecreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume - volumeIncrement);
        UpdateMusicText();
    }

    public void IncreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume + volumeIncrement);
        UpdateSfxText();
    }

    public void DecreaseSfxVolume()
    {
        btnsource.volume = Mathf.Clamp01(btnsource.volume - volumeIncrement);
        UpdateSfxText();
    }

    private void UpdateMusicText()
    {
        MusicVolumeText.text = (musicsource.volume * 100f).ToString("0") + "%";
    }

    private void UpdateSfxText()
    {
        SoundVolumeText.text = (btnsource.volume * 100f).ToString("0") + "%";
    }
}