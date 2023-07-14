using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] patterns;
    private GameObject currentPattern;
    private int[] patternIndex;
    private int current = 0;

    private void Awake()
    {
        patternIndex = new int[patterns.Length];

        for (int i = 0; i < patterns.Length; i++)
        {
            patternIndex[i] = i;
        }
    }

    private void Start()
    {
        Gamestart();
    }

    private void Update()
    {
        if (currentPattern.activeSelf == false)
        {
            ChangePattern();
        }
    }

    private void Gamestart()
    {
        currentPattern = patterns[patternIndex[current]];
        currentPattern.SetActive(true);
        current++;
    }

    private void ChangePattern()
    {
        if (currentPattern != null)
        {
            currentPattern.SetActive(false);
        }

        currentPattern = patterns[patternIndex[current]];
        currentPattern.SetActive(true);

        current++;

        if (current >= patternIndex.Length)
        {
            patternIndex = Utiles.RandomNumbers(patternIndex.Length, patternIndex.Length);
            current = 0;
        }
    }
}
