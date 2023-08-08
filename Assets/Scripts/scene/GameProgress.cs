using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public AudioSource musicAudioSource; // 음악을 재생하는 AudioSource 컴포넌트
    public Image fillImage; // 채움 정도를 나타내는 이미지

    private float musicLength; // 음악의 길이 (초)
    private float beatInterval; // 비트 간격 (초)

    private void Start()
    {
        // 음악의 길이와 비트 간격을 계산합니다.
        musicLength = musicAudioSource.clip.length;
        beatInterval = musicLength / (float) Time.time; // numberOfBeats를 정확한 비트 수로 변경하세요.

        // 음악을 재생합니다.
        musicAudioSource.Play();
    }

    private void Update()
    {
        // 현재 음악의 재생 위치를 계산합니다.
        float currentMusicPosition = musicAudioSource.time;

        // 이미지의 채움 정도를 현재 음악의 재생 위치에 맞게 조정합니다.
        float fillAmount = currentMusicPosition / musicLength;
        fillImage.fillAmount = fillAmount;
    }
}
