using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image3 : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(-0.4f, 0, 0) * Time.deltaTime;
    }
}