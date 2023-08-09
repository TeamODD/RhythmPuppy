using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public Image fillImage;
    public GameObject playerbudge;
    public RectTransform gameprogressguage;
    public GameObject emptysavepoint1;
    public GameObject emptysavepoint2;
    public GameObject emptysavepoint3;
    public GameObject fullsavepoint1;
    public GameObject fullsavepoint2;
    public GameObject fullsavepoint3;


    private float musicLength;
    private Vector3 initialPlayerBudgePosition;
    private float targetDistance; // 이동할 거리

    private void Start()
    {
        musicLength = musicAudioSource.clip.length;

        musicAudioSource.Play();

        initialPlayerBudgePosition = playerbudge.transform.position;

        targetDistance = gameprogressguage.rect.width;
    }

    private void Update()
    {
        float currentMusicPosition = musicAudioSource.time;

        float fillAmount = currentMusicPosition / musicLength;
        fillImage.fillAmount = fillAmount;

        if (fillAmount >= 1)
        {
            fillAmount = 1;
        }

        MovePlayerBudge(currentMusicPosition);
        SavePointChecking();
    }

    private void MovePlayerBudge(float currentMusicPosition)
    {
        float normalizedPosition = Mathf.Clamp01(currentMusicPosition / musicLength);
        float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, initialPlayerBudgePosition.x + targetDistance, normalizedPosition);
        Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);

        playerbudge.transform.position = newPosition;

        if (currentMusicPosition >= musicLength)
        {
            playerbudge.transform.position = new Vector3(initialPlayerBudgePosition.x + targetDistance, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);
        }
    }

    private void SavePointChecking()
    {
        
    }
}
