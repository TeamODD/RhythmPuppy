using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("투사체 발사 시 적용되는 힘(Force)")]
    float shootForce;

    [SerializeField, Tooltip("발사 종료 후 재사용 대기시간")]
    float shootCooldown;

    GameObject player;
    Transform head, neck;
    Vector2 dir;
    Coroutine cooldownCoroutine;

    void Awake()
    {
        player = transform.parent.gameObject;
        head = player.transform.Find("머리");
        neck = player.transform.Find("bone_2/neck"); 
        dir = Vector2.zero;
        cooldownCoroutine = null;
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

    // 2, -2.4
    private void fixPosition()
    {
        const float projCorrFactor = -42.5f;
        Vector2 dir;
        Quaternion q;
        float rot;

        rot = neck.rotation.z + projCorrFactor;
        rot = rot < 0 ? rot % 360 + 360 : rot % 360;
        q = Quaternion.Euler(0, 0, rot);
        dir = q * new Vector2(2, -2.4f);
        if (player.transform.localScale.x < 0) dir.x *= -1;

        transform.position = head.position + (Vector3)dir;
    }
}
