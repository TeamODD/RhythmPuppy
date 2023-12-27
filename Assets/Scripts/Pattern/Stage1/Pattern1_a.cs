using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1_a : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    private int dir = -1;
    [SerializeField]
    private float XPosition;

    EventManager eventManager;
    ObjectPoolManager PoolingManager;
    [HideInInspector]
    public float time;
    public static float yPosition;
    [HideInInspector]
    public bool IsPooled = false;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        PoolingManager = FindObjectOfType<ObjectPoolManager>();
        eventManager.playerEvent.deathEvent += deathEvent;
        time = 0;
        yPosition = Random.Range(-2f, 4.5f);
    }
    void Start()
    {
        if(!IsPooled)
        {
            gameObject.transform.position = new Vector3(XPosition, yPosition, 0);
        }
    }
    // FixedUpdate로 변경해야 할 수도 있음.
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            transform.position += new Vector3(speed * dir, 0, 0) * Time.fixedDeltaTime;
            if (gameObject.transform.position.x <= -10.5f)
                DestroyObject();
        }
        
    }

    private void DestroyObject()
    {
        if (IsPooled)
            PoolingManager.ReleaseObject();
        else
            Destroy(gameObject);

    }
    private void deathEvent()
    {
        eventManager.playerEvent.deathEvent -= deathEvent;
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
