using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Warning3;

public class Pattern3 : MonoBehaviour
{
    private float time;
    public Rigidbody2D rb;
    public BoxCollider2D bc;

    public Sprite changeImg;
    public SpriteRenderer thisImg;
    EventManager eventManager;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.playerEvent.deathEvent += deathEvent;
        time = 0;
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

    void deathEvent()
    {
        eventManager.playerEvent.deathEvent -= deathEvent;
        Destroy(gameObject);
    }
}
