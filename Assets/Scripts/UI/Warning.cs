using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    Image image;
    Color c;

    void Awake()
    {
        image = GetComponent<Image>();
        Invoke("destroy", 1f);
    }

    void Update()
    {
        if (image.color.a < 0.7f)
        {
            c = image.color;
            c.a += Time.deltaTime / 0.2f;
            image.color = c;
        }
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}
