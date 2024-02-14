using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    /* ���� ���� �� ǥ�õǴ� ���� (ȭ��ǥ��) */
    public class WarningArrow : MonoBehaviour
    {
        [Header("�ڽ� Objects")]
        [SerializeField] RectTransform arrow;
        [SerializeField] RectTransform backgroundBox;
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Vector3 direction { get; set; }

        SpriteRenderer srBackgroundBox;
        Color c;
        Vector3 begin, end;
        float time, speed;
        bool isInitializing;

        void Start()
        {
            float scaleRate;

            /* bBoxTransform = backgroundBox.GetComponent<RectTransform>();
            arrowTransform = arrow.GetComponent<RectTransform>(); */
            srBackgroundBox = backgroundBox.GetComponent<SpriteRenderer>();
            speed = 1.5f;
            // �ʱ�ȭ �۾����� Update() �Լ� ���� ���� - bool isInitializing�� ���ؼ� ����
            isInitializing = true;

            // Init BackgroundBox object
            backgroundBox.position = position;
            backgroundBox.localScale = scale;
            // Init Arrow object
            begin = position;
            end = position;
            if (direction.Equals(Vector3.left) || direction.Equals(Vector3.right))
            {
                scaleRate = scale.y / arrow.sizeDelta.y;
                if (direction.Equals(Vector3.left))
                {
                    begin.x -= scale.x / 2.0f;
                    end.x += scale.x / 2.0f;
                    // arrow.Rotate(new Vector3(0, 0, 0));
                }
                else
                {
                    // right
                    begin.x += scale.x / 2.0f;
                    end.x -= scale.x / 2.0f;
                    // ȭ��ǥ ��������Ʈ�� �������� �ٶ󺸰� ����
                    arrow.Rotate(new Vector3(0, 0, 180));
                }
            }
            else
            {
                scaleRate = scale.x / arrow.sizeDelta.x;
                if (direction.Equals(Vector3.up))
                {
                    begin.y -= scale.y / 2.0f;
                    end.y += scale.y / 2.0f;
                    // ȭ��ǥ ��������Ʈ�� ������ �ٶ󺸰� ȸ��
                    arrow.Rotate(new Vector3(0, 0, -90));
                }
                else
                {
                    // down
                    begin.y += scale.y / 2.0f;
                    end.y -= scale.y / 2.0f;
                    // ȭ��ǥ ��������Ʈ�� �Ʒ����� �ٶ󺸰� ȸ��
                    arrow.Rotate(new Vector3(0, 0, 90));
                }
            }
            arrow.localScale *= scaleRate;

            // Init backgroundBox's alpha 
            c = srBackgroundBox.color;
            c.a = 0.7f;
            srBackgroundBox.color = c;
            // Init arrow's alpha 
            /* c = arrow.color;
            c.a = 1f;
            arrow.color = c; */

            // ���� �غ� �Ϸ�
            time = 0.1f;
            arrow.position = Vector3.Lerp(begin, end, time);
            backgroundBox.SetParent(arrow);
            arrow.gameObject.SetActive(true);
            backgroundBox.gameObject.SetActive(true);
            isInitializing = false;
            StartCoroutine(destroySelf(1f));
            //StartCoroutine(ShowWarning());
        }

        void Update()
        {
            if (isInitializing) return;
            time += Time.deltaTime;
            arrow.position = Vector3.Lerp(begin, end, time);
        }

        IEnumerator destroySelf(float t)
        {
            yield return new WaitForSeconds(t);
            Destroy(gameObject);
        }
    }
}