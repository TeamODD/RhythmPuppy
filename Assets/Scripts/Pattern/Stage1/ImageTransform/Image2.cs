using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image2 : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(-0.6f, 0, 0) * Time.deltaTime;
    }
}
