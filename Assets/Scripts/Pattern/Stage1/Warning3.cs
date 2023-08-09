using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning3 : MonoBehaviour
{
    public static float xPosition;
    private float time;
    void Awake()
    {
        xPosition = Random.Range(-8.5f, 8.5f);
    }
    void Start()
    {
        time = 0;
        gameObject.transform.position = new Vector3(xPosition, -4f, 0);
        GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 0);

    }
    void Update()
    {
        time += Time.deltaTime;
        if (time < 0.5f)
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, time / 1f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 1f-time / 1f);

        //1초후 gameObject 삭제
        Destroy(gameObject, 1f);
    }
}
