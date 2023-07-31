using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private Sprite bgSprite;
    /*
    [SerializeField]
    private GameObject Img1;
    [SerializeField]
    private GameObject Img2;
    [SerializeField]
    private GameObject Img3;
    [SerializeField]
    private GameObject Img4;
    [SerializeField]
    private GameObject Img5;
    */

    private GameObject ImgName;
    [SerializeField]
    private GameObject parent;   
    
    GameObject[] tempImg = new GameObject[5];

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            ImgName = GameObject.Find("Img" + (i+1));
            imgRout(ImgName, i);
            StartCoroutine(imgRemove(ImgName, i, 2f));
        }
        /*
        imgRout(Img1, 0);
        StartCoroutine(imgRemove(Img1, 0, 2f));
        imgRout(Img2, 1);
        StartCoroutine(imgRemove(Img2, 1, 2f));
        imgRout(Img3, 2);
        StartCoroutine(imgRemove(Img3, 2, 2f));
        imgRout(Img4, 3);
        StartCoroutine(imgRemove(Img4, 3, 2f));
        imgRout(Img5, 4);
        StartCoroutine(imgRemove(Img5, 4, 2f));
        */
    }

    IEnumerator imgRemove(GameObject Img, int index, float calltime)
    {
        yield return new WaitForSeconds(calltime);
        //화면 밖의 좌표 -29까지 움직일시 오브젝트 삭제 후 클론 생성
        if (Img.transform.localPosition.x < -29f)
        {
            Destroy(Img);
            Img = tempImg[index];
            imgRout(Img, index);
        }
        StartCoroutine(imgRemove(Img, index, calltime));
    }
    void imgRout(GameObject Img, int index)
    {
        tempImg[index] = Instantiate(Img);
        tempImg[index].transform.SetParent(parent.transform, false);
        tempImg[index].transform.position = Img.transform.position;
        tempImg[index].transform.position += new Vector3(bgSprite.bounds.size.x - 0.04f, 0, 0);
        
    }

}
