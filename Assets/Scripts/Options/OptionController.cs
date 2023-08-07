using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField]
    GameObject eventsystem;
    [SerializeField]
    GameObject maincamera;

    // Start is called before the first frame update
    void Start()
    {
        //옵션 스테이지 씬이 불러오면서 메인 카메라와 오디오 리스닝, 이벤트 시스템이 겹치는
        //문제가 발생했음으로, 옵션 스테이지의 메인 카메라와 오디오 리스닝, 이벤트 시스템을
        //꺼버리는 스크립트입니다. 디버깅을 위해선 이 스크립트가 있는 오브젝트를 꺼주시길 바랍니다.

        if (eventsystem != null && eventsystem.activeSelf == true)
        {
            eventsystem.SetActive(false);
        }

        if (maincamera != null && maincamera.activeSelf == true)
        {
            maincamera.SetActive(false);
        }
    }
}
