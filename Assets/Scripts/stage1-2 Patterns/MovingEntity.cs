using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementTransform2D))]
public class MovingEntity : MonoBehaviour
{
    private MovementTransform2D movement2D;
    private Vector3 originPosition;
    private Vector3 originDirection;

    private void Awake()
    {
        movement2D = GetComponent<MovementTransform2D>();
        originPosition = transform.position;
        originDirection = movement2D.MoveDirection;
    }

    public void Reset()
    {
        movement2D.MoveTo(originDirection);
        transform.position = originPosition;
    }
}
