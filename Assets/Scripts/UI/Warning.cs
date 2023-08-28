using Cysharp.Threading.Tasks;
using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    Image image;
    SpriteRenderer sp;
    Color c;

    void Awake()
    {
        if (!TryGetComponent<Image>(out image))
        {
            if (!TryGetComponent<SpriteRenderer>(out sp)) 
                Destroy(gameObject);
         }

        StartCoroutine(destroy());
    }

    void Update()
    {
        if (image)
        {
            if (image.color.a < 0.7f)
            {
                c = image.color;
                c.a += Time.deltaTime / 0.2f;
                image.color = c;
            }
        }
        else if (sp)
        {
            if (sp.color.a < 0.7f)
            {
                c = sp.color;
                c.a += Time.deltaTime / 0.2f;
                sp.color = c;
            }
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
