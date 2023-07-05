using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PatternController patternController;

    public bool IsGamePlay { private set; get; } = false;

    public void GameStart()
    {
        patternController.GameStart();

        IsGamePlay = true;
    }

    public void GameOver()
    {
        patternController.GameOver();

        IsGamePlay = false;
    }
}
