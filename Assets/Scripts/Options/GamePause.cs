using EventManagement;
using SceneData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    [HideInInspector] public bool isPaused;
    AudioSource BGM;
    EventManager eventManager;

    void Awake()
    {
        // stage1_2BGM = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        BGM = FindObjectOfType<AudioSource>();
        eventManager = FindObjectOfType<EventManager>();
        isPaused = false;

        eventManager.stageEvent.pauseEvent += PauseEvent;
        eventManager.stageEvent.resumeEvent += ResumeEvent;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        // 하이어리키의 EventManager 오브젝트에서 이 함수를 참조함 
        if (!isPaused)
        {
            eventManager.stageEvent.pauseEvent();
        }
        else
        {
            eventManager.stageEvent.resumeEvent();
        }
    }

    private void PauseEvent()
    {
        Time.timeScale = 0f; // 시간 경과를 멈춥니다.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = true;

        // 음악을 일시정지합니다.
        if (BGM != null && BGM.isPlaying)
        {
            BGM.Pause();
        }
        // Option_Stage 씬을 로드합니다.
        SceneManager.LoadScene(SceneInfo.getSceneName(SceneName.OPTION), LoadSceneMode.Additive);

        // 여기에 일시정지시 수행할 작업을 추가할 수 있습니다.
    }

    public void ResumeEvent()
    {
        Time.timeScale = 1f; // 시간 경과를 정상적으로 진행합니다.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = false;

        // 음악을 다시 재생합니다.
        if (BGM != null && !BGM.isPlaying)
        {
            BGM.Play();
        }

        // Option_Stage 씬을 언로드합니다.
        SceneManager.UnloadSceneAsync(SceneInfo.getSceneName(SceneName.OPTION));

        // 여기에 일시정지 해제 시 수행할 작업을 추가할 수 있습니다.
    }
}

