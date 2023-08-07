using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] Image darkEffect;

    Color c;

    public void enableDarkEffect()
    {
        c = darkEffect.color;
        c.a = 0.8f;
        darkEffect.color = c;
    }

    public void disableDarkEffect()
    {
        c = darkEffect.color;
        c.a = 0f;
        darkEffect.color = c;
    }
}
