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
        // ���̾Ű�� EventManager ������Ʈ���� �� �Լ��� ������ 
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
        Time.timeScale = 0f; // �ð� ����� ����ϴ�.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = true;

        // ������ �Ͻ������մϴ�.
        if (BGM != null && BGM.isPlaying)
        {
            BGM.Pause();
        }
        // Option_Stage ���� �ε��մϴ�.
        SceneManager.LoadScene(SceneInfo.getSceneName(SceneName.OPTION), LoadSceneMode.Additive);

        // ���⿡ �Ͻ������� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }

    public void ResumeEvent()
    {
        Time.timeScale = 1f; // �ð� ����� ���������� �����մϴ�.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = false;

        // ������ �ٽ� ����մϴ�.
        if (BGM != null && !BGM.isPlaying)
        {
            BGM.Play();
        }

        // Option_Stage ���� ��ε��մϴ�.
        SceneManager.UnloadSceneAsync(SceneInfo.getSceneName(SceneName.OPTION));

        // ���⿡ �Ͻ����� ���� �� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }
}

