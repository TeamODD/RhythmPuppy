using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] GameObject screenEffect;

    GameObject darkObj, redObj;
    Image dark, red;
    Color c;

    void Awake()
    {
        init();
    }

    public void init()
    {
        darkObj = Instantiate(screenEffect);
        darkObj.transform.SetParent(transform);
        dark = darkObj.GetComponent<Image>();
        dark.color = new Color(0, 0, 0, 0);

        redObj = Instantiate(screenEffect);
        redObj.transform.SetParent(transform);
        red = redObj.GetComponent<Image>();
        red.color = new Color(255, 0, 0, 0);
    }

    public void enableDarkEffect()
    {
        c = dark.color;
        c.a = 0.8f;
        dark.color = c;
    }

    public void disableDarkEffect()
    {
        c = dark.color;
        c.a = 0f;
        dark.color = c;
    }

    public void hitEffect()
    {
        c = red.color;
        c.a = 0.8f;
        red.color = c;
        StartCoroutine(runHitEffect());
    }

    private IEnumerator runHitEffect()
    {
        c = red.color;
        c.a = 0f;
        red.color = c;
        while(c.a < 0.6f)
        {
            c.a += 0.1f;
            red.color = c;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.1f);
        while (0 < c.a)
        {
            c.a -= 0.1f;
            red.color = c;
            yield return new WaitForSeconds(0.02f);
        }
        c.a = 0;
    }
}
