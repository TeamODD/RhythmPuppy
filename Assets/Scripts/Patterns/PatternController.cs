using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PatternController : MonoBehaviour
{
    [SerializeField]
   /private GameController gameController;
    [SerializeField]
    private GameObject[] patterns; //�����ϰ� �ִ� ��� ����
    private GameObject currentPatterns; //���� �۵��ϰ� �ִ� ����
    private int[] patternIndexs; //��ġ�� �ʴ� Patterns.Length ������ ����
    private int current = 0; //patterns �迭�� ����

    private void Awake()
    {
        patternIndexs = new int[patterns.Length];

        //ó������ ������ ���������� �����ϵ��� ����
        for (int i = 0; i < patternIndexs.Length; ++i)
        {
            patternIndexs[i] = i;
        }
    }

    private void Update()
    {
        if (gameController.IsGamePlay == false) return;

        //���� ������� ������ ����Ǿ� ������Ʈ ��Ȱ��ȭ�Ǹ�
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
        //���� ������� ���ϸ� ��Ȱ��ȭ
    }

    public void ChangePattern()
    {
        //���� ���� ����
        currentPatterns = patterns[patternIndexs[current]];

        //���� ���� Ȱ��ȭ
        currentPatterns.SetActive(true);

        current++;

        //������ �ѹ��� ��� �����ߴٸ� ���� ������ ��ġ�� �ʴ� ������ ���ڷ� ����
        if (current >= patterns.Length)
        {
            patternIndexs = Utils.RandomNumbers(patternIndexs.Length, patternIndexs.Length);
            current = 0;

        }
    }
}
