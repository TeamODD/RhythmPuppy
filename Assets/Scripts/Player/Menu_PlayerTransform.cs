using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_PlayerTransform : MonoBehaviour
{
    public Vector3[] waypoints;
    public GameObject SoundManager;

    private Vector3 currentPosition;
    public static int currentIndex;
    private ShowInfo showinfo;
    private Vector3 speed;
    private float time;

    void Start()
    {
        currentIndex = 0;
        currentPosition = transform.position; //플레이어 현재 위치

        time = 1.0f;
    }

    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            ++currentIndex;
            indexExceptionFunc();
            InputD(currentPosition, waypoints[currentIndex], time, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            --currentIndex;
            indexExceptionFunc();
            transform.position = Vector3.MoveTowards(currentPosition, waypoints[currentIndex], 6);
        }

        if (Vector3.Distance(waypoints[currentIndex], currentPosition) == 0)
        {
            //스테이지 도착시 곡 정보(노래 이름, 아티스트)를 표시
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("SceneStage" + (currentIndex + 1));
            }
            //스페이스바 입력시 스테이지 입장을 구현
        }

    }

    void InputD(Vector3 currentPosition, Vector3 waypoint, float time, GameObject Cogi)
    {
        Vector3 velocity = Vector3.zero;

        float offset = 0.01f;
        while (waypoint.x - offset > transform.position.x)
        {
            transform.position
                += new Vector3(0.2f, 0, 0) * Time.deltaTime;
            Debug.Log(transform.position.x);

            //Vector3.SmoothDamp(transform.position, waypoint, ref velocity, time);
        }
        transform.position = waypoint;

        //SoundManager.GetComponent<PlaySelectSound>().ChangeMusic(currentIndex);
        PlaySelectSound.instance.ChangeMusic(currentIndex);
    }

    private void indexExceptionFunc()
    {
        try
        {
            Debug.Log(waypoints[currentIndex]);
        }
        catch (System.IndexOutOfRangeException)
        {
            if (currentIndex < 0)
                ++currentIndex;
            else
                --currentIndex;
        } //인덱스 초과시에 예외 처리
    }
}
