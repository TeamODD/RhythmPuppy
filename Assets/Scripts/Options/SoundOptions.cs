using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    
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
}