using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1_a : MonoBehaviour
{
    private Vector3 xPosition;
    private float yPosition;

    public void BeeMove()
    {
        this.transform.position = new Vector3(14, 0, 0);
        this.transform.position += new Vector3(-14, 0, -2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
