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
            else
                c = sp.color;
         }
        else
        {
            c = image.color;
        }

    }

    void Start()
    {
        StartCoroutine(ShowWarning());
    }

    /*void Update()
    {
        if (image)
        {
            if (image.color.a < 0.7f)
            {
                c.a += Time.deltaTime / 0.2f;
                image.color = c;
            }
        }
        else if (sp)
        {
            if (sp.color.a < 0.7f)
            {
                c.a += Time.deltaTime / 0.2f;
                sp.color = c;
            }
        }
    }*/

    private IEnumerator ShowWarning()
    {
        Destroy(gameObject, 1f);
        const float ALPHA_RATE = 0.7f;
        float time = 0, alpha = 0;
        
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            alpha = time / 0.3f;
            c.a = alpha * ALPHA_RATE;

            if (image)  image.color = c;
            else if (sp)    sp.color = c;
            yield return null;
        }
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            alpha = (time - 0.3f) / -0.6f + 1f;
            c.a = alpha * ALPHA_RATE;

            if (image) image.color = c;
            else if (sp) sp.color = c;
            yield return null;
        }
        while (time < 1)
        {
            time += Time.deltaTime;
            alpha = (time - 0.2f) / 0.8f;
            c.a = alpha * ALPHA_RATE;

            if (image) image.color = c;
            else if (sp) sp.color = c;
            yield return null;
        }
    }
}
