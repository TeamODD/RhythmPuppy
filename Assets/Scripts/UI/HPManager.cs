using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] List<GameObject> state;

    Player player;

    void Awake()
    {
        init();
    }

    void Update()
    {
        updateHP();
    }

    void init()
    {
        player = FindObjectOfType<Player>();
    }

    public void updateHP()
    {
        int hp = (int)player.currentHP;
        if (hp < 0) hp = 0;
        else if (state.Count <= hp) hp = state.Count - 1; 

        foreach (GameObject o in state)
        {
            o.SetActive(false);
        }

        state[hp].SetActive(true);
    }
}
