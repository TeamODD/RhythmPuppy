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
    public GameObject Select_Difficulty;
    public GameObject Normal;
    public GameObject Hard;
    public GameObject[] ParticleSystems;

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
    private float lastInputTime;
    private bool onInputDelay;
    public static bool ReadyToGoStage;
    public static bool IsPaused; //�ɼ�â���� Enter Ű �ߴ�
    public static int difficulty_num;

    private enum WorldStage
    {
        World1,
        World2
    };

    WorldStage world = WorldStage.World1;
   
    void Awake()
    {
        if (PlayerPrefs.HasKey("clearIndex"))
        {
            clearIndex = PlayerPrefs.GetInt("clearIndex");
        }
        else
        {
            clearIndex = 30; //���� ������ 1�� �ٲ��ּ���.
        }
        //��������->�� ���ƿ��� �� ��ġ, �ε��� ���� 
        if (savingIndex != 0)
        {
            gameObject.transform.position = waypoints[savingIndex];
            currentIndex = savingIndex;
        }
        else
            currentIndex = 0;

        //�޴��� ���ƿ��� �� �ε����� ���� ����� ��Ÿ������.
        if (savingIndex >= 2)
            BackGroundManager.GetComponent<BackGroundManager>().backgroundReset(2, "appear");
        if (savingIndex >= 7)
        {
            BackGroundManager.GetComponent<BackGroundManager>().backgroundReset(7, "appear");
            BackGroundManager.GetComponent<BackGroundManager>().backgroundReset(2, "appear");
        }

        corgi_posX = waypoints[currentIndex].x;
    }

    void Start()
    {
        difficulty_num = 0;
        GameObject.Find("MusicSoundManager").GetComponent<AudioSource>().Play();
        foreach (GameObject ParticleSystem in ParticleSystems)
        {
            ParticleSystem.GetComponent<ParticleSystem>().Stop();
        }
        
        ReadyToGoStage = false;
        IsPaused = false;
        onInputDelay = false;
        currentPosition = transform.position; //�÷��̾� ���� ��ġ
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        savingIndex = currentIndex;
        PlayerOnPoint.Invoke();
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerOnPointExceptMusicChange.Invoke();
        if (currentIndex < 7)
        {
            PlayerWalkingSound.instance.World1_Walking();
            world = WorldStage.World1;
        }
        else
        {
            PlayerWalkingSound.instance.World2_Walking();
            world = WorldStage.World2;
        }
    }
    
    void DifficultyOff()
    {
        ReadyToGoStage = false;
        onInputDelay = false;
    }

    void Update()
    {
        foreach (GameObject ParticleSystem in ParticleSystems)
        {
            ParticleSystem.transform.position = new Vector3(transform.position.x, -0.97f, transform.position.z);
        }

        int offset = 1;
        time += Time.deltaTime;
        if (onInputDelay || IsPaused) return;

        //���̵� ����â ��ũ��Ʈ.
        if (ReadyToGoStage)
        {
            //normal�� 0���� hard�� 1
            if (Input.GetKeyDown(KeyCode.A))
            {
                Normal.SetActive(true);
                Hard.SetActive(false);
                difficulty_num = 0;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Normal.SetActive(false);
                Hard.SetActive(true);
                difficulty_num = 1;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Select_Difficulty.SetActive(false);
                difficulty_num = 0;
                onInputDelay = true;
                Invoke("DifficultyOff", 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (ReadyToGoStage) return;
            if (waypoints.Length == currentIndex + offset || currentIndex + offset > clearIndex) return;
            ++currentIndex;
            onInputDelay = true; //��Ÿ ����
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            animator.SetBool("WalkBool", true);
            StartCoroutine(move("Forward"));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (ReadyToGoStage) return;
            if (0 == currentIndex) return;
            --currentIndex;
            onInputDelay = true;
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex + 1, "disappear");
            animator.SetBool("WalkBool", true);
            //�¿���� ���� �ʿ�
            StartCoroutine(move("Back"));
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(currentIndex == 1) //Ʃ�丮���� �׳� �����ϵ���
            {
                onInputDelay = true;
                PlaySelectSound.instance.MenuSelectSound();
                GetSceneString();
                if (SceneName == null) return;
                StartCoroutine(LoadingScene());
                return;
            }

            switch (ReadyToGoStage)
            {
                case false:
                    GetSceneString();
                    if (SceneName == null) return;
                    Select_Difficulty.SetActive(true);
                    ReadyToGoStage = true;
                    break;

                case true:
                    onInputDelay = true;
                    PlaySelectSound.instance.MenuSelectSound();
                    StartCoroutine(LoadingScene());
                    break;
            }
        }      
    }

    IEnumerator move(string s)
    {
        float offset = 0.01f;
        if (world == WorldStage.World1)
            ParticleSystems[0].GetComponent<ParticleSystem>().Play();
        else if (world == WorldStage.World2)
            ParticleSystems[1].GetComponent<ParticleSystem>().Play();

        switch (s)
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
        foreach (GameObject ParticleSystem in ParticleSystems)
        {
            ParticleSystem.GetComponent<ParticleSystem>().Stop();
        }

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
            case 1: SceneName = "Tutorials2";
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
                SceneName = "SceneStage2_2";
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
