using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class Pattern1_b : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int dir;

    EventManager eventManager;
    private float time;
    public static float yPosition;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        time = 0;
        yPosition = Random.Range(-1.3f, 3.5f);
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(10f, yPosition, 0);
        Destroy(gameObject, 3f);
    }
    void FixedUpdate()
    {
        time += 1f * Time.deltaTime;

        if (time > 1f)
        {
            //가로 길이 18 세로 10
            if (time < 1.5f)
                transform.position += new Vector3(speed * dir, -speed * 0.3f, 0) * Time.deltaTime;
            else if (time < 2f)
                transform.position += new Vector3(speed * dir, speed * 0.6f, 0) * Time.deltaTime;
            else if (time < 2.5f)
                transform.position += new Vector3(speed * dir, -speed * 0.6f, 0) * Time.deltaTime;
            else if (time < 3f)
                transform.position += new Vector3(speed * dir, speed * 0.6f, 0) * Time.deltaTime;
        }
    }

    private void deathEvent()
    {
        eventManager.deathEvent -= deathEvent;
        Destroy(gameObject);
    }
}
