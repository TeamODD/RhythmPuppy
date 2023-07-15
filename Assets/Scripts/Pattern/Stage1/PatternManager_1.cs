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
        //1�� ������ �Լ� ȣ��(��� 1�� ���� �����Ƿ�), ���� ���ƿ��µ� 1�ʰ� �ɸ����� ����
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
        //32ȸ ����� ȣ�� �ߴ�
        if (count_1_a == 32)
            CancelInvoke("Bee_1_a");
        if (count3 == 32)
            CancelInvoke("Piranha3");
    }
    
}
