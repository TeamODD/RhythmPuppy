using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundOptions : MonoBehaviour
{
    public AudioSource musicsource;
    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }
}