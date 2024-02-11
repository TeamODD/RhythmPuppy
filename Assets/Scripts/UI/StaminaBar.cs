using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] Slider slider;
        Player playerScript;

        void Awake()
        {
            playerScript = FindObjectOfType<Player>();
        }

        void Update()
        {
            slider.value = playerScript.currentStamina;
        }
    }
}