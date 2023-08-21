using System.Collections;
using UnityEngine;

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

    [SerializeField, Tooltip("투사체 발사 시 적용되는 힘(Force)")]
    float shootForce;

    [SerializeField, Tooltip("발사 종료 후 재사용 대기시간")]
    float shootCooldown;

    GameObject player;
    Transform neck, head;
    Vector2 dir;
    Coroutine cooldownCoroutine;
    ProjectileData data;

    void Awake()
    {
        init();
    }

    void FixedUpdate()
    {
        if (dir.Equals(Vector2.zero))
            fixPosition();
        else
            transform.Translate(dir * shootForce * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            dir = Vector2.zero;
        }
    }

    public void init()
    {
        player = transform.parent.gameObject;
        neck = player.transform.Find("bone_2/neck");
        head = player.transform.Find("bone_2/neck/head");
        dir = Vector2.zero;
        cooldownCoroutine = null;

        float rad, correctFactor;
        rad = (neck.position - transform.position).magnitude;
        correctFactor = 16f;
        data = new ProjectileData(rad, correctFactor);
    }

    public void shoot()
    {
        if (cooldownCoroutine != null) return;
        transform.SetParent(null);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dir = (mousePos - transform.position).normalized;
    }

    public void stop()
    {
        transform.SetParent(player.transform);
        dir = Vector2.zero;
        cooldownCoroutine = StartCoroutine(runShootCooldown());
    }

    private IEnumerator runShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        cooldownCoroutine = null;
    }

    private void fixPosition()
    {
        float angle, rot;
        angle = head.localRotation.eulerAngles.z;
        rot = neck.localRotation.eulerAngles.z + head.localRotation.eulerAngles.z + data.correctFactor;

        Vector3 dir = (Quaternion.Euler(0, 0, rot) * Vector3.right).normalized;
        if (player.transform.localScale.x < 0) dir.x *= -1;
        transform.position = neck.position + dir * data.rad;
    }
}
