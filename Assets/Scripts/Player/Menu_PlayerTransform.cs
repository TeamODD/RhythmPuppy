using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu_PlayerTransform : MonoBehaviour
{
    public Vector3[] waypoints;
    public GameObject AudioListener;
    public GameObject BackGroundManager;
    public SpriteRenderer LoadingScreenSprite;
    public GameObject LoadingScreen;
    public CanvasGroup LoadingCanvas;
    public CanvasGroup DefaultCanvas;
    public AudioSource volume;
    public UnityEvent PlayerOnPoint;
    public UnityEvent PlayerOnPointExceptMusicChange;
    public UnityEvent Loading;
    public Animator animator;
    public GameObject corgiLoading;

    private Vector3 endPoint;
    private Vector3 currentPosition;
    public static int currentIndex;
    public static int savingIndex;
    public static int clearIndex;
    public static float corgi_posX;
    private string SceneName;
    [SerializeField]
    private float speed;
    private float time;
    private bool onInputDelay;

    void Awake()
    {
        if (PlayerPrefs.HasKey("clearIndex"))
        {
            clearIndex = PlayerPrefs.GetInt("clearIndex");
        }
        else
        {
            clearIndex = 30; //개발 끝나면 1로 바꿔주세요.
        }

        if (savingIndex != 0)
        {
            gameObject.transform.position = waypoints[savingIndex];
            currentIndex = savingIndex;
            if (savingIndex >= 2) //메뉴로 돌아왔을시 월드2라면 월드2배경을 띄움.
                BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            else if(savingIndex >= 7) //메뉴로 돌아왔을시 월드1이라면 월드1배경을 띄움.
                BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
        }
        else
            currentIndex = 0;

        corgi_posX = waypoints[currentIndex].x;
    }

    void Start()
    {
        GameObject.Find("SoundManager").GetComponent<AudioSource>().Play();
        onInputDelay = false;

        currentPosition = transform.position; //플레이어 현재 위치
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Point();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerOnPointExceptMusicChange.Invoke();
        if (currentIndex < 7)
            PlaySelectSound.instance.World1_Walking();
        else
            PlaySelectSound.instance.World2_Walking();
    }

    void Update()
    {
        int offset = 1;
        time += Time.deltaTime;
        if (onInputDelay) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (waypoints.Length == currentIndex + offset || currentIndex + offset > clearIndex) return;
            ++currentIndex;
            onInputDelay = true; //연타 방지
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            animator.SetBool("WalkBool", true);
            StartCoroutine(move("Forward"));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (0 == currentIndex) return;
            --currentIndex;
            onInputDelay = true;
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex + 1, "disappear");
            animator.SetBool("WalkBool", true);
            //좌우반전 구현 필요
            StartCoroutine(move("Back"));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetSceneString();
            if (SceneName == null) return;
            onInputDelay = true;
            PlaySelectSound.instance.MenuSelectSound();
            StartCoroutine(LoadingScene());
        }
    }

    void Point()
    {
        savingIndex = currentIndex;
        PlayerOnPoint.Invoke();
    }

    IEnumerator move(string s)
    {
        float offset = 0.01f;
        
        switch(s)
        {
            case "Forward":
                endPoint = waypoints[currentIndex];
                while(transform.position.x < endPoint.x - offset)
                {
                    transform.position += new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                onInputDelay = false;
                break;
            case "Back":
                endPoint = waypoints[currentIndex];
                while (transform.position.x > endPoint.x + offset)
                {
                    transform.position -= new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                onInputDelay = false;
                break;
        }
        transform.position = endPoint;
        animator.SetBool("WalkBool", false);

        yield break;
    }

    IEnumerator LoadingScene()
    {
        DefaultCanvas.alpha = 0;
        Loading.Invoke();
        time = 0;
        while (time < 1f)
        {
            LoadingScreenSprite.color = new Color(0, 0, 0, time);
            LoadingCanvas.alpha = time;
            volume.volume = 1f - time;
            yield return new WaitForFixedUpdate();
        }
        LoadingScreenSprite.color = new Color(0, 0, 0, 1);
        LoadingCanvas.alpha = 1;
        
        AudioListener.GetComponent<AudioListener>().enabled = false;
        volume.enabled = false;
        corgiLoading.gameObject.SetActive(true);
        PlaySelectSound.instance.StartLoading(SceneName, LoadingScreen);
        yield break;
    }
    void GetSceneString()
    {
        switch(currentIndex)
        {
            case 1: SceneName = "Tutorials";
                break;
            case 2: SceneName = "SceneStage1_1";
                break;
            case 4:
                SceneName = "SceneStage1_2";
                break;
            case 6:
                SceneName = "SceneStage1_3";
                break;
            case 8:
                SceneName = "SceneStage2_1";
                break;
            case 10:
                break;
            default:
                SceneName = null;
                break;
        }
    }
    void InputDelay()
    {
        onInputDelay = false;
    }
}
