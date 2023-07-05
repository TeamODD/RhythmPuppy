using UnityEngine;

public class PatternController : MonoBehaviour
{
    [SerializeField]
    private GameController  gameController;
    [SerializeField]
    private GameObject[]    patterns;           //�����ϰ� �ִ� ��� ����
    private GameObject      currentPattern;     //���� ������� ����
    private int[]           patternIndexs;      //��ġ�� �ʴ� patterns.Length ������ ����
    private int             current = 0;        //patternIndexs �迭�� ����

    private void Awake()
    {
        //�����ϰ� �ִ� ����(patterns) ������ �����ϰ� �޸� �Ҵ�
        patternIndexs = new int[patterns.Length];

        //ó������ ������ ���������� �����ϵ��� ����
        for (int i = 0; i < patternIndexs.Length; ++i)
        {
            patternIndexs[i] = i;
        }
    }

    private void Updates()
    {
        if (gameController.IsGamePlay == false)
            return;

        //���� ������� ������ ����Ǿ� ������Ʈ�� ��Ȱ��ȭ �Ǹ�
        if (currentPattern.activeSelf == false)
        {
            // ���� ���� ����
        }
    }

    public void GameStart()
    {
        
    }

    public void GameOver()
    {
        currentPattern.SetActive(false);
    }

    //patterns �迭�� patternIndexs[current] ��ȣ�� �ִ� ������ ���� ����(currentPattern)���� ����,
    //���� ���� ������Ʈ�� Ȱ��ȭ �ϸ� ���� ������Ʈ�� �ִ� OnEnable() �޼ҵ尡 ����Ǿ� ������ ����
    public void ChangePattern()
    {
        //���� ����(currentPattern) ����
        currentPattern = patterns[patternIndexs[current]];
        //���� ���� Ȱ��ȭ
        currentPattern.SetActive(true);

        current++;

        //������ �ѹ��� ��� �����ߴٸ� ���� ������ ��ġ�� �ʴ� ������ ���ڷ� ����
        if (current >= patternIndexs.Length)
        {
            patternIndexs = Utils.RandomNumbers(patternIndexs.Length, patternIndexs.Length);
            current = 0;
        }
    }
}
