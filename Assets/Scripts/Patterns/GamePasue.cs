using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePasue : MonoBehaviour
{
    private bool isPaused = false;
    public AudioSource stage1_2BGM;

    private void Start()
    {
        // AudioSource ������Ʈ�� �����ɴϴ�. �� ������Ʈ�� �ش� ���� ������Ʈ�� �߰��Ǿ�� �մϴ�.
        stage1_2BGM = stage1_2BGM.GetComponent<AudioSource>();
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

        // ������ �Ͻ������մϴ�.
        if (stage1_2BGM != null && stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Pause();
        }

        // Option_Stage ���� �ε��մϴ�.
        SceneManager.LoadScene("Option_Stage", LoadSceneMode.Additive);

        // ���⿡ �Ͻ������� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // �ð� ����� ���������� �����մϴ�.
        isPaused = false;

        // ������ �ٽ� ����մϴ�.
        if (stage1_2BGM != null && !stage1_2BGM.isPlaying)
        {
            stage1_2BGM.Play();
        }

        // Option_Stage ���� ��ε��մϴ�.
        SceneManager.UnloadScene("Option_Stage");

        // ���⿡ �Ͻ����� ���� �� ������ �۾��� �߰��� �� �ֽ��ϴ�.
    }
}
    
