using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Obstacles
{
    public class Cat_1 : ObstacleBase
    {
        [SerializeField] float force;

        void OnCollisionEnter2D(Collision2D col)
        {
            GameObject o = col.transform.root.gameObject;
            Vector2 point = col.GetContact(0).point;
            RaycastHit2D hit = Physics2D.Raycast(point, transform.forward, 100);

            /* 고양이가 가려지지 않고 플레이어와 닿았는가? */
            if (transform.Equals(hit.transform) && o.gameObject.CompareTag("Player"))
            {
                StartCoroutine(damageEvent(o));
            }
        }

        public override void init()
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * force, ForceMode2D.Impulse);
        }
    }
}