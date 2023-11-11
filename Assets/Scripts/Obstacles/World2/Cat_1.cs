using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Obstacles
{
    public class Cat_1 : MonoBehaviour
    {
        [SerializeField] float gravityScale;
        [SerializeField] float force;

        const float G = 9.8f;

        Vector3 velocity;


        void Awake()
        {
            init();
        }

        void FixedUpdate()
        {
            physicalCalculation();
            if (transform.position.y < -3f)
                Destroy(gameObject);
        }

        public void init()
        {
            velocity = new Vector3(0, force, 0);
        }

        private void physicalCalculation()
        {
            transform.Translate(velocity * Time.fixedDeltaTime);
            velocity.y -= G * Time.fixedDeltaTime * gravityScale;
        }

    }
}