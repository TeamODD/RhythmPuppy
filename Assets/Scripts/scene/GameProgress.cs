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
    private float targetDistance; // �̵��� �Ÿ�
    private bool isarrivecheckpoint1 = false;
    private bool isarrivecheckpoint2 = false;
    private bool isarrivecheckpoint3 = false;

    public float checkpointTime;

    private void Start()
    {
        MovePlayerBudge(currentMusicPosition);
        SavePointChecking(fillAmount);
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

    private void SavePointChecking(float fillAmount)
    private void SavePointChecking()
    {
        float checkpoint1Threshold = 0.25f; // 25% ���̺� ����Ʈ �Ӱ谪
        float checkpoint2Threshold = 0.50f; // 50% ���̺� ����Ʈ �Ӱ谪
        float checkpoint3Threshold = 0.75f; // 75% ���̺� ����Ʈ �Ӱ谪

        // 25% ���� üũ
        if (fillAmount >= checkpoint1Threshold)
        {
            emptysavepoint1.SetActive(false);
            fullsavepoint1.SetActive(true);
            isarrivecheckpoint1 = true;
        }
        else
        {
            emptysavepoint1.SetActive(true);
            fullsavepoint1.SetActive(false);
        }

        // 50% ���� üũ
        if (fillAmount >= checkpoint2Threshold)
        {
            emptysavepoint2.SetActive(false);
            fullsavepoint2.SetActive(true);
            isarrivecheckpoint2 = true;
        }
        else
        {
            emptysavepoint2.SetActive(true);
            fullsavepoint2.SetActive(false);
        }

        // 75% ���� üũ
        if (fillAmount >= checkpoint3Threshold)
        {
            emptysavepoint3.SetActive(false);
            fullsavepoint3.SetActive(true);
            isarrivecheckpoint3 = true;
        }
        else
        {
            emptysavepoint3.SetActive(true);
            fullsavepoint3.SetActive(false);
        }
    }

    public void CheckingWhereToBack()
    {
        float checkpointTime = 0f;

        if (isarrivecheckpoint3)
        {
            checkpointTime = musicLength * 0.75f;
        }
        else if (isarrivecheckpoint2)
        {
            checkpointTime = musicLength * 0.50f;
        }
        else if (isarrivecheckpoint1)
        {
            checkpointTime = musicLength * 0.25f;
        }

        PlayerPrefs.SetFloat("checkpointTime", checkpointTime);
        
        SettingCheckPoint();
    }

    public void SettingCheckPoint()
    {
        // �÷��̾� ������ ��ġ ����
        float normalizedPosition = Mathf.Clamp01(checkpointTime / musicLength);
        float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, initialPlayerBudgePosition.x + targetDistance, normalizedPosition);
        Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);
        playerbudge.transform.position = newPosition;

        // ���� ��� ��ġ ����
        musicAudioSource.time = checkpointTime;

        // fillImage ������Ʈ (�ʿ��� ���)
        fillImage.fillAmount = checkpointTime / musicLength;
    }
}