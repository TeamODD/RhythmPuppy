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
        // ����� ������ ������ ���� Ŭ����.

        [Tooltip("���� ���� �ð�")]
        public float startAt;
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
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class PatternInfoElementTitleAttribute : PropertyAttribute
    {
        public PatternInfoElementTitleAttribute() { }
    }
}