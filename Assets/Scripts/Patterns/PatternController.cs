using System;
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
        if (currentPattern != null && currentPattern.activeSelf == false)
        {
            ChangePattern();
        }
    }

    private void Gamestart()
    {
        ChangePattern();
    }

    private void Gameover()
    {
        if (currentPattern != null)
        {
            currentPattern.SetActive(false);
        }
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

        if (current >= patterns.Length)
        {
            patternIndex = Utiles.RandomNumbers(patternIndex.Length, patternIndex.Length);
            current = 0;
        }
    }
}
