using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public GameObject target;
    public GameObject Warning;
    private int count;
    
    void Awake()
    {
        Bee();
        count = 1;
    }
    void Bee()
    {
        Instantiate(target);
        Instantiate(Warning);
        
        count++;
        Invoke("Bee", 0.5f);
    }
    void Update()
    {
        if (count == 32)
            CancelInvoke("Bee");
    }
    //ó�� �� ���� �پ ����(����)
}
