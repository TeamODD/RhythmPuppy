using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_PlayerTransform : MonoBehaviour
{
    
    public Vector3[] waypoints;
    public GameObject SoundManager;
    public GameObject BackGroundManager;

    private Vector3 endPoint;
    private Vector3 currentPosition;
    public static int currentIndex;
    [SerializeField]
    private float speed;
    private bool onInputDelay;

    void Start()
    {
        
        onInputDelay = false;
        currentIndex = 0;
        currentPosition = transform.position; //플레이어 현재 위치
    }

    void FixedUpdate()
    {
        if (onInputDelay)
            return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            ++currentIndex;
            onInputDelay = true; //연타 방지
            Invoke("InputDelay", 1f); //1초후에 연타 방지 끄도록
            if (waypoints.Length < currentIndex)
            {
                currentIndex--;
                return;
            }
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex, "appear");
            StartCoroutine(move("Forward"));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            --currentIndex;
            onInputDelay = true; //연타 방지
            Invoke("InputDelay", 1f); //1초후에 연타 방지 끄도록
            if (0 > currentIndex)
            {
                currentIndex++;
                return;
            }
            BackGroundManager.GetComponent<BackGroundManager>().backgroundAlpha(currentIndex + 1, "disappear");
            StartCoroutine(move("Back"));
        }

        if (Vector3.Distance(waypoints[currentIndex], currentPosition) == 0)
        {
            //스테이지 도착시 곡 정보(노래 이름, 아티스트)를 표시
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("SceneLoading");
            }
            //스페이스바 입력시 스테이지 입장을 구현
        }

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
                    yield return new WaitForEndOfFrame();
                }
                break;
            case "Back":
                endPoint = waypoints[currentIndex];
                while (transform.position.x > endPoint.x + offset)
                {
                    transform.position -= new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
                    yield return new WaitForEndOfFrame();
                }
                break;


        }
        transform.position = endPoint;
        //SoundManager.GetComponent<PlaySelectSound>().ChangeMusic(currentIndex);
        PlaySelectSound.instance.ChangeMusic(currentIndex);

        yield break;
    }

    void InputDelay()
    {
        onInputDelay = false;
    }
}
