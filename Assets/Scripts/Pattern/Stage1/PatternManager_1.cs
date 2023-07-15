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
        Invoke("Bee_1_a", 3.0f);
        Invoke("Piranha3", 19.0f);
    }
    void Bee_1_a()
    {
        Instantiate(Bee);
        Instantiate(Warning_1_a);

        count_1_a++;
        Invoke("Bee_1_a", 0.5f);
    }
    void Piranha3()
    {
        Instantiate(Warning3);
        Instantiate(Piranha);
        
        count3++;
        Invoke("Piranha3", 1f);
    }
    
    void Update()
    {
        //32회 실행시 호출 중단
        if (count_1_a == 32)
            CancelInvoke("Bee_1_a");
        if (count3 == 32)
            CancelInvoke("Piranha3");
    }
    
}
