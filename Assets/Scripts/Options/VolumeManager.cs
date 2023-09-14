using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private AudioSource StageMusic;
    private AudioSource StageSound;

    private float StageMusicVolumeIncrement;
    private float StageSoundVolumeIncrement;

    private float MusicVolume;
    private float SfxVolume;

    //볼륨매니저의 역할은 옵션에서 설정한 음량을 스테이지와 메뉴로 저장 및 로드 시키는 것.
    //1.스테이지의 원래 음량을 저장한다.
    //2.원래 음량을 5로 나누어서 스테이지에 맞는 볼륨 증감소율을 구한다.

    private void Start()
    {
        GameObject StageMusicObject = GameObject.FindGameObjectWithTag("MusicManager");
        if (StageMusicObject != null)
        {
            StageMusic = StageMusicObject.GetComponent<AudioSource>();
            float OriginalStageMusicVolume = StageMusic.volume;
            StageMusicVolumeIncrement = OriginalStageMusicVolume / 5;
            PlayerPrefs.SetFloat("StageMusicVolumeIncrement", StageMusicVolumeIncrement);
        }

        GameObject StageSoundObject = GameObject.FindGameObjectWithTag("SoundManager");
        if (StageSoundObject != null)
        {
            StageSound = StageSoundObject.GetComponent<AudioSource>();
            float OriginalStageSoundVolume = StageSound.volume;
            StageSoundVolumeIncrement = OriginalStageSoundVolume / 5;
            PlayerPrefs.SetFloat("StageSoundVolumeIncrement", StageSoundVolumeIncrement);

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
            if (StageSound != null && StageSound.volume != 0)
            {
                StageSound.volume = SfxVolume * 10 * StageSoundVolumeIncrement;
            }
            else if (StageSound != null && StageSound.volume == 0)
            {
                StageSound.volume += StageSoundVolumeIncrement;
            }
        }
    }
}
