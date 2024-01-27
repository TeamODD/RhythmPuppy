using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingFadeOut : MonoBehaviour
{
    [SerializeField]
    private Image Loading;
    [SerializeField]
    private CanvasGroup LoadingCanvas;
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
        if(GameObject.Find("corgiFace")) //이 if문은 메뉴로 돌아올시 메뉴인지 확인하기 위함
            gameObject.transform.position = new Vector3(Menu_PlayerTransform.corgi_posX, 2.49f, 0);
        if (a <= 0)
        {
            Destroy(gameObject);
            Destroy(LoadingCanvas.gameObject);
        }

        //gameObject.transform.position = new Vector3(0, 0, 0);
        LoadingCanvas.alpha = a;
        Loading.color = new Color(0, 0, 0, a);
        a -= Time.deltaTime * 0.5f; //2초
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
