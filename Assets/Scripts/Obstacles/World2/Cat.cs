using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class Cat : MonoBehaviour
    {
        private float speed = 4f;

        /*void FixedUpdate()
        {
            transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }*/

        void OnEnable()
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * 10, ForceMode2D.Impulse);
        }

        void FixedUpdate()
        {
            if(transform.position.y < -1.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}