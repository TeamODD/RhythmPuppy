using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class RatSwarm : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float runtime;

        float cooltime, dir;
        bool cooldown;
        GameObject player;

        void OnEnable()
        {
            cooldown = true;
            player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(runCooldown());
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

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject o = col.transform.parent.gameObject;
            if (o.gameObject.CompareTag("Player"))
            {
                o.gameObject.GetComponent<Player>().getDamage(1);
            }
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