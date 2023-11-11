using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2_a : MonoBehaviour
{
    //3.2초 동안 굴러가도록
    private Rigidbody2D rb;
    [SerializeField]
    private float power;
    private float time;
    EventManager eventManager;

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.playerEvent.deathEvent += deathEvent;
        rb = gameObject.GetComponent<Rigidbody2D>();
        time = 0;
        Destroy(gameObject, 4.2f);
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            if (time <= 1.6f)
            {
                gameObject.transform.position += new Vector3(0, power * -1, 0) * Time.deltaTime;
            }
            gameObject.transform.position += new Vector3(power * -1.3f, 0, 0) * Time.deltaTime;
        }
    }

    private void deathEvent()
    {
        eventManager.playerEvent.deathEvent -= deathEvent;
        Destroy(gameObject);
    }
}
