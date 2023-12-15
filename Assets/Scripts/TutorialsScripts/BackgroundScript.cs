using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField]
    GameObject BackGrounds;

    private float[] SpeedList;

    private void Start()
    {
        for (int i = 0; i < BackGrounds.transform.childCount; i++)
        {
            GameObject BackGround = BackGrounds.transform.GetChild(i).gameObject;
            SpriteRenderer spriteRenderer = BackGround.GetComponent<SpriteRenderer>();
            int LayerOrder = spriteRenderer.sortingOrder;
            float Speed = (float)LayerOrder / BackGrounds.transform.childCount;
            SpeedList = new float[BackGrounds.transform.childCount];
            SpeedList[i] = Speed;    

            StartCoroutine(Move(BackGround, Speed, i));

            GameObject NewBackGround = Instantiate(BackGround);
            NewBackGround.transform.position = BackGround.transform.position;
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            StartCoroutine(Move(NewBackGround, Speed, i));
        }
    }

    private IEnumerator Move(GameObject obj, float speed, int index)
    {
        while(obj.transform.position.x > -25f)
        {
            obj.transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
        }
        Copy(obj, speed, index);
        Destroy(obj);
        yield return null;
    }

    private void Copy(GameObject obj, float Speed, int index)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        GameObject NewBackGround = Instantiate(obj);
        NewBackGround.transform.position = obj.transform.position;
        NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2, 0f, 0f);
        StartCoroutine(Move(NewBackGround, Speed, index));
    }
}
