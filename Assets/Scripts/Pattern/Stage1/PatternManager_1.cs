using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject Warning;
    private int count_1_a;

    void Awake()
    {
        Invoke("Bee_1_a", 4.0f);
        count_1_a = 1;
    }
    void Bee_1_a()
    {
        Instantiate(target);
        Instantiate(Warning);
        
        count_1_a++;
        Invoke("Bee_1_a", 0.5f);
    }
    void Update()
    {
        //32회 실행시 호출 중단
        if (count_1_a == 32)
            CancelInvoke("Bee_1_a");
    }
    
}
