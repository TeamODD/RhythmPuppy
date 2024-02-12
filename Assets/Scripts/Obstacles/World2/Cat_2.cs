using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;

namespace Obstacles
{
    public class Cat_2 : MonoBehaviour
    {
        [SerializeField] float speed;

        private ObjectPoolManager PoolingManager;
        [HideInInspector]
        public bool IsPooled;
        GameObject player;
        SpriteRenderer sp;
        Vector3 dir;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            PoolingManager = FindObjectOfType<ObjectPoolManager>();
            IsPooled = false;

            /* 패턴매니저 2-2를 위해서 따로 함수로 뺐습니다.
             * Vector3 scale;
             * scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.transform.position.x < transform.position.x)
                scale.x *= -1;
            transform.localScale = scale;

            dir = (player.transform.position - transform.position);*/
            Setting();
        }

        void FixedUpdate()
        {
            transform.position += dir.normalized * speed * Time.fixedDeltaTime;

            if (transform.position.y < -20)
                DestroyObject();
        }

        public void Setting()
        {
            Vector3 scale;

            scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.transform.position.x < transform.position.x)
                scale.x *= -1;
            transform.localScale = scale;

            dir = (player.transform.position - transform.position);
        }

        public void setDir(Vector3 dir)
        {
            this.dir = dir;
        }

        private void DestroyObject()
        {
            if (IsPooled)
            {
                PoolingManager.ReleaseObject();
            }
            else
                Destroy(gameObject);
        }
    }
}