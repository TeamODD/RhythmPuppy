using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rig2D = GetComponent<Rigidbody2D>();
        if (col.gameObject.CompareTag("Ground"))
        {
            rig2D.velocity = Vector2.zero;
        }
    }
}
