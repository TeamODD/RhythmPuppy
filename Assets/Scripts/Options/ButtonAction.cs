using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Option;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onContinue()
    {
        Option.SetActive(false);
    }
}
