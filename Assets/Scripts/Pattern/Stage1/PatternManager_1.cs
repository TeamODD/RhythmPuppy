using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject Bee;
    [SerializeField]
    private GameObject Warning_1_a;
    [SerializeField]
    private GameObject Piranha;
    [SerializeField]
    private GameObject Warning3;
    [SerializeField]
    private GameObject Oak_2_a;
    [SerializeField]
    private GameObject Oak_2_b;
    [SerializeField]
    private GameObject Warning_2_a;
    [SerializeField]
    private GameObject Warning_2_b;

    private int count_1_a;
    private int count3;

    void Awake()
    {
        count_1_a = 1;
        count3 = 1;
    }
    void Start()
    {
        //1초 빠르게 함수 호출(경고가 1초 동안 나오므로), 벌이 날아오는데 1초가 걸리도록 설정
        Invoke("Pattern1_a", 3.0f);
        Invoke("Pattern3", 19.0f);

        Invoke("Pattern2_a", 12.0f);
        Invoke("Pattern2_a", 20.0f);
        //패턴2_b 첫번째 호출
        Invoke("Pattern2_b", 5.7f);
        Invoke("Pattern2_b", 7.7f);
        //패턴2_b 두번째 호출
        Invoke("Pattern2_b", 13.7f);
        Invoke("Pattern2_b", 15.7f);
        Invoke("Pattern2_b", 17.7f);
        //패턴2_b 세번째 호출
        Invoke("Pattern2_b", 21.7f);
        Invoke("Pattern2_b", 23.7f);

    }
    void Pattern1_a()
    {
        Instantiate(Bee);
        Instantiate(Warning_1_a);

        count_1_a++;
        Invoke("Pattern1_a", 0.5f);
    }

    void Pattern2_a()
    {
        Instantiate(Warning_2_a);
        Instantiate(Oak_2_a);
    }
    void Pattern2_b()
    {
        Instantiate(Warning_2_b);
        Instantiate(Oak_2_b);
    }

    void Pattern3()
    {
        Instantiate(Warning3);
        Instantiate(Piranha);
        
        count3++;
        Invoke("Pattern3", 1f);
    }

    void Update()
    {
        //32회 실행시 호출 중단
        if (count_1_a == 32)
            CancelInvoke("Pattern1_a");
        if (count3 == 32)
        {
            CancelInvoke("Pattern3");
        }
            
    }
    
}
