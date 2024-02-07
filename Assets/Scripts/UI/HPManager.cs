using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManagement
{
    public class HPManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> state;

        Player player;

        void Awake()
        {
            init();
        }
        void init()
        {
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            updateHP();
        }


        public void updateHP()
        {
            int hp = (int)player.currentHP;
            if (hp < 0) hp = 0;
            else if (state.Count < hp) hp = state.Count;

            for (int i = 0; i < state.Count; i++)
            {
                if (i == hp) state[i].SetActive(true);
                if (i != hp) state[i].SetActive(false);
            }
        }
    }
}