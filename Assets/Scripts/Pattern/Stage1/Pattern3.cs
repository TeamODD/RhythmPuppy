using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Warning3;

public class Pattern3 : MonoBehaviour
{
    public Rigidbody2D rb;
    

    [SerializeField]
    private float power;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        gameObject.transform.position = new Vector3(xPosition, -10f, 0);
        Invoke("Jump", 0.9f);
        
    }
    void Jump()
    {
        rb.AddForce(Vector2.up * power, ForceMode2D.Impulse);
        Destroy(gameObject, 1f);
    }
}
