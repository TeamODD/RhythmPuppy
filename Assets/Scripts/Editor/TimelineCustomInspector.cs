using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using TimelineManager;
using System.Runtime.Serialization;
using Unity.VisualScripting;

namespace TimelineManager
{
    [CustomPropertyDrawer(typeof(PlaylistElementNameAttribute))]
    public class PlaylistDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string newLabel = "";
            Playlist p = (Playlist)property.GetUnderlyingValue();
            if (p == null || p.prefab == null)
                newLabel = label.text;
            else
                newLabel = p.ToString();

            EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
        }
    }

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

            newLabel += t.startAt.ToString() + "段";

            if (t.detail.endAt != 0)
            {
                newLabel += " ~ " + t.detail.endAt.ToString() + "段";
            }
            else if (t.detail.duration != 0)
            {
                if (t.detail.repeatNo != 0 && t.detail.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.detail.repeatNo * (t.detail.duration + t.detail.repeatDelayTime)).ToString() + "段";
                }
                else
                {
                    newLabel += " ~ " + (t.startAt + t.detail.duration).ToString() + "段";
                }
            }
            else
            {
                if (t.detail.repeatNo != 0 && t.detail.repeatDelayTime != 0)
                {
                    newLabel += " ~ " + (t.startAt + t.detail.repeatDelayTime * t.detail.repeatNo).ToString() + "段";
                }
            }

            EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
        }
    }
}