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
            Vector3 scale;
            player = GameObject.FindGameObjectWithTag("Player");

            scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.transform.position.x < transform.position.x)
                scale.x *= -1;
            transform.localScale = scale;

            dir = (player.transform.position - transform.position);
        }

        public void setDir(Vector3 dir)
        {
            this.dir = dir;
        }
    }
}