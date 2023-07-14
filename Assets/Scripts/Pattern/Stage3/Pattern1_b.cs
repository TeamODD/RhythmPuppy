using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1_b : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int dir;

    public float yPosition;

    void Awake()
    {
        yPosition = Random.Range(-3f, 2f);
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(14, yPosition, 0);
    }
    void Update()
    {
        //가로 길이 18 세로 10
        gameObject.transform.position += new Vector3(speed * dir * 2f, 0 , 0) * Time.deltaTime;
        if (gameObject.transform.position.x <= 9)
            transform.position += new Vector3(speed * dir, speed * 2, 0) * Time.deltaTime;
        if (gameObject.transform.position.x <= 4.5)
            transform.position += new Vector3(speed * dir, -speed * 4, 0) * Time.deltaTime;
        if (gameObject.transform.position.x <= 0)
            transform.position += new Vector3(speed * dir, speed * 4, 0) * Time.deltaTime;
        if (gameObject.transform.position.x <= -4.5)
            transform.position += new Vector3(speed * dir, -speed * 4, 0) * Time.deltaTime;
        if (gameObject.transform.position.x <= -9)
            transform.position += new Vector3(speed * dir, speed, 0) * Time.deltaTime;


        if (gameObject.transform.position.x <= -14)
            Destroy(gameObject);
    }
}
