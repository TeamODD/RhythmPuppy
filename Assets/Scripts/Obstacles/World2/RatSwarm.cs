using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class RatSwarm : ObstacleBase
    {
        [SerializeField] float speed;
        [SerializeField] float runtime;

        float cooltime, dir;
        bool cooldown;

        void FixedUpdate()
        {
            if (!cooldown)
            {
                dir = player.transform.position.x - transform.position.x;
                dir /= Mathf.Abs(dir);
                transform.position += dir * new Vector3(speed * Time.fixedDeltaTime, 0, 0);
            }
        }

        public override void init()
        {
            player = GameObject.FindGameObjectWithTag("Player");
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