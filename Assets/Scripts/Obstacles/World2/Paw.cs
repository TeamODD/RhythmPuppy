using Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    float rotated, r;

    void Awake()
    {
        rotated = 0f;
    }

    void FixedUpdate()
    {
        r = rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(new Vector3(0, 0, -1 * r));
        rotated += r;
        if (140 < rotated) Destroy(gameObject);
    }

}
