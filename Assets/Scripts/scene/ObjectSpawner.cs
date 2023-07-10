using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab;

    private void Awake()
    {
        for (int y = 0; y < 10; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                Vector3 position = new Vector3(-4.5f + x, 4.5f - y, 0);

                GameObject clone = Instantiate(squarePrefab, position, Quaternion.identity);

                clone.transform.localScale = Vector3.one * 0.95f;
            }
        }
    }
}
