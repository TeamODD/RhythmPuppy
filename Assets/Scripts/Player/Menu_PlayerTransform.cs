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

    private Vector3 endPoint;
    private Vector3 currentPosition;
    public static int currentIndex;
    [SerializeField]
    private float speed;
    private float time;
    private bool onInputDelay;

    void Start()
    {
        
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
        Point();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (onInputDelay) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            ++currentIndex;
            onInputDelay = true; //연타 방지
            if (waypoints.Length < currentIndex)
            {
                currentIndex--;
                onInputDelay = false;
                return;
            }
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            StartCoroutine(move("Forward"));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            --currentIndex;
            onInputDelay = true;
            if (0 > currentIndex)
            {
                currentIndex++;
                onInputDelay = false;
                return;
            }
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex + 1, "disappear");
            StartCoroutine(move("Back"));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            onInputDelay = true;
            StartCoroutine(LoadingScene());
            
        }
    }

    void Point()
    {
        PlayerOnPoint.Invoke();
    }

    IEnumerator move(string s)
    {
        Vector3 velocity = Vector3.zero;
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
        LoadingCanvas.alpha = 1;
        AudioListener.GetComponent<AudioListener>().enabled = false;
        volume.enabled = false;
        PlaySelectSound.instance.StartLoading("SceneStage" + currentIndex/2, LoadingScreen);
        yield break;
    }

    void InputDelay()
    {
        onInputDelay = false;
    }
}
