using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject pattern;

    void Awake()
    {
        init();
    }

    public void init()
    {
        Invoke("runPattern", 1f);
    }

    private void runPattern()
    {
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(transform);
        o.SetActive(true);
    }
}
