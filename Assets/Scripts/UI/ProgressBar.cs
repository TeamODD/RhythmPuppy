using EventManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] GameObject emptySavePointPrefab, fullSavePointPrefab;
        [SerializeField] Transform playerBudge, beginPointTransform, endPointTransform;

        AudioSource musicAudioSource, savePointSound;

        EventManager eventManager;
        Image fillImage;
        WaitUntil w;
        GameObject[] emptySavePoint, fullSavePoint;
        Coroutine initCoroutine;
        Vector2 beginPoint, endPoint;
        int filledSavePoints;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            fillImage = transform.Find("GameProgressGuage").GetComponent<Image>();
            musicAudioSource = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
            savePointSound = transform.GetComponent<AudioSource>();
            w = new WaitUntil(() => musicAudioSource.clip != null);
            beginPoint = beginPointTransform.position;
            endPoint = endPointTransform.position;
            fillImage.fillAmount = 0;
            filledSavePoints = 0;

            eventManager.stageEvent.gameStartEvent += gameStartEvent;
            eventManager.stageEvent.rewindEvent += rewindEvent;
            eventManager.playerEvent.deathEvent += deathEvent;
            eventManager.playerEvent.reviveEvent += gameStartEvent;
            eventManager.uiEvent.resolutionChangeEvent += resolutionChangeEvent;
        }

        void Start()
        {
            initCoroutine = StartCoroutine(init());
        }

        private IEnumerator init()
        {
            yield return w;
            emptySavePoint = new GameObject[eventManager.savePointTime.Length];
            fullSavePoint = new GameObject[eventManager.savePointTime.Length];
            for (int i = 0; i < eventManager.savePointTime.Length; i++)
            {
                float normalizedPosition = Mathf.Clamp01(eventManager.savePointTime[i] / musicAudioSource.clip.length);
                float targetX = Mathf.Lerp(beginPoint.x, endPoint.x, normalizedPosition);
                Vector3 newPosition = new Vector2(targetX, beginPoint.y);

                emptySavePoint[i] = Instantiate(emptySavePointPrefab);
                emptySavePoint[i].transform.SetParent(transform);
                emptySavePoint[i].transform.localScale = Vector3.one;
                emptySavePoint[i].SetActive(true);
                emptySavePoint[i].transform.position = newPosition;
                fullSavePoint[i] = Instantiate(fullSavePointPrefab);
                fullSavePoint[i].transform.SetParent(transform);
                fullSavePoint[i].transform.localScale = Vector3.one;
                fullSavePoint[i].SetActive(false);
                fullSavePoint[i].transform.position = newPosition;
            }
            initCoroutine = null;
        }

        void Update()
        {
            if (initCoroutine != null) return;

            if (musicAudioSource.clip.length - 0.7f < musicAudioSource.time)
            {
                MovePlayerBudge(musicAudioSource.clip.length);
                SavePointChecking();
                musicAudioSource.Pause();
                musicAudioSource.time = musicAudioSource.clip.length - 0.1f;
                return;
            }

            float currentMusicPosition = musicAudioSource.time;
            float fillAmount = currentMusicPosition / musicAudioSource.clip.length;
            fillImage.fillAmount = fillAmount;

            if (1 <= fillAmount)
            {
                fillAmount = 1;
            }

            MovePlayerBudge(currentMusicPosition);
            SavePointChecking();
        }

        void resolutionChangeEvent()
        {
            beginPoint = beginPointTransform.position;
            endPoint = endPointTransform.position;
        }


        private void MovePlayerBudge(float currentMusicPosition)
        {
            float normalizedPosition = Mathf.Clamp01(currentMusicPosition / musicAudioSource.clip.length);
            float targetX = Mathf.Lerp(beginPoint.x, endPoint.x, normalizedPosition);
            Vector2 newPosition = new Vector2(targetX, beginPoint.y);

            playerBudge.position = newPosition;

            if (1 <= normalizedPosition)
            {
                playerBudge.position = new Vector2(endPoint.x, beginPoint.y);
            }
        }

        private void SavePointChecking()
        {
            for (int i = filledSavePoints; i < emptySavePoint.Length; i++)
            {
                if (eventManager.savePointTime[i] <= musicAudioSource.time)
                {
                    // i번째 세이브포인트 달성
                    emptySavePoint[i].SetActive(false);
                    fullSavePoint[i].SetActive(true);
                    savePointSound.Play();
                    filledSavePoints += 1;
                }
                else
                {
                    // i번째 세이브포인트 미달성
                    emptySavePoint[i].SetActive(true);
                    fullSavePoint[i].SetActive(false);
                }
            }
        }

        private void gameStartEvent()
        {
            StartCoroutine(playMusic());
        }

        private IEnumerator playMusic()
        {
            /* gameStart 이벤트 1초뒤에 음악이 시작됨 (패턴 경고등을 보여주는 시간 때문) */
            yield return new WaitForSeconds(1);
            musicAudioSource.Play();
        }

        private void rewindEvent()
        {
            if (filledSavePoints == 0)
                musicAudioSource.time = 0;
            else
                musicAudioSource.time = eventManager.savePointTime[filledSavePoints - 1];
        }

        private void deathEvent()
        {
            musicAudioSource.Pause();
        }
    }
}