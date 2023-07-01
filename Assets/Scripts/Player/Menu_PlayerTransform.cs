using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_PlayerTransform : MonoBehaviour
{
    public Vector3[] waypoints;
    private Vector3 currentPosition;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Vector3[3];

        //도착지 배열에 값 할당
        waypoints.SetValue(new Vector3(-6, 0, 0), 0);
        waypoints.SetValue(new Vector3(0, 0, 0), 1);

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
    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position; //플레이어 현재 위치
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentIndex;
            indexExceptionFunc();
            transform.position = Vector3.MoveTowards(currentPosition, waypoints[currentIndex], 6);
            //오른쪽 방향키 입력시 캐릭터를 다음 스테이지까지 이동
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentIndex;
            indexExceptionFunc();
            transform.position = Vector3.MoveTowards(currentPosition, waypoints[currentIndex], 6);
        }

        if (Vector3.Distance(waypoints[currentIndex], currentPosition) == 0f)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("SceneStage" + (currentIndex + 1));
            }
            //스테이지 도착시 곡 정보(노래 이름, 아티스트)를 표시하고
            //스페이스바 입력시 스테이지 입장을 구현
        }
    }
}
