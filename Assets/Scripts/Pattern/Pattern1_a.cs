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
    // FixedUpdate로 변경해야 할 수도 있음.
    void Update()
    {
        transform.position += new Vector3(speed * dir, 0, 0) * Time.fixedDeltaTime;
        if (gameObject.transform.position.x <= -15)
            Destroy(gameObject);
        
    }
    //재욱 형님 코드 가지고 온 거(플레이어 충돌 코드 같길래 붙여넣기만 함, 수정 안 함)
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
