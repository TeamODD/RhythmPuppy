using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private int count;

    void Start()
    {
        //Invoke("Bee_1_b", 68f);
        Bee_1_b();
        count = 1;
    }

    void Bee_1_b()
    {
        Instantiate(target);
        count++;
        Invoke("Bee_1_b", 0.5f);
    }

    void Update()
    {
        if (count == 28)
            CancelInvoke("Bee_1_b");
    }
}
