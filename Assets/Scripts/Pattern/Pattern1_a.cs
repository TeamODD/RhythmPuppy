using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1_a : MonoBehaviour
{
    float speed;
    private int dir;

    public static float yPosition;

    void Awake()
    {
        speed = 0.7f;
        dir = -1;
        yPosition = Random.Range(-3.5f, 5.0f);
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(14, yPosition, 0);
    }
    // FixedUpdate�� �����ؾ� �� ���� ����.
    void Update()
    {
        transform.position += new Vector3(speed * dir, 0, 0) * Time.fixedDeltaTime;
        if (gameObject.transform.position.x <= -15)
            Destroy(gameObject);
        
    }
    //��� ���� �ڵ� ������ �� ��(�÷��̾� �浹 �ڵ� ���淡 �ٿ��ֱ⸸ ��, ���� �� ��)
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().getDamage(1);
            Destroy(gameObject);
        }
        Debug.Log("hi");
    }
}
