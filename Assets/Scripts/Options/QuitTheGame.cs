using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitTheGame : MonoBehaviour
{
    public void OnQuitTheGame()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            // 유니티 에디터에서 실행 중인 경우
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            // 실행 중인 플레이 모드가 에디터가 아닌 경우 (실제 빌드된 게임 등)
            Application.Quit();
        }
    }
}
