using EventManagement;
using SceneData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    public bool isPaused { get; set; }
    AudioSource BGM;
    EventManager eventManager;

    void Awake()
    {
        /*stage1_2BGM = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();*/
        BGM = FindObjectOfType<AudioSource>();
        eventManager = GetComponentInParent<EventManager>();
        isPaused = false;

        eventManager.onPause.AddListener(PauseEvent);
        eventManager.onResume.AddListener(ResumeEvent);
    }

    public void TogglePause()
    {
        /* ���̾Ű�� EventManager ������Ʈ���� �� �Լ��� ������ */
        if (isPaused)
        {
            PauseEvent();
        }
        else
        {
            ResumeEvent();
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

