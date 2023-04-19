using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Wasabi.Attribute;

namespace Wasabi.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FoldOutAttribute : PropertyAttribute
    {
        public readonly string target;
        public FoldOutAttribute(string target = null)
        {
            this.target = target;
        }
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(FoldOutAttribute), true)]
    public class FoldOutAttributeDrawer : PropertyDrawer
    {
        public string PathRefresh(SerializedProperty property)
        {
            string path = property.propertyPath;
            path = path.Remove(path.LastIndexOf('.') + 1, property.name.Length);
            path += ((FoldOutAttribute)attribute).target;
            return path;
        }

        bool ValueUpdate(SerializedProperty property)
        {
            string target = ((FoldOutAttribute)attribute).target;
            if (target == null)
            {
                return true;
            }
            else
            {
                var targetProperty = property.serializedObject.FindProperty(PathRefresh(property));
                if(targetProperty != null && targetProperty.type == "bool" && targetProperty.boolValue)
                {
                    return true;
                }
            }
            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ValueUpdate(property))
                return EditorGUI.GetPropertyHeight(property, label, true);
            else
                return 0;
        }

        // Draw a disabled property field
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ValueUpdate(property))
            {
                string target = ((FoldOutAttribute)attribute).target;
                if(target == null)
                {
                    property.boolValue = EditorGUI.Foldout(position, property.boolValue, label);
                    
                }
                else
                {
                    int indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel++;
                    EditorGUI.PropertyField(position, property, label, true);
                    EditorGUI.indentLevel = indent;
                }
            }
            /*
            GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
      
            GUI.enabled = true;
            */
        }
    }
}
#endif