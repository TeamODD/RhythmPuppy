using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineManager
{
    [System.Serializable]
    public struct Detail
    {
        [Header("End At")]
        [Tooltip("패턴 종료 시간")]
        public float endAt;

        [Header("Duration")]
        [Tooltip("패턴 지속 시간")]
        public float duration;

        [Header("Repeating")]
        [Tooltip("패턴 반복 횟수")]
        public int repeatNo;
        [Tooltip("반복 딜레이")]
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
