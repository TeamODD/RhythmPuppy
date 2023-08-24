using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] float[] savePointTime;

    AudioSource musicAudioSource;
    Coroutine initAudioSource;

    Image fillImage;
    GameObject playerbudge, puppybudge;
    RectTransform gameprogressguage;
    GameObject[] emptysavepoint;
    GameObject[] fullsavepoint;

    float musicLength;
    Vector3 initialPlayerBudgePosition;
    float targetDistance; // 이동할 거리
    bool[] isarrivecheckpoint;

    private float checkpointTime;

    private void Awake()
    {
        fillImage = transform.Find("GameProgressGuage").GetComponent<Image>();
        playerbudge = transform.Find("MovingPlayerBudge").gameObject;
        puppybudge = transform.Find("PuppyBudge").gameObject;

        emptysavepoint = new GameObject[3];
        emptysavepoint[0] = transform.Find("EmptySavePoint1").gameObject;
        emptysavepoint[1] = transform.Find("EmptySavePoint2").gameObject;
        emptysavepoint[2] = transform.Find("EmptySavePoint3").gameObject;

        fullsavepoint = new GameObject[3];
        fullsavepoint[0] = transform.Find("FullSavePoint1").gameObject;
        fullsavepoint[1] = transform.Find("FullSavePoint2").gameObject;
        fullsavepoint[2] = transform.Find("FullSavePoint3").gameObject;

        isarrivecheckpoint = new bool[3] { false, false, false };

        gameprogressguage = transform.Find("FullSavePoint3").GetComponent<RectTransform>();
        initAudioSource = StartCoroutine(init());

        initialPlayerBudgePosition = playerbudge.transform.position;
        targetDistance = gameprogressguage.rect.width;
        
    }

    private void Update()
    {
        if (initAudioSource != null || !musicAudioSource.isPlaying) return;

        float currentMusicPosition = musicAudioSource.time;

        float fillAmount = currentMusicPosition / musicLength;
        fillImage.fillAmount = fillAmount;

        if (fillAmount >= 1)
        {
            fillAmount = 1;
        }

        MovePlayerBudge(currentMusicPosition);
        SavePointChecking(fillAmount);
    }

    private IEnumerator init()
    {
        fillImage.fillAmount = 0;
        musicAudioSource = FindObjectOfType<AudioSource>();
        while (musicAudioSource.clip == null) yield return null;
        
        musicLength = musicAudioSource.clip.length;  //158.6678f 
        initAudioSource = null;

        for (int i = 0; i < savePointTime.Length; i++)
        {
            float normalizedPosition = Mathf.Clamp01(savePointTime[i] / musicLength);
            float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, puppybudge.transform.position.x, normalizedPosition);
            Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);

            emptysavepoint[i].transform.position = newPosition;
            fullsavepoint[i].transform.position = newPosition;
        }
    }

    private void MovePlayerBudge(float currentMusicPosition)
    {
        float normalizedPosition = Mathf.Clamp01(currentMusicPosition / musicLength);
        float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, puppybudge.transform.position.x, normalizedPosition);
        /*float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, initialPlayerBudgePosition.x + targetDistance, normalizedPosition)*/;
        Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);

        playerbudge.transform.position = newPosition;

        if (currentMusicPosition >= musicLength)
        {
            playerbudge.transform.position = new Vector3(initialPlayerBudgePosition.x + targetDistance, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);
        }
    }

    private void SavePointChecking(float fillAmount)
    {
        if (fillAmount >= savePointTime[0])
        {
            emptysavepoint[0].SetActive(false);
            fullsavepoint[0].SetActive(true);
            isarrivecheckpoint[0] = true;
        }
        else
        {
            emptysavepoint[0].SetActive(true);
            fullsavepoint[0].SetActive(false);
        }

        if (fillAmount >= savePointTime[1])
        {
            emptysavepoint[1].SetActive(false);
            fullsavepoint[1].SetActive(true);
            isarrivecheckpoint[1] = true;
        }
        else
        {
            emptysavepoint[1].SetActive(true);
            fullsavepoint[1].SetActive(false);
        }

        if (fillAmount >= savePointTime[2])
        {
            emptysavepoint[2].SetActive(false);
            fullsavepoint[2].SetActive(true);
            isarrivecheckpoint[2] = true;
        }
        else
        {
            emptysavepoint[2].SetActive(true);
            fullsavepoint[2].SetActive(false);
        }
    }

    public void CheckingWhereToBack()
    {
        float checkpointTime = 0f;

        if (isarrivecheckpoint[2])
        {
            checkpointTime = musicLength * 0.75f;
        }
        else if (isarrivecheckpoint[1])
        {
            checkpointTime = musicLength * 0.50f;
        }
        else if (isarrivecheckpoint[0])
        {
            checkpointTime = musicLength * 0.25f;
        }

        PlayerPrefs.SetFloat("checkpointTime", checkpointTime);

        SettingCheckPoint();
    }

    public void SettingCheckPoint()
    {
        // 플레이어 뱃지의 위치 설정
        float normalizedPosition = Mathf.Clamp01(checkpointTime / musicLength);
        float targetX = Mathf.Lerp(initialPlayerBudgePosition.x, initialPlayerBudgePosition.x + targetDistance, normalizedPosition);
        Vector3 newPosition = new Vector3(targetX, initialPlayerBudgePosition.y, initialPlayerBudgePosition.z);
        playerbudge.transform.position = newPosition;

        // 음악 재생 위치 설정
        musicAudioSource.time = checkpointTime;

        // fillImage 업데이트 (필요한 경우)
        fillImage.fillAmount = checkpointTime / musicLength;
    }
}