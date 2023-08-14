using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;

    AudioSource stagemusicsource;

    private void Start()
    {
        GameObject stagemusic = GameObject.FindWithTag("StageMusic");
        stagemusicsource = stagemusic.GetComponent<AudioSource>(); //일단 보류.
    }

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }

    public void SetButtonVolume(float volume)
    {
        btnsource.volume = volume;
    }

    public void OnSfx()
    {
        btnsource.Play();
    }

    public void OffBgm()
    {
        musicsource.Stop();
    }
}