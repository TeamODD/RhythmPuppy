using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Obstacles
{
    public class Cat_1 : MonoBehaviour
    {
        [SerializeField] float force;

        Rigidbody2D rig2D;

        void Awake()
        {
            init();
        }

        public void init()
        {
            rig2D = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().AddForce(transform.up * force, ForceMode2D.Impulse);
            rig2D.gravityScale = 1f;
        }
    }
}