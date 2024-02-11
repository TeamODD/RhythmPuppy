using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning2_a : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 0);
        time = 0;

        //1초후 gameObject 삭제
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < 0.5f)
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, time / 1f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 1f - time / 1f);
    }
}
