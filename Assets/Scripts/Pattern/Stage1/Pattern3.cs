using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Warning3;

public class Pattern3 : MonoBehaviour
{
    private float time;
    public Rigidbody2D rb;
    
    public Sprite changeImg;
    public SpriteRenderer thisImg;

    void Awake()
    {
        time = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(xPosition, -6f, 0);
        Invoke("ChangeImg", 1.5f);
        Destroy(gameObject, 2.5f);
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            if (gameObject.transform.position.y < -3.94)
            {
                gameObject.transform.position += new Vector3(0, 6f, 0) * Time.deltaTime;
            }
            if (time > 1.5f)
            {
                if (gameObject.transform.position.y < -2.7)
                    gameObject.transform.position += new Vector3(0, 60f, 0) * Time.deltaTime;
            }
            if (time > 2f)
            {
                gameObject.transform.position += new Vector3(0, -80f, 0) * Time.deltaTime;
            }
        }
    }
    void ChangeImg()
    {
        thisImg.sprite = changeImg;
    }
}
