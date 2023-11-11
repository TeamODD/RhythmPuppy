using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern4 : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float speed;
    public static float xPosition;
    EventManager eventManager;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        xPosition = Random.Range(-8.5f, 8.5f);
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.position = new Vector3(xPosition, 12f, 0);
    }
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0, -speed, 0) * Time.fixedDeltaTime;
        if (gameObject.transform.position.y <= -7)
        {
            Destroy(gameObject);
        }
    }

    void deathEvent()
    {
        Destroy(gameObject);
    }
}
