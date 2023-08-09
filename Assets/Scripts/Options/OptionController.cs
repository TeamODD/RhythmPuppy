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
        //�ɼ� �������� ���� �ҷ����鼭 ���� ī�޶�� ����� ������, �̺�Ʈ �ý����� ��ġ��
        //������ �߻���������, �ɼ� ���������� ���� ī�޶�� ����� ������, �̺�Ʈ �ý�����
        //�������� ��ũ��Ʈ�Դϴ�. ������� ���ؼ� �� ��ũ��Ʈ�� �ִ� ������Ʈ�� ���ֽñ� �ٶ��ϴ�.

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
