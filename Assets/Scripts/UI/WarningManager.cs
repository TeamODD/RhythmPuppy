using System;
using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIManagement
{
    public enum WarningType
    {
        Box,
        Arrow,
    }

    /* 패턴 시작 전 표시되는 경고등의 총괄 관리 스크립트 */
    public class WarningManager : MonoBehaviour
    {

        [SerializeField] GameObject warningBoxPrefab, warningArrowPrefab;

        EventManager eventManager;

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onWarning.AddListener(onWarning);
        }

        public void onWarning(WarningType type, Vector3 pos, Vector3 scale, Vector3 dir)
        {
            switch (type)
            {
                case WarningType.Box:
                    warnWithBox(pos, scale);
                    break;
                case WarningType.Arrow:
                    warnWithArrow(pos, scale, dir);
                    break;
            }
        }

        void warnWithBox(Vector3 pos, Vector3 scale)
        {
            GameObject warningBox = Instantiate(warningBoxPrefab);
            warningBox.transform.SetParent(transform);
            warningBox.transform.position = pos;
            warningBox.transform.localScale = scale;
            warningBox.SetActive(true);
        }

        void warnWithArrow(Vector3 pos, Vector3 scale, Vector3 dir)
        {
            GameObject warningArrow = Instantiate(warningArrowPrefab);
            warningArrow.transform.SetParent(transform);
            warningArrow.transform.position = pos;

            WarningArrow warningArrowScript = warningArrow.GetComponent<WarningArrow>();
            warningArrowScript.position = pos;
            warningArrowScript.scale = scale;
            warningArrowScript.direction = dir;

            warningArrow.SetActive(true);
        }
    }
}