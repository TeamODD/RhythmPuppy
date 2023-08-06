using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using TimelineManager;
using System.Runtime.Serialization;

[CustomPropertyDrawer(typeof(PatternArrayElementTitleAttribute))]
public class PatternArrayDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string pathName = property.propertyPath + ".";
        SerializedProperty prefab = property.serializedObject.FindProperty(pathName + "prefab");
        if (prefab == null || prefab.objectReferenceValue == null)
        {
            EditorGUI.PropertyField(position, property, new GUIContent(label.text, label.tooltip), true);
            return;
        }
        string newLabel = prefab.objectReferenceValue.name;
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
        string[] propName = { "startAt",  "detail.detailType" , "detail.duration", "detail.repeatNo", "detail.repeatDelayTime" };
        string newLabel = "", pathName = property.propertyPath + ".";

        float startAt = property.serializedObject.FindProperty(pathName + propName[0]).floatValue;

        Detail detail = new Detail(
                (PatternDetail)property.serializedObject.FindProperty(pathName + propName[1]).enumValueIndex,
                property.serializedObject.FindProperty(pathName + propName[2]).floatValue,
                property.serializedObject.FindProperty(pathName + propName[3]).intValue,
                property.serializedObject.FindProperty(pathName + propName[4]).floatValue
            );

        if (detail.detailType != PatternDetail.None)
            newLabel += "(" + detail.detailType.ToString().ToUpper() + ") ";
        newLabel += startAt.ToString() + "段";

        if (detail.duration != 0)
        {
            if (detail.repeatNo != 0 && detail.repeatDelayTime != 0)
            {
                newLabel += " ~ " + (startAt + detail.repeatNo * (detail.duration + detail.repeatDelayTime)).ToString() + "段";
            }
            else
            {
                newLabel += " ~ " + (startAt + detail.duration).ToString() + "段";
            }
        }
        else
        {
            if (detail.repeatNo != 0 && detail.repeatDelayTime != 0)
            {
                newLabel += " ~ " + (startAt + detail.repeatDelayTime * detail.repeatNo).ToString() + "段";
            }
        }

        EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
    }
}
