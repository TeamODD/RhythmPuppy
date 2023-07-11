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
    //처음 두 마리 붙어서 나옴(가로)
}
