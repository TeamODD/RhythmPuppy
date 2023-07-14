using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{

    private GameObject player;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 2.2f, 0);
    }
}
