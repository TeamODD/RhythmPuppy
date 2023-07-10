using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern10 : MonoBehaviour
{
    [SerializeField]
    private MovementTransform2D chestnut;
    [SerializeField]
    private GameObject chestnutProjectile;
    //-10<x<10까지 중에서 무작위로 낙하
    //낙하 1초 후 폭발하면서 8방향으로 가시 발사
    private void Bomb()
    {
        int count = 30;
        float intervalAngle = 360 / count;

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(ChestNutProjectile, ChestNut.transform.position, Quaternion.identity);

            //발사체 이동 방향(각도)
            float angle = intervalAngle * i; 
            //발사체 이동 방향(벡터)
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

            //발사체 이동 방향 설정
            clone.GetComponent<MovementTransform2D>().MoveTo(new Vector2(x, y));   
        }
    }
}
