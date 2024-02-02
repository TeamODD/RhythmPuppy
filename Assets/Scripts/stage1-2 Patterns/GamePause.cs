using EventManagement;
using SceneData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    public bool isPaused = false;
    AudioSource BGM;
    EventManager eventManager;

    private void Start()
    {
        /*stage1_2BGM = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();*/
        BGM = FindObjectOfType<AudioSource>();
        eventManager = FindObjectOfType<EventManager>();
        eventManager.stageEvent.pauseEvent += PauseGame;
        eventManager.stageEvent.resumeEvent += ResumeEvent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC Ű�� ������ �Ͻ�����/����
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            eventManager.stageEvent.resumeEvent();
        }
        else
        {
            eventManager.stageEvent.pauseEvent();
        }
    }

    private void PauseGame()
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

    public void ResumeGame()
    {
        eventManager.stageEvent.resumeEvent();
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

