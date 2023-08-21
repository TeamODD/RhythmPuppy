using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class RatSwarm : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float runtime;

        GameObject player;
        SpriteRenderer sp;
        float cooltime, dir;
        bool cooldown;

        void Awake()
        {
            init();
        }

        void Update()
        {
            if (transform.position.x < player.transform.position.x)
                sp.flipX = true;
            else
                sp.flipX = false;
        }

        void FixedUpdate()
        {
            if (!cooldown)
            {
                dir = player.transform.position.x - transform.position.x;
                dir /= Mathf.Abs(dir);
                transform.position += dir * new Vector3(speed * Time.fixedDeltaTime, 0, 0);
            }
        }

        public void init()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            sp = GetComponent<SpriteRenderer>();
            cooldown = true;

            StartCoroutine(runCooldown());
        }

        public void setCooltime(float cooltime)
        {
            this.cooltime = cooltime;
        }

        private IEnumerator runCooldown()
        {
            yield return new WaitForSeconds(cooltime);
            cooldown = false;
            yield break;
        }
    }
}