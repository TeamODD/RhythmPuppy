using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab;
    [SerializeField]
    private float spacing = 0.1f;

    private void Awake()
    {
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        float squareWidth = (screenWidth - spacing * 15) / 16;
        float squareHeight = (screenHeight - spacing * 8) / 9;

        for (int y = 0; y < 9; ++y)
        {
            for (int x = 0; x < 16; ++x)
            {
                Vector3 position = new Vector3(squareWidth * x - screenWidth / 2 + squareWidth / 2 + spacing * x, screenHeight / 2 - squareHeight * y - squareHeight / 2 - spacing * y, 0);

                GameObject clone = Instantiate(squarePrefab, position, Quaternion.identity);

                clone.transform.localScale = new Vector3(squareWidth, squareHeight, 1);
            }
        }
    }
}