using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToNextPage : MonoBehaviour
{
    [SerializeField]
    GameObject HowToPlay;
    [SerializeField]
    GameObject UI1;
    [SerializeField]
    GameObject UI2;
    [SerializeField]
    GameObject LoadingScreen;
    SpriteRenderer LoadingScreenAlpha;
    // Start is called before the first frame update
    void Start()
    {
        HowToPlay.SetActive(true);
        UI1.SetActive(false);
        UI2.SetActive(false);
        LoadingScreenAlpha = LoadingScreen.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (HowToPlay.activeSelf)
        {
            HowToPlay.SetActive(false);
            UI1.SetActive(true);
            UI2.SetActive(false);
        }

        else if (UI1.activeSelf)
        {
            HowToPlay.SetActive(false);
            UI1.SetActive(false);
            UI2.SetActive(true);
        }

        else if (UI2.activeSelf)
        {
            StartCoroutine(Loading());
        }
    }
    IEnumerator Loading()
    {
        float alpha = 0;
        Save();
        while(LoadingScreenAlpha.color.a < 1)
        {
            LoadingScreenAlpha.color = new Color(0, 0, 0, alpha);
            alpha += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        LoadingScreenAlpha.color = new Color(0, 0, 0, 1);
        DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(1f); //2초후 로딩
        var mAsymcOperation = SceneManager.LoadSceneAsync("SceneMenu_01", LoadSceneMode.Single);
        LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        yield return mAsymcOperation;
        //여기서 끝(아래 실행 안 됨)
        mAsymcOperation = SceneManager.UnloadSceneAsync("Tutorials");
        yield return mAsymcOperation;
        yield break;
    }

    void Save() //클리어 스테이지 인덱스 저장 함수
    {
        if (Menu_PlayerTransform.clearIndex > 1) return;
        Menu_PlayerTransform.clearIndex = 2;
        PlayerPrefs.SetInt("clearIndex", 2);
    }
}
