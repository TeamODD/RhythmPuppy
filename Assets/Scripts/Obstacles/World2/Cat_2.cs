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

        void Awake()
        {
            init();
        }

        void FixedUpdate()
        {
            transform.position += dir.normalized * speed * Time.fixedDeltaTime;

            if (transform.position.y < -20)
                Destroy(gameObject);
        }

        public void init()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            sp = GetComponent<SpriteRenderer>();
            sp.flipX = false;
            if (player.transform.position.x < transform.position.x)
                sp.flipX = true;
        }

        public void setDir(Vector3 dir)
        {
            this.dir = dir;
        }
    }
}