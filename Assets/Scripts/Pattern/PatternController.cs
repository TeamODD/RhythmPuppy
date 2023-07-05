using UnityEngine;

public class PatternController : MonoBehaviour
{
    [SerializeField]
    private GameController  gameController;
    [SerializeField]
    private GameObject[]    patterns;           //보유하고 있는 모든 패턴
    private GameObject      currentPattern;     //현재 재생중인 패턴
    private int[]           patternIndexs;      //겹치지 않는 patterns.Length 개수의 숫자
    private int             current = 0;        //patternIndexs 배열의 순번

    private void Awake()
    {
        //보유하고 있는 패턴(patterns) 개수와 동일하게 메모리 할당
        patternIndexs = new int[patterns.Length];

        //처음에는 패턴을 순차적으로 실행하도록 설정
        for (int i = 0; i < patternIndexs.Length; ++i)
        {
            patternIndexs[i] = i;
        }
    }

    private void Updates()
    {
        if (gameController.IsGamePlay == false)
            return;

        //현재 재생중인 패턴이 종료되어 오브젝트가 비활성화 되면
        if (currentPattern.activeSelf == false)
        {
            // 다음 패턴 실행
        }
    }

    public void GameStart()
    {
        
    }

    public void GameOver()
    {
        currentPattern.SetActive(false);
    }

    //patterns 배열의 patternIndexs[current] 번호에 있는 패턴을 현재 패턴(currentPattern)으로 설정,
    //현재 패턴 오브젝트를 활성화 하면 패턴 컴포넌트에 있는 OnEnable() 메소드가 실행되어 패턴이 진행
    public void ChangePattern()
    {
        //현재 패턴(currentPattern) 변경
        currentPattern = patterns[patternIndexs[current]];
        //현재 패턴 활성화
        currentPattern.SetActive(true);

        current++;

        //패턴을 한바퀴 모두 실행했다면 패턴 순서를 겹치지 않는 임의의 숫자로 설정
        if (current >= patternIndexs.Length)
        {
            patternIndexs = Utils.RandomNumbers(patternIndexs.Length, patternIndexs.Length);
            current = 0;
        }
    }
}
