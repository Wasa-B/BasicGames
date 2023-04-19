using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Wasabi.Attribute;


namespace Wasabi.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectVectorAttribute : PropertyAttribute
    {
        public readonly bool localPosition;
        public SelectVectorAttribute(bool localPosition = false)
        {
            this.localPosition = localPosition;
        }
    }
}
#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(SelectVectorAttribute))]
    public class SelectVectorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var posRect = new Rect(position.x, position.y, 200, position.height);
            var buttonRect = new Rect(position.x - 62, position.y, 60, position.height);
            EditorGUI.PropertyField(posRect, property, GUIContent.none);
            if (GUI.Button(buttonRect, "current"))
            {
                property.vector2Value = Selection.activeTransform.position;
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
#endif