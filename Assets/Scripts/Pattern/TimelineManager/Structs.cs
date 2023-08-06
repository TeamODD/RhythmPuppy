using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineManager
{
    [System.Serializable]
    public struct Detail
    {
        [Header("Detail Type")]
        [Tooltip("패턴 세부 타입")]
        public PatternDetail detailType;

        [Header("Duration")]
        [Tooltip("패턴 지속 시간")]
        public float duration;

        [Header("Repeating")]
        [Tooltip("패턴 반복 횟수")]
        public int repeatNo;
        [Tooltip("반복 딜레이")]
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
