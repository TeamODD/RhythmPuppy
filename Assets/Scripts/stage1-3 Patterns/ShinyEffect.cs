using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShinyEffect : MonoBehaviour
{
    RectTransform rTransform;
    Image image;
    float rotateSpeed, fadeinSpeed;

    void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        rotateSpeed = 1.5f;
        fadeinSpeed = 2f;
    }

    void OnEnable()
    {
        StartCoroutine(fadein());
        StartCoroutine(disableCoroutine());
    }

    void Update()
    {
        rTransform.Rotate(new Vector3(0, 0, 36 * rotateSpeed * Time.deltaTime));
    }

    IEnumerator fadein()
    {
        float time = 0;

        setImageAlpha(image, 0);
        while (time < 1)
        {
            time += Time.deltaTime;
            setImageAlpha(image, time * fadeinSpeed);
            yield return null;
        }
        setImageAlpha(image, 1);
    }

    IEnumerator disableCoroutine()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void setImageAlpha(Image image, float a)
    {
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}
