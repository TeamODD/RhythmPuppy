using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using EventManagement;
using SceneData;

public class GameClear : MonoBehaviour
{
    private Vector3 PuppyTransform;
    private SpriteRenderer Puppy;
    public Sprite HappyPuppy;
    private Collider2D Collider;
    [SerializeField]
    private GameObject corgi;
    private Vector3 CorgiTransform;
    [SerializeField]
    private GameObject corgiEnd;
    [SerializeField]
    private SpriteRenderer spot1;
    [SerializeField]
    private SpriteRenderer spot2;
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private SpriteRenderer heartAlpha;
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private SpriteRenderer ScreenAlpha;
    [SerializeField]
    private SpriteRenderer Rank;
    [SerializeField]
    private Sprite[] RankImgs;
    [SerializeField]
    private CanvasGroup canvas;

    public static bool clear;
    private int deathcount;
    private bool S_Rank_True;
    EventManager eventManager;
    

    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.stageEvent.clearEvent += Clear;
        clear = false;
        Collider = gameObject.GetComponent<CircleCollider2D>();
        PuppyTransform = gameObject.transform.position;
        Puppy = gameObject.GetComponent<SpriteRenderer>();
        CorgiTransform = corgi.transform.position;
    }
    /*public void CommingOutFunc(float WaitTime, float StartTime)*/
    public void CommingOutFunc()
    {
        StartCoroutine(CommingOut());
    }
    /*IEnumerator CommingOut(float WaitTime, float StartTime)*/
    IEnumerator CommingOut()
    {
        float speed = 0.1f;
        //노래 끝나고 3초 후 퍼피 등장
        /*yield return new WaitForSeconds(WaitTime - StartTime + 3f);*/
        while(gameObject.transform.position.x > 3.5f)
        {
            gameObject.transform.position -= new Vector3(speed, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        clear = true;
        yield break;
    }   
    IEnumerator Moving()
    {
        float alpha = 0;
        gameObject.transform.position = new Vector3(3,-3.25f,0);
        corgi.SetActive(false);
        corgiEnd.transform.position = new Vector3(-3,-4.3f,0);
        yield return new WaitForSeconds(1f);
        while(gameObject.transform.position.x > 1)
        {
            gameObject.transform.position += new Vector3(-0.02f, 0, 0);
            corgiEnd.transform.position += new Vector3(0.02f, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        /*corgi.transform.position = new Vector3(-1,-4.3f,0);
        corgi.GetComponent<Player>().enabled = false;
        corgi.GetComponent<Animator>().enabled = false;*/
        while (spot1.color.a < 1)
        {
            spot1.color = new Color(1, 1, 1, alpha);
            spot2.color = new Color(1, 1, 1, alpha);
            heartAlpha.color = new Color(1, 1, 1, alpha);
            heart.transform.position += new Vector3(0, 0.02f, 0);
            alpha += 0.01f;
            yield return new WaitForEndOfFrame();
        }
        alpha = 0;
        //여기까지 암전, 하트가 나오도록.
        Rank.sprite = RankImgs[deathcount];
        if (S_Rank_True == true)
            Rank.sprite = RankImgs[3];
        //health 관련해서 S 판정 내는 if문 작성하기
        while (Rank.color.a < 1)
        {
            Rank.color = new Color(1, 1, 1, alpha);
            canvas.alpha = alpha;
            alpha += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        alpha = 0;
        //여기까지 랭크가 나오도록.
        yield return new WaitForSeconds(5f);
        while (ScreenAlpha.color.a < 1)
        {
            ScreenAlpha.color = new Color(0, 0, 0, alpha);
            alpha += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        //로딩창 등장

        Save();

        DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f); //2초후 로딩
        var mAsymcOperation = SceneManager.LoadSceneAsync(SceneInfo.getSceneName(SceneName.STAGEMENU), LoadSceneMode.Single);
        LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        yield return mAsymcOperation;

        /*LoadingScreen.transform.position = new Vector3(0, 0, 0);
        Debug.Log("FadeOut");
        mAsymcOperation = SceneManager.UnloadSceneAsync("SceneStage1");
        Debug.Log("UnLoad Default Scene");
        yield return mAsymcOperation;*/
    }


    /*void OnTriggerEnter2D(Collider2D other)
    {
        Clear();
    }*/
    void Clear()
    {
        S_Rank_True = GameObject.Find("corgi").GetComponent<Player>().S_Rank_True;
        deathcount = (int)GameObject.Find("corgi").GetComponent<Player>().deathCount;
        Puppy.sprite = HappyPuppy;
        StartCoroutine(Moving());
    }

    void Save() //클리어 스테이지 인덱스 저장 함수
    {
        if (Menu_PlayerTransform.clearIndex > 4) return;
        Menu_PlayerTransform.clearIndex = 4;
        PlayerPrefs.SetInt("clearIndex", 4);
        if(S_Rank_True == true)
        {
            PlayerPrefs.SetInt("1-1", 3);
            return;
        }
        PlayerPrefs.SetInt("1-1", deathcount);
    }

    void FixedUpdate()
    {
        if (clear)
        {
            Collider.enabled = true;
        }
    }
}
