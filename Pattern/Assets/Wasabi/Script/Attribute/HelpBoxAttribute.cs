using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Wasabi.Attribute;

namespace Wasabi.Attribute
{
    public enum HelpBoxType { None, Info, Warning, Error }
    [AttributeUsage(AttributeTargets.Field)]
    public class HelpBoxAttribute : PropertyAttribute
    {

        readonly internal string text;
        readonly internal int mType;

        public HelpBoxAttribute(string text, HelpBoxType helpBoxType = HelpBoxType.Info)
        {
            this.text = text;
            this.mType = (int)helpBoxType;
        }
    }
}
#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
    public class HelpBoxAttributeDrawer : PropertyDrawer
    {
        float height;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        // Draw a disabled property field
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            height = EditorGUI.GetPropertyHeight(property, label, true);

            EditorGUI.PropertyField(position, property, label, true);
            height += EditorGUI.GetPropertyHeight(property, label, true);

            int indent = EditorGUI.indentLevel;
            EditorStyles.helpBox.stretchHeight = true;
            EditorStyles.helpBox.fontSize = 15;
            float boxHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(((HelpBoxAttribute)attribute).text), position.width - indent * 15);
            Rect rect = new Rect(position.x + indent * 15, position.y + EditorGUI.GetPropertyHeight(property, label, true) + 5, position.width- indent * 15, boxHeight);
 
            EditorGUI.HelpBox(rect, ((HelpBoxAttribute)attribute).text,(MessageType)((HelpBoxAttribute)attribute).mType);
            height += boxHeight;
            /*
            GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
      
            GUI.enabled = true;
            */
        }
    }
}
#endif