using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
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
            float OriginalStageMusicVolume = StageMusic.volume;
            StageMusicVolumeIncrement = OriginalStageMusicVolume / 5;
            PlayerPrefs.SetFloat("StageMusicVolumeIncrement", StageMusicVolumeIncrement);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
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
        }
    }
}
