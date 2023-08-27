using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1_a : MonoBehaviour
{
    [SerializeField]
    float speed = 3.5f;
    [SerializeField]
    private int dir = -1;

    EventManager eventManager;
    private float time;
    public static float yPosition;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.deathEvent += deathEvent;
        time = 0;
        yPosition = Random.Range(-2f, 4.5f);
    }
    void Start()
    {
        gameObject.transform.position = new Vector3(10, yPosition, 0);
    }
    // FixedUpdate로 변경해야 할 수도 있음.
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            transform.position += new Vector3(speed * dir, 0, 0) * Time.fixedDeltaTime;
            if (gameObject.transform.position.x <= -15)
                Destroy(gameObject);
        }
        
    }

    private void deathEvent()
    {
        eventManager.deathEvent -= deathEvent;
        Destroy(gameObject);
    }
    //재욱 형님 코드 가지고 온 거(플레이어 충돌 코드 같길래 붙여넣기만 함, 수정 안 함)
    /*void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.SendMessage("getDamage");
            Destroy(gameObject);
        }
        Debug.Log("hi");
    }*/
}
