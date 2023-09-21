using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCorgiScript : MonoBehaviour
{
    [SerializeField]
    private int transparencyValue; // 0���� 255 ������ ���� ���� ������ ����

    private void Start()
    {
        // �ڽ� ������Ʈ�� �ִ� ��� ��������Ʈ �������� ������
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        // ��� ��������Ʈ �������� ������ ����
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;

            // ���� ���� 0���� 255 ���̷� ����
            transparencyValue = Mathf.Clamp(transparencyValue, 0, 255);

            // 0���� 255 ������ ���� 0���� 1 ������ �Ǽ��� ��ȯ
            float normalizedAlpha = transparencyValue / 255.0f;

            color.a = normalizedAlpha; // ���� �� ����
            renderer.color = color; // ����� ���� ����
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.NameToLayer("Obstacle").Equals(collision.gameObject.layer))
        {
            GameObject Tutorials2Manager = GameObject.Find("Tutorials2Manager");
            Tutorials2Manager.GetComponent<Tutorials2Manager>().IsFirstHited = true;
        }
    }
}
