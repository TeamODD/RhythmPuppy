using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    Transform player;
    Vector3 positionCorrectVector;

    void Awake()
    {
        init();
    }

    void Update()
    {
        transform.position = player.position + positionCorrectVector;
    }

    private void init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        positionCorrectVector = new Vector3(0, 1f, 0);
    }
}
