using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Wasabi.Attribute;

namespace Wasabi.Attribute
{


    [AttributeUsage(AttributeTargets.Field)]
    public class DataSelectAttribute : PropertyAttribute
    {
        public readonly string _variableName;
        public DataSelectAttribute(string variableName)
        {
            this._variableName = variableName;
        }
    }

    public interface IDataObject
    {
        public List<string> GetNameList();
    }
    public class ArrayTitleEnumAttribute : PropertyAttribute
    {
        public readonly string names;
        public ArrayTitleEnumAttribute(string varName)
        {
            names = varName;
        }
    }
}
#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ArrayTitleEnumAttribute))]
    public class ArrayTitleEnumAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        protected virtual ArrayTitleEnumAttribute Attribute => attribute as ArrayTitleEnumAttribute;
        SerializedProperty TitleNameProp;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string FullPathName = property.propertyPath + "." + Attribute.names;
            TitleNameProp = property.serializedObject.FindProperty(FullPathName);
            string newLabel = GetTitle();
            if (string.IsNullOrEmpty(newLabel))
                newLabel = label.text;
            EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
            //EditorGUI.PropertyField(position, property, label);
        }
        string GetTitle()
        {
            switch (TitleNameProp.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return TitleNameProp.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return TitleNameProp.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return TitleNameProp.floatValue.ToString();
                case SerializedPropertyType.String:
                    return TitleNameProp.stringValue;
                case SerializedPropertyType.Color:
                    return TitleNameProp.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return TitleNameProp.objectReferenceValue.ToString();
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return TitleNameProp.enumNames[TitleNameProp.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return TitleNameProp.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return TitleNameProp.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return TitleNameProp.vector4Value.ToString();
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                case SerializedPropertyType.ExposedReference:
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    break;
                case SerializedPropertyType.Vector2Int:
                    break;
                case SerializedPropertyType.Vector3Int:
                    break;
                case SerializedPropertyType.RectInt:
                    break;
                case SerializedPropertyType.BoundsInt:
                    break;
                default:
                    break;
            }
            return "";
        }
    }

    [CustomPropertyDrawer(typeof(DataSelectAttribute), true)]
    public class DataSelectAttributeDrawer : PropertyDrawer
    {
        bool value = false;
        string path = "";
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        // Draw a disabled property field
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            path = property.propertyPath;
            path = path.Remove(path.LastIndexOf('.') + 1, property.name.Length);
            path += ((DataSelectAttribute)attribute)._variableName;
            SerializedProperty valueProperty = property.serializedObject.FindProperty(path);
            //UnityEngine.Object obj = valueProperty.objectReferenceValue;
            if (valueProperty != null && valueProperty.objectReferenceValue != null)
            {
                FieldDraw(valueProperty.objectReferenceValue, property, position, label);
            }
            else
            {
                valueProperty = property.serializedObject.FindProperty(((DataSelectAttribute)attribute)._variableName);
                if (valueProperty != null && valueProperty.objectReferenceValue != null)
                {
                    FieldDraw(valueProperty.objectReferenceValue, property, position, label);
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }


            /*
            GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
            GUI.enabled = true;
            */
        }

        void FieldDraw(UnityEngine.Object obj,SerializedProperty property, Rect position, GUIContent label)
        {
            if (obj.GetType().GetInterface("IDataObject") == typeof(IDataObject))
            {
                IDataObject dataSet = (IDataObject)obj;
                List<string> nameList = dataSet.GetNameList();
                int index = nameList.FindIndex((x) => x == property.stringValue);
                if (index < 0) index = 0;
                index = EditorGUI.Popup(position, label.text, index, nameList.ToArray());
                property.stringValue = nameList[index];
            }
            else if (obj.GetType() == typeof(Animations.AnimatorController))
            {
                var animController = (Animations.AnimatorController)obj;
                var childAnimeStates = animController.layers[0].stateMachine.states;
                List<string> nameList = new List<string>();
                nameList.Add("None");
                for (int i = 0; i < childAnimeStates.Length; i++)
                {
                    nameList.Add(childAnimeStates[i].state.name);
                }

                int index = nameList.FindIndex((x) => x == property.stringValue);
                if (property.stringValue == "") index = 0;
                else if (index < 0) index = 0;
                index = EditorGUI.Popup(position, label.text, index, nameList.ToArray());
                if (nameList[index] == "None")
                    property.stringValue = "";
                else
                    property.stringValue = nameList[index];
            }
        }
    }
}
#endif