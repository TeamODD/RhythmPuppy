using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2_b : MonoBehaviour
{
    //3.2�� ���� ����������
    private Rigidbody2D rb;
    [SerializeField]
    private float power;
    private float RotateSpeed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        RotateSpeed = 540;
        //1�� ���� �� �ۿ��� �������� ������� 1�� �ʰ� ���� �� 4�� ���� �� �ȿ��� ������.
        Destroy(gameObject, 5);
    }

    void Update()
    {
        gameObject.transform.position += new Vector3(power * -1, 0, 0) * Time.deltaTime;
        gameObject.transform.Rotate(0, 0, Time.deltaTime * RotateSpeed, Space.Self);
    }
}