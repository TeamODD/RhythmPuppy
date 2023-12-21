using System;
using System.Collections;
using System.Reflection;
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
            if (i == 0)
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            }
            else if (i == 1)
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            }
            else if (i == 2)
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            }
            else if (i == 3)
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            }
            else if (i == 4) //�������� �ʴ� ��
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x, 0f, 0f);
            }
            else if (i == 5) //�� ����=33.79428f, ����=33.73f
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x - 0.06428f, 0f, 0f);
            }
            else if (i == 6) //���ϴ� ����x=33.79714f, ����x=27.85f
            {
                NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x - 5.94714f, 0f, 0f);
            }
             
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
        Debug.Log(spriteRenderer.bounds.size.x * 2);
        GameObject NewBackGround = Instantiate(obj);
        NewBackGround.transform.position = obj.transform.position;
        //������ ������ �˾Ƽ� ����� ��. �ʱ� ����� �޸� �ι�°���ʹ� �����̴� ���¿��� ����Ǿ��⿡ �߰� ������ �ʿ�.
        if (index == 0)
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.03f, 0f, 0f);
        }
        else if (index == 1)
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.03f, 0f, 0f);
        }
        else if (index == 2)
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.03f, 0f, 0f);
        }
        else if (index == 3)
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.03f, 0f, 0f);
        }
        else if (index == 4) //�������� �ʴ� ��
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.03f, 0f, 0f); 
        }
        else if (index == 5) //��
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 0.06428f - 0.03f, 0f, 0f);
        }
        else if (index == 6) //���ϴ� ����x=33.79714f, ����x=27.85f
        {
            NewBackGround.transform.position += new Vector3(spriteRenderer.bounds.size.x * 2 - 5.94714f - 0.03f, 0f, 0f);
        }
        StartCoroutine(Move(NewBackGround, Speed, index));
    }
}
