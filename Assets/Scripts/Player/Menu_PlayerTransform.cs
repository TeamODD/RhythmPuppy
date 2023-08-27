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
    public AudioSource volume;
    public UnityEvent PlayerOnPoint;
    public UnityEvent Loading;
    public Animator animator;
    public GameObject corgiLoading;

    private Vector3 endPoint;
    private Vector3 currentPosition;
    public static int currentIndex;
    public static int savingIndex;
    public static int clearIndex;
    [SerializeField]
    private float speed;
    private float time;
    private bool onInputDelay;

    void Start()
    {
        if (savingIndex != 0)
        {
            gameObject.transform.position = waypoints[savingIndex];
        }
        if (PlayerPrefs.HasKey("clearIndex"))
        {
            clearIndex = PlayerPrefs.GetInt("clearIndex");
        } else
        {
            clearIndex = 30;
        }
        onInputDelay = false;
        currentIndex = 0;
        
        currentPosition = transform.position; //플레이어 현재 위치
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Point();
    }
    void OnTriggerExit2D(Collider2D other)
    {
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
            savingIndex = currentIndex;
            onInputDelay = true; //연타 방지
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            animator.SetBool("WalkBool", true);
            StartCoroutine(move("Forward"));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (0 == currentIndex) return;
            savingIndex = currentIndex;
            --currentIndex;
            onInputDelay = true;
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex + 1, "disappear");
            animator.SetBool("WalkBool", true);
            //좌우반전 구현 필요
            StartCoroutine(move("Back"));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            onInputDelay = true;
            PlaySelectSound.instance.MenuSelectSound();
            StartCoroutine(LoadingScene());
        }
    }

    void Point()
    {
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
        time = 0;
        while (time < 1f)
        {
            LoadingScreenSprite.color = new Color(0, 0, 0, time);
            volume.volume = 1f - time;
            yield return new WaitForFixedUpdate();
        }
        LoadingScreenSprite.color = new Color(0, 0, 0, 1);
        Loading.Invoke();
        LoadingCanvas.alpha = 1;
        AudioListener.GetComponent<AudioListener>().enabled = false;
        volume.enabled = false;
        corgiLoading.gameObject.SetActive(true);
        PlaySelectSound.instance.StartLoading("SceneStage" + currentIndex/2, LoadingScreen);
        yield break;
    }

    void InputDelay()
    {
        onInputDelay = false;
    }
}
