using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    GameObject Option;

    public void onContinue()
    {
        Option.SetActive(false);
    }
}
