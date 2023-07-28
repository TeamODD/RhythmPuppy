using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] List<GameObject> state;

    public void updateHP(int hp)
    {
        foreach (GameObject o in state)
        {
            o.SetActive(false);
        }

        state[hp].SetActive(true);
    }
}
