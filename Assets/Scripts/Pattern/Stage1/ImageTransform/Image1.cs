using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image1 : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(-1.15f, 0, 0) * Time.deltaTime;
        
    }
}
