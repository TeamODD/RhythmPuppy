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
        eventManager.stageEvent.pauseEvent += PauseGame;
        eventManager.stageEvent.resumeEvent += ResumeGame;
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

        EventSystem.SetActive(false);

        // ������ �Ͻ������մϴ�.
        if (stage1_2BGM != null && stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Pause();
        }

        // Option_Stage ���� �ε��մϴ�.
        SceneManager.LoadScene(SceneInfo.getSceneName(SceneName.OPTION), LoadSceneMode.Additive);

        // ���⿡ �Ͻ������� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // �ð� ����� ���������� �����մϴ�.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = false;

        EventSystem.SetActive(true);

        // ������ �ٽ� ����մϴ�.
        if (stage1_2BGM != null && !stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Play();
        }

        // Option_Stage ���� ��ε��մϴ�.
        SceneManager.UnloadSceneAsync(SceneInfo.getSceneName(SceneName.OPTION));

        // ���⿡ �Ͻ����� ���� �� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }
}
    
