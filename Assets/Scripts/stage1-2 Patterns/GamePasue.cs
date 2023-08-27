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

    private void Start()
    {
        stage1_2BGM = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
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
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // �ð� ����� ����ϴ�.
        isPaused = true;

        EventSystem.SetActive(false);

        // ������ �Ͻ������մϴ�.
        if (stage1_2BGM != null && stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Pause();
        }

        // Option_Stage ���� �ε��մϴ�.
        SceneManager.LoadScene("Option_Stage", LoadSceneMode.Additive);

        // ���⿡ �Ͻ������� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // �ð� ����� ���������� �����մϴ�.
        isPaused = false;

        EventSystem.SetActive(true);

        // ������ �ٽ� ����մϴ�.
        if (stage1_2BGM != null && !stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Play();
        }

        // Option_Stage ���� ��ε��մϴ�.
        SceneManager.UnloadSceneAsync("Option_Stage");

        // ���⿡ �Ͻ����� ���� �� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }
}
    
