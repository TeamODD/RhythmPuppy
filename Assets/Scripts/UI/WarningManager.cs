using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;

namespace UIManagement
{
    /* 패턴 시작 전 표시되는 경고등의 총괄 관리 스크립트 */
    public class WarningManager : MonoBehaviour
    {
        EventManager eventManager;

        void Awake()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onWarning.AddListener(warnWithBox);
        }

        public void warnWithBox(GameObject warningType, Vector3 pos, Vector3 size)
        {
            GameObject o = Instantiate(warningType);
            o.transform.SetParent(transform);
            o.transform.position = pos;
            o.transform.localScale = size;
            o.SetActive(true);
        }
    }
}