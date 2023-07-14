using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject Warning;

    private int count_1_b;

    void Start()
    {
        //Invoke("Bee_1_b", 68f);
        Bee_1_b();
        count_1_b = 1;
    }

    void Bee_1_b()
    {
        Instantiate(target);
        Instantiate(Warning);
        count_1_b++;
        Invoke("Bee_1_b", 0.5f);
    }

    void Update()
    {
        if (count_1_b == 28)
            CancelInvoke("Bee_1_b");
    }
}
