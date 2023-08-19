using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class Oak_A : MonoBehaviour
    {
        float speed;
        private int dir;


        // Start is called before the first frame update
        void Start()
        {
            speed = 5f;
        }

        void FixedUpdate()
        {
            if (dir != 0)
            {
                transform.position += new Vector3(speed * dir, 0, 0) * Time.fixedDeltaTime;
            }
        }
        void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag("Player"))
            {
                col.gameObject.SendMessage("getDamage");
                Destroy(gameObject);
            }
            Debug.Log("hi");
        }

        public void setDir(int dir)
        {
            this.dir = dir;
        }
    }
}