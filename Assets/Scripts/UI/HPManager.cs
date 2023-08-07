using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] List<GameObject> state;

    public void updateHP(int hp)
    {
        if (hp < 0) hp = 0;
        else if (state.Count <= hp) hp = state.Count - 1; 

        foreach (GameObject o in state)
        {
            o.SetActive(false);
        }

        state[hp].SetActive(true);
    }
}
