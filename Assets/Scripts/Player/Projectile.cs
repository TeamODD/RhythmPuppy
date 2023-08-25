using System.Collections;
using UnityEngine;
using static EventManager.PlayerEvent;

public class Projectile : MonoBehaviour
{
    struct ProjectileData
    {
        readonly public float rad { get; }
        readonly public float correctFactor { get; }

        public ProjectileData(float rad, float corrFactor)
        {
            this.rad = rad;
            this.correctFactor = corrFactor;
        }
    }

    EventManager eventManager;
    Camera mainCamera;
    Player player;
    Transform neck, head;
    Vector3 dir;
    ProjectileData data;

    void Awake()
    {
        init();
    }

    public void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        mainCamera = Camera.main;
        player = transform.parent.GetComponent<Player>();
        neck = player.transform.Find("bone_2/neck");
        head = player.transform.Find("bone_2/neck/head");
        dir = Vector3.zero;

        float rad, correctFactor;
        rad = (neck.position - transform.position).magnitude;
        correctFactor = 16f;
        data = new ProjectileData(rad, correctFactor);

        eventManager.playerEvent.shootEvent += shootEvent;
        eventManager.playerEvent.teleportEvent += stop;
        eventManager.playerEvent.shootCancelEvent += stop;
    }

    void FixedUpdate()
    {
        if (dir.Equals(Vector3.zero))
        {
            if (transform.parent != null)
                headToMousePos();
        }
        else
        {
            transform.Translate(dir * player.shootForce * Time.fixedDeltaTime, Space.World);
        }
    }

    void LateUpdate()
    {
        fixPositionIntoScreen();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            dir = Vector3.zero;
        }
    }

    public void shootEvent()
    {
        transform.SetParent(null);
        Vector3 dir = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dir.z = 0;
        this.dir = dir.normalized;
    }

    public void stop()
    {
        transform.SetParent(player.transform);
        dir = Vector3.zero;
    }

    private void headToMousePos()
    {
        float rot = neck.localRotation.eulerAngles.z + head.localRotation.eulerAngles.z + data.correctFactor;

        Vector3 dir = (Quaternion.Euler(0, 0, rot) * Vector3.right).normalized;
        if (player.transform.localScale.x < 0) dir.x *= -1;
        transform.position = neck.position + dir * data.rad;
    }

    private void fixPositionIntoScreen()
    {
        Vector3 pos = mainCamera.WorldToViewportPoint(transform.position);
        Vector3 copy = pos;

        if (pos.x <= 0f) pos.x = 0f;
        if (1f <= pos.x) pos.x = 1f;
        if (pos.y <= 0f) pos.y = 0f;
        if (1f <= pos.y) pos.y = 1f;

        if (!pos.Equals(copy))
        {
            dir = Vector3.zero;
            transform.position = mainCamera.ViewportToWorldPoint(pos);
        }
    }
}
