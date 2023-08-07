using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class Cat_1 : MonoBehaviour
    {
        [SerializeField] float force;

        /*void FixedUpdate()
        {
            transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }*/

        void OnEnable()
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * force, ForceMode2D.Impulse);
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
    }
}