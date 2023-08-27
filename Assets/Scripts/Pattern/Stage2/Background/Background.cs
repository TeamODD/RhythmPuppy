using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stage_2
{
    public class Background : MonoBehaviour
    {
        enum Type
        {
            Fast, Normal, Slow
        }

        [SerializeField] Type type;
        [SerializeField] float width;
        [SerializeField] bool doNotCopyThisObject;

        bool isCopied;
        RectTransform rectTransform;
        float speed;
        Vector3 v;

        void Awake()
        {
            isCopied = false;
            rectTransform = GetComponent<RectTransform>();
            switch (type)
            {
                case Type.Fast:
                    speed = 1f;
                    break;
                case Type.Normal:
                    speed = 0.4f;
                    break;
                case Type.Slow:
                    speed = 0.1f;
                    break;
            }
            v = new Vector3(speed, 0, 0);
        }

        void FixedUpdate()
        {
            transform.position += -v * Time.deltaTime;
            if (!doNotCopyThisObject && !isCopied && transform.position.x < 0) copy();
            if (transform.position.x < -width * 1.5f) Destroy(gameObject);
        }

        private void copy()
        {
            isCopied = true;
            GameObject o = Instantiate(gameObject);
            o.transform.position = transform.position + new Vector3(width, 0, 0);
            o.transform.SetParent(transform.parent);
        }
    }
}