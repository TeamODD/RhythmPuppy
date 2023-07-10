using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern10 : MonoBehaviour
{
    [SerializeField]
    private MovementTransform2D chestnut;
    [SerializeField]
    private GameObject chestnutProjectile;
    //-10<x<10���� �߿��� �������� ����
    //���� 1�� �� �����ϸ鼭 8�������� ���� �߻�
    private void Bomb()
    {
        int count = 30;
        float intervalAngle = 360 / count;

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(ChestNutProjectile, ChestNut.transform.position, Quaternion.identity);

            //�߻�ü �̵� ����(����)
            float angle = intervalAngle * i; 
            //�߻�ü �̵� ����(����)
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

            //�߻�ü �̵� ���� ����
            clone.GetComponent<MovementTransform2D>().MoveTo(new Vector2(x, y));   
        }
    }
}
