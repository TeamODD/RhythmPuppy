using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    Player player;

    void Awake()
    {
        init();
    }

    void Update()
    {
        slider.value = player.stamina;
    }

    public void init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
