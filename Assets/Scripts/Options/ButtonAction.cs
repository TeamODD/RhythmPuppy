using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonAction : MonoBehaviour
{
    public GameObject target;
    public GameObject Warning;
    private int count;

    private Pattern1_a Pattern1_a;
    private float yPosition;

    void Start()
    {
        //Pattern1_a.BeeMove();
    }
    
    void Awake()
    {
        Bee();
        count = 1;
    }
    void Bee()
    {
        Instantiate(target);
        Instantiate(Warning);
        
        count++;
        Invoke("Bee", 0.5f);
    }
    void Update()
    {
        
    }

    public void onContinue()
    {
        //Option.SetActive(false);

        if (count == 32)
            CancelInvoke("Bee");
    }
    //처음 두 마리 붙어서 나옴(가로)
}
