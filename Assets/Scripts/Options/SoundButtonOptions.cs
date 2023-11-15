using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource sfxsource;
    public TMP_Text MusicVolumeText;
    public TMP_Text SoundVolumeText;

    private float volumeIncrement = 0.1f; // 볼륨 증가량 조절

    private AudioSource StageMusic;
    private float StageMusicVolumeIncrement;

    private float MusicVolume;
    private float SfxVolume;

    private void Start()
    {
        GameObject StageMusicObject = GameObject.FindGameObjectWithTag("Music");
        if (StageMusicObject != null)
        {
            StageMusic = StageMusicObject.GetComponent<AudioSource>();
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            StageMusicVolumeIncrement = PlayerPrefs.GetFloat("StageMusicVolumeIncrement");

            musicsource.volume = MusicVolume;

            if (StageMusic != null && StageMusic.volume != 0)
            {
                StageMusic.volume = MusicVolume * 10 * StageMusicVolumeIncrement;
            }
            else if (StageMusic != null && StageMusic.volume == 0)
            {
                StageMusic.volume += StageMusicVolumeIncrement;
            }
        }

        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            sfxsource.volume = SfxVolume;
        }
        UpdateMusicText();
        UpdateSfxText();
    }

    public void IncreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume + volumeIncrement);
        if (StageMusic != null)
        {
            StageMusic.volume = Mathf.Clamp01(StageMusic.volume + StageMusicVolumeIncrement);
        }

        PlayerPrefs.SetFloat("MusicVolume", musicsource.volume);
        UpdateMusicText();
    }

    public void DecreaseMusicVolume()
    {
        musicsource.volume = Mathf.Clamp01(musicsource.volume - volumeIncrement);
        if (StageMusic != null)
        {
            StageMusic.volume = Mathf.Clamp01(StageMusic.volume - StageMusicVolumeIncrement);
        }
        
        PlayerPrefs.SetFloat("MusicVolume", musicsource.volume);
        UpdateMusicText();
    }

    public void IncreaseSfxVolume()
    {
        sfxsource.volume = Mathf.Clamp01(sfxsource.volume + volumeIncrement);
        PlayerPrefs.SetFloat("SfxVolume", sfxsource.volume);
        UpdateSfxText();
    }

    public void DecreaseSfxVolume()
    {
        sfxsource.volume = Mathf.Clamp01(sfxsource.volume - volumeIncrement);
        PlayerPrefs.SetFloat("SfxVolume", sfxsource.volume);
        UpdateSfxText();
    }

    private void UpdateMusicText()
    {
        MusicVolumeText.text = (musicsource.volume * 100f).ToString("0") + "%";
    }

    private void UpdateSfxText()
    {
        SoundVolumeText.text = (sfxsource.volume * 100f).ToString("0") + "%";
    }
}