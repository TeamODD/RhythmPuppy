using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardAction : MonoBehaviour
{
    [SerializeField] GameObject Option;

    public AudioSource musicsource;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Option.activeSelf)
            {
                Option.SetActive(true); 
                musicsource.Play();
            }
        }
    }
}
