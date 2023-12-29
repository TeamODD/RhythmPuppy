using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
//using UnityEngine.Pool;

namespace Obstacles
{
    public class Pattern9 : MonoBehaviour
    {
        [SerializeField] float gravityScale;

        private ObjectPoolManager PoolingManager;
        private Rigidbody2D rigid;

        [HideInInspector]
        public bool IsPooled = false;

        const float G = 9.8f;

        public Vector3 velocity;


        void Awake()
        {
            init();
        }

        void FixedUpdate()
        {
            physicalCalculation();
            if (transform.position.y < 0)
                rigid.simulated = false;
            else
                rigid.simulated = true;
            if (transform.position.y < -4f)
                DestroyObject();
        }

        public void init()
        {
            rigid = gameObject.GetComponent<Rigidbody2D>();
            PoolingManager = FindObjectOfType<ObjectPoolManager>();
            //gameObject.GetComponent<Rigidbody2D>().GetShapes(PhysicsShapeType2D.Polygon, gameObject());
        }

        private void physicalCalculation()
        {
            transform.Translate(velocity * Time.fixedDeltaTime);
            velocity.y -= G * Time.fixedDeltaTime * gravityScale;
        }

        private void DestroyObject()
        {
            if (IsPooled)
            {
                //Pool¿¡ ¹Ý³³
                //Debug.Log("Released Cat");
                PoolingManager.ReleaseObject();
            }
            else
                Destroy(this.gameObject);
        }

    }
}
