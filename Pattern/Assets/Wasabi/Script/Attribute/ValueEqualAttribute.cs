using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Wasabi.Attribute;

namespace Wasabi.Attribute
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ValueEqualAttribute : PropertyAttribute
    {
        public readonly string _variableName;
        public readonly object _value;
        public readonly bool _exclusive;
        public ValueEqualAttribute(string variableName, object value, bool exclusive = false)
        {
            _variableName = variableName;
            _value = value;
            _exclusive = exclusive;
        }
        /// <summary>
        /// value is booleanType True
        /// </summary>
        /// <param name="variableName"></param>
        public ValueEqualAttribute(string variableName)
        {
            _variableName = variableName;
            _value = true;
        }
    }
}
#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ValueEqualAttribute), true)]
    public class ValueEqualAttributeDrawer : PropertyDrawer
    {
        bool value = false;
        string path = "";

        public void PathRefresh(SerializedProperty property)
        {
            path = property.propertyPath;
            path = path.Remove(path.LastIndexOf('.') + 1, property.name.Length);
            path += ((ValueEqualAttribute)attribute)._variableName;
        }
        public void ValueUpdate(SerializedProperty property)
        {
            PathRefresh(property);
            SerializedProperty valueProperty = property.serializedObject.FindProperty(path);
            value = false;
            if (valueProperty != null)
            {
                FieldInfo targetField = null;
                object obj = valueProperty.serializedObject.targetObject;
                var paths = path.Split('.');

                for (int i = 0; i < paths.Length; i++)
                {
                    var p = paths[i];
                    var type = obj.GetType();
                    if (p == "Array")
                    {
                        p = paths[i + 1];
                        int arrayIndex = int.Parse(p.Substring(p.IndexOf('[') + 1, p.Length - 2 - p.IndexOf('[')));
                        if (type.IsArray)
                        {
                            //Debug.LogError(p + " : "+ int.Parse(p.Substring(p.IndexOf('[') + 1, p.Length - 2 - p.IndexOf('['))));
                            //obj = array.GetValue(int.Parse(p.Substring(p.IndexOf('['))));

                            obj = ((Array)obj).GetValue(arrayIndex);
                        }
                        else if (type.IsGenericType)
                        {
                            //Debug.Log("List : "+type.Name + ", type : "+type.ToString());
                            FieldInfo[] fs = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                            foreach (FieldInfo fi in fs)
                            {
                                if (fi.Name == "_items")
                                {
                                    var no = fi.GetValue(obj);
                                    //Debug.Log("FI :: " + fi.Name + " : " + no.GetType().Name);

                                    obj = ((Array)no).GetValue(arrayIndex);
                                    break;

                                }
                            }
                        }

                        i++;
                        continue;
                    }

                    //Debug.Log(path + ": " + p + " , Type : " + type);

                    targetField = type.GetField(p, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    obj = targetField.GetValue(obj);


                }
                if (obj != null)
                {
                    value = ((ValueEqualAttribute)attribute)._value.Equals(obj) ^ ((ValueEqualAttribute)attribute)._exclusive;

                    //Debug.Log(value+" : " + ((ValueEqualAttribute)attribute)._value + " = "+targetField.GetValue(obj));
                }
                else
                {
                    Debug.Log("Not Find Field : " + path + ", " + property.serializedObject.targetObject.GetType());
                }
            }
            else
            {
                Debug.Log("Not Find Property : " + path);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ValueUpdate(property);
            if (value)
                return EditorGUI.GetPropertyHeight(property, label, true);
            else
                return 0;
        }

        // Draw a disabled property field
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ValueUpdate(property);
            if (value)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            /*
            GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
      
            GUI.enabled = true;
            */
        }
    }
}
#endif