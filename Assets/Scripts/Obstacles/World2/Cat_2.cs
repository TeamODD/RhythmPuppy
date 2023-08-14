using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class Cat_2 : MonoBehaviour
    {
        [SerializeField] float speed;

        GameObject player;
        SpriteRenderer sp;
        Vector3 dir;

        void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            sp = GetComponent<SpriteRenderer>();
            sp.flipX = false;
            if (player.transform.position.x < transform.position.x)
                sp.flipX = true;

        }

        void FixedUpdate()
        {
            transform.position += dir.normalized * speed * Time.fixedDeltaTime;

            if (transform.position.y < -20)
                Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject o = col.transform.parent.gameObject;
            if (o.gameObject.CompareTag("Player"))
            {
                o.gameObject.GetComponent<Player>().getDamage(1);
            }
        }

        public void setDir(Vector3 dir)
        {
            this.dir = dir;
        }
    }
}