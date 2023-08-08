using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public AudioSource musicAudioSource; // ������ ����ϴ� AudioSource ������Ʈ
    public Image fillImage; // ä�� ������ ��Ÿ���� �̹���

    private float musicLength; // ������ ���� (��)
    private float beatInterval; // ��Ʈ ���� (��)

    private void Start()
    {
        // ������ ���̿� ��Ʈ ������ ����մϴ�.
        musicLength = musicAudioSource.clip.length;
        beatInterval = musicLength / (float) Time.time; // numberOfBeats�� ��Ȯ�� ��Ʈ ���� �����ϼ���.

        // ������ ����մϴ�.
        musicAudioSource.Play();
    }

    private void Update()
    {
        // ���� ������ ��� ��ġ�� ����մϴ�.
        float currentMusicPosition = musicAudioSource.time;

        // �̹����� ä�� ������ ���� ������ ��� ��ġ�� �°� �����մϴ�.
        float fillAmount = currentMusicPosition / musicLength;
        fillImage.fillAmount = fillAmount;
    }
}
