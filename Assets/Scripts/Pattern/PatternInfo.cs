using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Patterns
{
    [Serializable]
    public class PatternInfo
    {
        // 공통된 패턴의 정보를 가진 클래스.

        [Tooltip("패턴 시작 시간")]
        public float startAt;
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
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class PatternInfoElementTitleAttribute : PropertyAttribute
    {
        public PatternInfoElementTitleAttribute() { }
    }
}