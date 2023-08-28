using EventManagement;
using SceneData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePasue : MonoBehaviour
{
    private bool isPaused = false;
    AudioSource stage1_2BGM;
    [SerializeField]
    GameObject EventSystem;
    EventManager eventManager;

    private void Start()
    {
        stage1_2BGM = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        eventManager = FindObjectOfType<EventManager>();
        eventManager.stageEvent.pauseEvent += TogglePause;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC 키를 누르면 일시정지/해제
        {
            eventManager.stageEvent.pauseEvent();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // 시간 경과를 멈춥니다.
        Time.fixedDeltaTime = 0;
        isPaused = true;

        EventSystem.SetActive(false);

        // 음악을 일시정지합니다.
        if (stage1_2BGM != null && stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Pause();
        }

        // Option_Stage 씬을 로드합니다.
        SceneManager.LoadScene(SceneInfo.getSceneName(SceneName.OPTION), LoadSceneMode.Additive);

        // 여기에 일시정지시 수행할 작업을 추가할 수 있습니다.
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 시간 경과를 정상적으로 진행합니다.
        Time.fixedDeltaTime = 0.02f;
        isPaused = false;

        EventSystem.SetActive(true);

        // 음악을 다시 재생합니다.
        if (stage1_2BGM != null && !stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Play();
        }

        // Option_Stage 씬을 언로드합니다.
        SceneManager.UnloadSceneAsync(SceneInfo.getSceneName(SceneName.OPTION));

        // 여기에 일시정지 해제 시 수행할 작업을 추가할 수 있습니다.
    }
}
    
