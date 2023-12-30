using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using EventManagement;
using TMPro;
using SceneData;

public class GameClear : MonoBehaviour
{
    private Vector3 PuppyTransform;
    private SpriteRenderer Puppy;
    public Sprite HappyPuppy;
    private Collider2D Collider;
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
    private Image Screen_UI;
    [SerializeField]
    private SpriteRenderer Rank;
    [SerializeField]
    private Sprite[] RankImgs;
    [SerializeField]
    private TMP_Text RankText;
    private CanvasGroup canvas;

    //private AudioSource MusicManager;

    public static bool clear;
    private int deathcount;
    private bool S_Rank_True;
    EventManager eventManager;
    

    void Start()
    {
        //MusicManager = GameObject.FindWithTag("MusicManager").GetComponent<AudioSource>();
        corgi = GameObject.FindGameObjectWithTag("Player");
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
        //�뷡 ������ 2�� �� ���� ����(3�� ���� �� �Լ��� �θ��Ƿ�)
        yield return new WaitForSeconds(5f);
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
        //������� ����, ��Ʈ�� ��������.
        Rank.sprite = RankImgs[deathcount];
        if (S_Rank_True == true)
            Rank.sprite = RankImgs[3];
        //health �����ؼ� S ���� ���� if�� �ۼ��ϱ�

        byte RankAlpha = 0;
        while (Rank.color.a < 1 || RankAlpha < 255)
        {
            Rank.color = new Color(1, 1, 1, alpha);
            RankText.color = new Color32(255, 255, 255, RankAlpha);
            alpha += 0.01f;
            RankAlpha += 1;
            yield return new WaitForFixedUpdate();
        }
        alpha = 0;
        //������� ��ũ�� ��������.
        //
        /*���̵��� ���̵� �ƿ� ȿ���� UI������ UI image�� �⺻ ��������Ʈ �̹���
         * �� ������ Ȱ���Ͽ����ϴ�. */
        yield return new WaitForSeconds(5f);
        while (Screen_UI.color.a < 1 || ScreenAlpha.color.a < 1)
        {
            Screen_UI.color = new Color(0, 0, 0, alpha);
            ScreenAlpha.color = new Color(0, 0, 0, alpha);
            alpha += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        //�ε�â ����

        Save();

        DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f); //2���� �ε�
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

    void Save() //Ŭ���� �������� �ε��� ���� �Լ�
    {
        Scene scene = SceneManager.GetActiveScene();

        if (Menu_PlayerTransform.clearIndex > Menu_PlayerTransform.currentIndex + 2) return;
        Menu_PlayerTransform.clearIndex = Menu_PlayerTransform.currentIndex + 2;
        PlayerPrefs.SetInt("clearIndex", Menu_PlayerTransform.currentIndex + 2);

        //1�� �ϵ�, 0�� �븻(��ũ ���� �� ��)
        if (Menu_PlayerTransform.difficulty_num != 1) return;
        if (S_Rank_True == true)
        {
            PlayerPrefs.SetInt(scene.name, 3);
            return;
        }
        PlayerPrefs.SetInt(scene.name, deathcount);
    }

    void FixedUpdate()
    {
        if (clear)
        {
            Collider.enabled = true;
        }
    }
}
