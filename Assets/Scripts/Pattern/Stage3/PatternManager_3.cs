using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject Warning;
    [SerializeField]
    private GameObject Apple;
    [SerializeField]
    private GameObject Warning_4;

    private int count_1_b;

    void Start()
    {
        Invoke("Pattern1_b", 68f);

        Invoke("Pattern4", 36f);
        Invoke("Pattern4", 52f);
        Invoke("Pattern4", 60f);

        count_1_b = 1;
    }

    void Pattern1_b()
    {
        Instantiate(target);
        Instantiate(Warning);
        count_1_b++;
        Invoke("Pattern1_b", 0.5f);
    }
    void Pattern4()
    {
        InstantiateApple();
        Invoke("InstantiateApple", 0.5f);
        Invoke("InstantiateApple", 1f);
        Invoke("InstantiateApple", 1.8f);
        Invoke("InstantiateApple", 2.5f);
        Invoke("InstantiateApple", 3f);
        Invoke("InstantiateApple", 3.5f);
        Invoke("InstantiateApple", 4f);
        Invoke("InstantiateApple", 4.5f);
        Invoke("InstantiateApple", 5f);
        Invoke("InstantiateApple", 6f);
        Invoke("InstantiateApple", 6.5f);
        Invoke("InstantiateApple", 7f);
        Invoke("InstantiateApple", 7.5f);
        Invoke("InstantiateApple", 7.6f);
    }
    void InstantiateApple()
    {
        Instantiate(Apple);
        Instantiate(Warning_4);
    }
    void Update()
    {
        if (count_1_b == 28)
            CancelInvoke("Pattern1_b");
    }
}
