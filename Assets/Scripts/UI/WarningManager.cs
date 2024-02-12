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

    /* ���� ���� �� ǥ�õǴ� ������ �Ѱ� ���� ��ũ��Ʈ */
    public class WarningManager : MonoBehaviour
    {

        [SerializeField] GameObject warningBox, warningArrow;

        EventManager eventManager;

        void Start()
        {
            eventManager = GetComponentInParent<EventManager>();
            eventManager.onWarning.AddListener(onWarning);
        }

        public void onWarning(WarningType type, Vector3 pos, Vector3 size, Vector3 dir)
        {
            switch (type)
            {
                case WarningType.Box:
                    warnWithBox(pos, size);
                    break;
                case WarningType.Arrow:
                    warnWithArrow(pos, size, dir);
                    break;
            }
        }

        public void warnWithBox(Vector3 pos, Vector3 size)
        {
            GameObject box = Instantiate(warningBox);
            box.transform.SetParent(transform);
            box.transform.position = pos;
            box.transform.localScale = size;
            box.SetActive(true);
        }

        public void warnWithArrow(Vector3 pos, Vector3 size, Vector3 dir)
        {
            GameObject arrow = Instantiate(warningArrow);
            arrow.transform.SetParent(transform);
            arrow.transform.position = pos;
            // set arrow's dir - ȭ��ǥ ���� ����
            size.x = Mathf.Abs(size.x);
            if (dir == Vector3.right)
                size.x = -size.x;
            arrow.transform.localScale = size;

            arrow.SetActive(true);
        }
    }
}