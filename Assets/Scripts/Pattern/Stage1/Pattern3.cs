using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Warning3;

public class Pattern3 : MonoBehaviour
{
    private float time;
    public Rigidbody2D rb;
    [SerializeField]
    private float speed;

    void Awake()
    {
        time = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(xPosition, -10f, 0);
        Destroy(gameObject, 1.5f);
    }
    void FixedUpdate()
    {
        time += 1f*Time.deltaTime;
        if(gameObject.transform.position.y < - 7.83)
        {
            gameObject.transform.position += new Vector3(0, 6f, 0) * Time.deltaTime;
        }
        if (time > 0.5f)
        {
            if (gameObject.transform.position.y < -2.87)
                gameObject.transform.position += new Vector3(0, 50f, 0) * Time.deltaTime;
        }
        if (time > 1f)
        {
            gameObject.transform.position += new Vector3(0, -70f, 0) * Time.deltaTime;
        }
    }
}
