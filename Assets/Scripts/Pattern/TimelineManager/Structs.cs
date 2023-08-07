using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineManager
{
    [System.Serializable]
    public struct Detail
    {
        [Header("End At")]
        [Tooltip("���� ���� �ð�")]
        public float endAt;

        [Header("Duration")]
        [Tooltip("���� ���� �ð�")]
        public float duration;

        [Header("Repeating")]
        [Tooltip("���� �ݺ� Ƚ��")]
        public int repeatNo;
        [Tooltip("�ݺ� ������")]
        public float repeatDelayTime;

        public Detail(float endAt, float duration, int repeatNo, float repeatDelayTime)
        {
            this.endAt = endAt;
            this.duration = duration;
            this.repeatNo = repeatNo;
            this.repeatDelayTime = repeatDelayTime;
        }
    }
}
