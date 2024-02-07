using EventManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] GameObject emptySavePointPrefab;
        [SerializeField] GameObject fullSavePointPrefab;
        [SerializeField] Transform playerBudge, beginPointTransform, endPointTransform;

        AudioSource musicAudioSource;

        EventManager eventManager;
        Image fillImage;
        WaitUntil w;
        GameObject[] emptySavePoint, fullSavePoint;
        Coroutine initCoroutine;
        Vector2 beginPoint, endPoint;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            fillImage = transform.Find("GameProgressGuage").GetComponent<Image>();
            musicAudioSource = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
            w = new WaitUntil(() => musicAudioSource.clip != null);
            beginPoint = beginPointTransform.position;
            endPoint = endPointTransform.position;
            fillImage.fillAmount = 0;

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
                SavePointChecking(1);
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
            SavePointChecking(fillAmount);
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

        private void SavePointChecking(float fillAmount)
        {
            for (int i = 0; i < emptySavePoint.Length; i++)
            {
                if (eventManager.savePointTime[i] <= musicAudioSource.time)
                {
                    emptySavePoint[i].SetActive(false);
                    fullSavePoint[i].SetActive(true);
                }
                else
                {
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
            for (int i = 0; i < eventManager.savePointTime.Length; i++)
            {
                if (eventManager.savePointTime[i] <= musicAudioSource.time) continue;

                if (i == 0)
                    musicAudioSource.time = 0;
                else
                    musicAudioSource.time = eventManager.savePointTime[i - 1];
                return;
            }
            musicAudioSource.time = eventManager.savePointTime[eventManager.savePointTime.Length - 1];
        }

        private void deathEvent()
        {
            musicAudioSource.Pause();
        }
    }
}