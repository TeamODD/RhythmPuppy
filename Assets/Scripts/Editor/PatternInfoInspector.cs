using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Runtime.Serialization;
using Unity.VisualScripting;

namespace Patterns
{
    [CustomPropertyDrawer(typeof(PatternInfoElementTitleAttribute))]
    public class PatternInfoDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PatternInfo t = (PatternInfo)property.GetUnderlyingValue();
            string newLabel = "", patternName = "None";


            newLabel += t.startAt.ToString() + "段";

            if (t.endAt != 0)
            {
                newLabel += " ~ " + t.endAt.ToString() + "段";
            }
            else if (t.duration != 0)
            {
                if (t.repeatNo != 0 && t.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.repeatNo * (t.duration + t.repeatDelayTime)).ToString() + "段";
                }
                else
                {
                    newLabel += " ~ " + (t.startAt + t.duration).ToString() + "段";
                }
            }
            else
            {
                if (t.repeatNo != 0 && t.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.repeatDelayTime * (t.repeatNo - 1)).ToString() + "段";
                }
            }
            if (t.prefab != null) patternName = t.prefab.name;
            newLabel += " : " + patternName;

            EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
        }
    }
}