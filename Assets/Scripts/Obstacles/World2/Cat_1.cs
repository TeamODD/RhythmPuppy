using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
//using UnityEngine.Pool;

namespace Obstacles
{
    public class Cat_1 : MonoBehaviour
    {
        [SerializeField] float gravityScale;
        [SerializeField] float force;

        private ObjectPoolManager PoolingManager;
        private Rigidbody2D rigid;

        [HideInInspector]
        public bool IsPooled = false;

        const float G = 9.8f;

        Vector3 velocity;


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
            Jump();
            rigid = gameObject.GetComponent<Rigidbody2D>();
            PoolingManager = FindObjectOfType<ObjectPoolManager>();
            //gameObject.GetComponent<Rigidbody2D>().GetShapes(PhysicsShapeType2D.Polygon, gameObject());
        }

        private void physicalCalculation()
        {
            transform.Translate(velocity * Time.fixedDeltaTime);
            velocity.y -= G * Time.fixedDeltaTime * gravityScale;
        }

        public void Jump()
        {
            //����2_2�� ���� �Լ� �߰��߽��ϴ�
            velocity = new Vector3(0, force, 0);
        }

        private void DestroyObject()
        {
            if (IsPooled)
            {
                //Pool�� �ݳ�
                //Debug.Log("Released Cat");
                PoolingManager.ReleaseObject();
            }
            else
                Destroy(this.gameObject);
        }

    }
}