using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 1f;
    
    private Rigidbody2D rig2D;

    // Start is called before the first frame update
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");

        transform.position += new Vector3(h * speed * Time.deltaTime, 0, 0);
    }

    private void jump()
    {
        rig2D.velocity = new Vector2(0, 0);
        rig2D.AddForce(new Vector2(0, jumpForce));
    }
}
