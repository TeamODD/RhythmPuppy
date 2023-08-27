using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingFadeOut : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer Loading;
    bool isSceneLoaded;
    float a;

    void Awake()
    {
        a = 1f;
        isSceneLoaded = false;
    }

    void Update()
    {
        if (!isSceneLoaded) return;
        if (a <= 0) Destroy(gameObject);

        Loading.color = new Color(0, 0, 0, a);
        a -= Time.deltaTime * 0.3f;
    }

    public void FadeOut()
    {
        isSceneLoaded = true;
    }

    /*public IEnumerator FadeOutI()
    {
        WaitUntil w = new WaitUntil(() => !operation.isDone);
        yield return w;
        c = null;
    }*/
}
