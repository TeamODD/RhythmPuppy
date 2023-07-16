using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pattern1_b;

public class Warning1_b : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        gameObject.transform.position = new Vector3(15.5f, yPosition, 0);
        GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 0);

    }

    // Update is called once per frame
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
