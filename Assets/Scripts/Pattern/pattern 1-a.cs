using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewBehaviourScript : MonoBehaviour
{
    private Vector3 xPosition;
    private float yPosition;
    

    void BeeAttack(yPosition)
    {
        transform.position = new Vector3(14, yPosition, 0);

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
