using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Obstacles
{
    public class Cat_1 : MonoBehaviour
    {
        [SerializeField] float force;

        /*void FixedUpdate()
        {
            transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }*/

        void OnEnable()
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * force, ForceMode2D.Impulse);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject o = col.transform.parent.gameObject;

            if (o.gameObject.CompareTag("Player"))
            {

                o.gameObject.GetComponent<Player>().getDamage(1);
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            GameObject o = col.transform.gameObject;
            Vector2 point = col.GetContact(0).point;
            RaycastHit2D hit = Physics2D.Raycast(point, transform.forward, 100);

            /* 고양이가 가려지지 않고 플레이어와 닿았는가? */
            if (transform.Equals(hit.transform) && o.gameObject.CompareTag("Player"))
                o.gameObject.GetComponent<Player>().getDamage(1);
        }
    }
}