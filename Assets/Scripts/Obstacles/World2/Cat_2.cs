using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class Cat_2 : MonoBehaviour
    {
        private GameObject player;
        private Vector3 dir;
        private float speed;

        void OnEnable()
        {
            speed = 5f;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void FixedUpdate()
        {
            transform.position += dir.normalized * speed * Time.fixedDeltaTime;

            if (transform.position.y < -20)
                Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<Player>().getDamage(1);
                Destroy(gameObject);
            }
        }

        public void setDir(Vector3 dir)
        {
            this.dir = dir;
        }
    }
}