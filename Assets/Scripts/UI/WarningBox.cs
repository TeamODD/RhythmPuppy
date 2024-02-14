using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    /* 패턴 시작 전 표시되는 경고등 (박스형) */
    public class WarningBox : MonoBehaviour
    {
        Image image;
        SpriteRenderer sp;
        Color c;

        void Awake()
        {
            if (!TryGetComponent(out image))
            {
                if (!TryGetComponent(out sp))
                    Destroy(gameObject);
                else
                    c = sp.color;
            }
            else
            {
                c = image.color;
            }
        }

        void Start()
        {
            StartCoroutine(destroySelf(1f));
            StartCoroutine(changeAlpha());
        }

        IEnumerator changeAlpha()
        {
            const float ALPHA_RATE = 0.7f;
            float time = 0, alpha = 0;

            while (time < 0.3f)
            {
                time += Time.deltaTime;
                alpha = time / 0.3f;
                c.a = alpha * ALPHA_RATE;

                if (image) image.color = c;
                else if (sp) sp.color = c;
                yield return null;
            }
            while (time < 0.6f)
            {
                time += Time.deltaTime;
                alpha = (time - 0.3f) / -0.6f + 1f;
                c.a = alpha * ALPHA_RATE;

                if (image) image.color = c;
                else if (sp) sp.color = c;
                yield return null;
            }
            while (time < 1)
            {
                time += Time.deltaTime;
                alpha = (time - 0.2f) / 0.8f;
                c.a = alpha * ALPHA_RATE;

                if (image) image.color = c;
                else if (sp) sp.color = c;
                yield return null;
            }
        }

        IEnumerator destroySelf(float t)
        {
            yield return new WaitForSeconds(t);
            Destroy(gameObject);
        }
    }
}