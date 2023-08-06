using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineManager
{
    [System.Serializable]
    public struct Detail
    {
        [Header("Detail Type")]
        [Tooltip("���� ���� Ÿ��")]
        public PatternDetail detailType;

        [Header("Duration")]
        [Tooltip("���� ���� �ð�")]
        public float duration;

        [Header("Repeating")]
        [Tooltip("���� �ݺ� Ƚ��")]
        public int repeatNo;
        [Tooltip("�ݺ� ������")]
        public float repeatDelayTime;

        public Detail(PatternDetail detailType, float duration, int repeatNo, float repeatDelayTime)
        {
            this.detailType = detailType;
            this.duration = duration;
            this.repeatNo = repeatNo;
            this.repeatDelayTime = repeatDelayTime;
        }
    }
}
