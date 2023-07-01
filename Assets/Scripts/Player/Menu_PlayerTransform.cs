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

        //������ �迭�� �� �Ҵ�
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
        } //�ε��� �ʰ��ÿ� ���� ó��
    }
    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position; //�÷��̾� ���� ��ġ
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentIndex;
            indexExceptionFunc();
            transform.position = Vector3.MoveTowards(currentPosition, waypoints[currentIndex], 6);
            //������ ����Ű �Է½� ĳ���͸� ���� ������������ �̵�
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
            //�������� ������ �� ����(�뷡 �̸�, ��Ƽ��Ʈ)�� ǥ���ϰ�
            //�����̽��� �Է½� �������� ������ ����
        }
    }
}
