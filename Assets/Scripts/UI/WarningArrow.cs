using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    /* 패턴 시작 전 표시되는 경고등 (화살표형) */
    public class WarningArrow : MonoBehaviour
    {
        [Header("자식 Objects")]
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
            // 초기화 작업동안 Update() 함수 동작 억제 - bool isInitializing을 통해서 억제
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
                    // 화살표 스프라이트가 오른쪽을 바라보게 변경
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
                    // 화살표 스프라이트가 위쪽을 바라보게 회전
                    arrow.Rotate(new Vector3(0, 0, -90));
                }
                else
                {
                    // down
                    begin.y += scale.y / 2.0f;
                    end.y -= scale.y / 2.0f;
                    // 화살표 스프라이트가 아래쪽을 바라보게 회전
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

            // 실행 준비 완료
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