using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utiles
{
    public static int[] RandomNumbers(int maxCount, int n)
    {
        int[] defaults = new int[maxCount];
        int[] results = new int[n];

        for (int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }

        for (int i = 0; i < n; ++i)
        {
            int index = Random.Range(0, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount --;
        }

        return results; 
    }
}
