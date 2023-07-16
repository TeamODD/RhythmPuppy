using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject Bee_1_a;
    [SerializeField]
    private GameObject Warning_1_a;
    [SerializeField]
    private GameObject Bee_1_b;
    [SerializeField]
    private GameObject Warning_1_b;
    [SerializeField]
    private GameObject Piranha;
    [SerializeField]
    private GameObject Warning_3;
    [SerializeField]
    private GameObject Oak_2_a;
    [SerializeField]
    private GameObject Oak_2_b;
    [SerializeField]
    private GameObject Warning_2_a;
    [SerializeField]
    private GameObject Warning_2_b;
    [SerializeField]
    private GameObject Apple;
    [SerializeField]
    private GameObject Warning_4;

    private float time;
    private int count_1_a;
    private int count_1_b;
    private int count_2_a;
    private int count_2_b;
    private int count_3;
    private int count_4;

    void Awake()
    {
        time = 0;
        count_1_a = 1;
        count_1_b = 1;
        count_3 = 1;
        count_2_a = 1;
        count_2_b = 1;
        count_4 = 1;
    }
    void Start()
    {
        //1�� ������ �Լ� ȣ��(��� 1�� ���� �����Ƿ�), ���� ���ƿ��µ� 1�ʰ� �ɸ����� ����
        Invoke("Pattern1_a", 3.0f);
        Invoke("Pattern3", 19.0f);

    }
    void Pattern1_a()
    {
        Instantiate(Bee_1_a);
        Instantiate(Warning_1_a);

        count_1_a++;
        Invoke("Pattern1_a", 0.5f);
    }
    void Pattern1_b()
    {
        Instantiate(Bee_1_b);
        Instantiate(Warning_1_b);

        count_1_b++;
        Invoke("Pattern1_b", 2f);
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
        Instantiate(Warning_3);
        Instantiate(Piranha);
        
        count_3++;
        Invoke("Pattern3", 1f);
    }

    void Update()
    {
        //32ȸ ����� ȣ�� �ߴ�
        if (count_1_a == 32)
        {
            CancelInvoke("Pattern1_a");
            count_1_a = 1;
        }
        if (count_3 == 32)
        {
            CancelInvoke("Pattern3");
            count_3 = 1;
        }
            
    }
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        
        
    }
}
