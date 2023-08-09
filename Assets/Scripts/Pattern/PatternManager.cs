using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject pattern;

    void Start()
    {
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(transform);
        o.SetActive(true);
    }
}
