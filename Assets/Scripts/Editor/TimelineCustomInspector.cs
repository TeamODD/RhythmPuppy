using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Runtime.Serialization;
using Unity.VisualScripting;

namespace Patterns
{
    [CustomPropertyDrawer(typeof(TimelineElementTitleAttribute))]
    public class TimelineDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Timeline t = (Timeline)property.GetUnderlyingValue();
            string newLabel = "";

            newLabel += t.startAt.ToString() + "��";

            if (t.endAt != 0)
            {
                newLabel += " ~ " + t.endAt.ToString() + "��";
            }
            else if (t.duration != 0)
            {
                if (t.repeatNo != 0 && t.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.repeatNo * (t.duration + t.repeatDelayTime)).ToString() + "��";
                }
                else
                {
                    newLabel += " ~ " + (t.startAt + t.duration).ToString() + "��";
                }
            }
            else
            {
                if (t.repeatNo != 0 && t.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.repeatDelayTime * (t.repeatNo - 1)).ToString() + "��";
                }
            }

            EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
        }
    }
}