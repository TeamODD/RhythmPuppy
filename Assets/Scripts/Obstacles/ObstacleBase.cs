using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public abstract class ObstacleBase : MonoBehaviour
    {
        protected GameObject player;

        PolygonCollider2D col;

        public abstract void init();

        void Awake()
        {
            col = GetComponent<PolygonCollider2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            init();
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            GameObject o = c.transform.root.gameObject;
            if (o.CompareTag("Player"))
            {
                StartCoroutine(damageEvent(o));
            }
        }

        public IEnumerator damageEvent(GameObject player)
        {
            col.enabled = false;
            player.SendMessage("getDamage");
            yield return new WaitForSeconds(1f);
            col.enabled = true;
        }
    }
}