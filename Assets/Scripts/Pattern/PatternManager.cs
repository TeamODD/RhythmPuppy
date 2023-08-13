using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject pattern;

    public void init()
    {
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(transform);
        o.SetActive(true);
    }
}
