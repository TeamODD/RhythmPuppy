using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2_b : MonoBehaviour
{
    //3.2초 동안 굴러가도록
    private Rigidbody2D rb;
    [SerializeField]
    private float power;
    private float RotateSpeed;
    private float time;

    void Awake()
    {
        time = 0;
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        RotateSpeed = 540;
        //1초 동안 맵 밖에서 굴러가서 경고문보다 1초 늦게 도착 후 4초 동안 맵 안에서 굴러감.
        Destroy(gameObject, 4.5f);
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            gameObject.transform.position += new Vector3(power * -1, 0, 0) * Time.deltaTime;
            //gameObject.transform.Rotate(0, 0, Time.deltaTime * RotateSpeed, Space.Self);
        }
    }
}