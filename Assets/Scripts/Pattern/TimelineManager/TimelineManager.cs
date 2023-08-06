using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineManager
{
    [System.Serializable]
    public class Pattern
    {
        public GameObject prefab;
        [TimelineElementTitle()]
        public Timeline[] timeline;
    }

    [System.Serializable]
    public class Timeline
    {
        [Tooltip("���� ���� �ð�")]
        public float startAt;
        public Detail detail;
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class PatternArrayElementTitleAttribute : PropertyAttribute
    {
        public PatternArrayElementTitleAttribute() { }
    }

    [System.AttributeUsage(System.AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class TimelineElementTitleAttribute : PropertyAttribute
    {
        public TimelineElementTitleAttribute() { }
    }
}