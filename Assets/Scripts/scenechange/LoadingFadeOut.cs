using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingFadeOut : MonoBehaviour
{
    private float time;
    [SerializeField]
    private SpriteRenderer Loading;

    public void FadeOut()
    {
        StartCoroutine(FadeOutI());
    }

    public IEnumerator FadeOutI()
    {
        Destroy(gameObject, 3f);
        time = 0;
        while (time < 1f)
        {
            Loading.color = new Color(0, 0, 0, 1 - time);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    void Update()
    {
        time += Time.deltaTime;
    }
}
