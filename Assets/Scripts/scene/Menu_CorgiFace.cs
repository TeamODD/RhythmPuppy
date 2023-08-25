using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_CorgiFace : MonoBehaviour
{
    public GameObject[] corgi;

    void Start()
    {
        Facing();
    }
    void Facing()
    {
        switch (Menu_PlayerTransform.clearIndex)
        {
            case 1:
                break;
            case 2:
                corgi[0].SetActive(true);
                break;
            case 4:
                for (int i = 0; i < 2; i++)
                {
                    corgi[i].SetActive(true);
                }
                break;
            case 6:
                for (int i = 0; i < 3; i++)
                {
                    corgi[i].SetActive(true);
                }
                break;
            case 8:
                for (int i = 0; i < 4; i++)
                {
                    corgi[i].SetActive(true);
                }
                break;
            case 10:
                for (int i = 0; i < 5; i++)
                {
                    corgi[i].SetActive(true);
                }
                break;
            case 12:
                for (int i = 0; i < 6; i++)
                {
                    corgi[i].SetActive(true);
                }
                break;
        }
    }
}
