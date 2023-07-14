using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class RatSwarm : MonoBehaviour
    {
        private int dir;
        private float speed = 3f;
        private bool cooldown = true;

        void OnEnable()
        {
            if (transform.position.x < 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            cooldown = true;
            StartCoroutine(runCooldown());
        }

        void FixedUpdate()
        {
            if (!cooldown)
            {
                transform.position += dir * new Vector3(speed * Time.fixedDeltaTime, 0, 0);
            }

            if (15 < Mathf.Abs(transform.position.x))
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<Player>().getDamage(1);
                Destroy(gameObject);
            }
        }

        private IEnumerator runCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            cooldown = false;
            yield break;
        }
    }
}