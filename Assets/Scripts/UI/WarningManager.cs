using System.Collections;
using System.Collections.Generic;
using EventManagement;
using UnityEngine;

namespace UIManagement
{
    /* ���� ���� �� ǥ�õǴ� ������ �Ѱ� ���� ��ũ��Ʈ */
    public class WarningManager : MonoBehaviour
    {
        [SerializeField]
        GameObject warningBoxPrefab, warningArrowPrefab;

        EventManager eventManager;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            eventManager.stageEvent.warnWithBox += warnWithBox;
        }

        public void warnWithBox(Vector3 pos, Vector3 size)
        {
            GameObject o = Instantiate(warningBoxPrefab);
            o.transform.SetParent(transform);
            o.transform.position = pos;
            o.transform.localScale = size;
            o.SetActive(true);
        }
    }
}