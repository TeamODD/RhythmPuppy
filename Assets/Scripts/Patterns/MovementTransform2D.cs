using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTransform2D : MonoBehaviour
{
    [SerializeField]
    private float movespeed;
    [SerializeField]
    private Vector3 moveDirection;

    public Vector3 MoveDirection => moveDirection;

    private void Update()
    {
        transform.position += moveDirection * movespeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
