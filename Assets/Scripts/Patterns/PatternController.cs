using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PatternController : MonoBehaviour
{
    [SerializeField]
   /private GameController gameController;
    [SerializeField]
    private GameObject[] patterns; //보유하고 있는 모든 패턴
    private GameObject currentPatterns; //현재 작동하고 있는 패턴
    private int[] patternIndexs; //겹치지 않는 Patterns.Length 개수의 숫자
    private int current = 0; //patterns 배열의 순번

    private void Awake()
    {
        patternIndexs = new int[patterns.Length];

        //처음에는 패턴을 순차적으로 실행하도록 설정
        for (int i = 0; i < patternIndexs.Length; ++i)
        {
            patternIndexs[i] = i;
        }
    }

    private void Update()
    {
        if (gameController.IsGamePlay == false) return;

        //현재 재생중인 패턴이 종료되어 오브젝트 비활성화되면
        if (currentPattern.activeSelf == false)
        {
            ChangePattern();
        }
    }

    public void GameStart()
    {
        ChangePattern();
    }

    public void GameOver()
    {
        //현재 재생중인 패턴만 비활성화
    }

    public void ChangePattern()
    {
        //현재 패턴 변경
        currentPatterns = patterns[patternIndexs[current]];

        //현재 패턴 활성화
        currentPatterns.SetActive(true);

        current++;

        //패턴을 한바퀴 모두 실행했다면 패턴 순서를 겹치지 않는 임의의 숫자로 설정
        if (current >= patterns.Length)
        {
            patternIndexs = Utils.RandomNumbers(patternIndexs.Length, patternIndexs.Length);
            current = 0;

        }
    }
}
