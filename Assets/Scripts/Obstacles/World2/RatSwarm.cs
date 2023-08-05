using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class RatSwarm : MonoBehaviour
    {
        [SerializeField] float speed = 3f;
        [SerializeField] float runtime = 16f;
        [SerializeField] float cooltime;

        bool cooldown;
        float dir;
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
                Destroy(gameObject);
            }
        }

        private IEnumerator runCooldown()
        {
            yield return new WaitForSeconds(cooltime);
            cooldown = false;
            Invoke("destroySelf", runtime);
            yield break;
        }

        private void destroySelf()
        {
            Destroy(gameObject);
        }
    }
}