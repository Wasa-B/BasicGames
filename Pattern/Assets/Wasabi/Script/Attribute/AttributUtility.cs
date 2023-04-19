using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Wasabi.Attribute;
using System.Reflection;
using UnityEditor;

namespace Wasabi.Attribute
{
#if UNITY_EDITOR
    public static class AttributUtility
    {
        public static object GetObject(SerializedProperty serializedProperty)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return serializedProperty.intValue;
                    
                case SerializedPropertyType.Boolean:
                    return serializedProperty.boolValue;
                    
                case SerializedPropertyType.Float:
                    return serializedProperty.floatValue;
                case SerializedPropertyType.String:
                    return serializedProperty.stringValue;
                case SerializedPropertyType.Color:
                    return serializedProperty.colorValue;
                case SerializedPropertyType.ObjectReference:
                    break;
                case SerializedPropertyType.LayerMask:
                    return serializedProperty.intValue;
                case SerializedPropertyType.Enum:
                    return serializedProperty.enumValueIndex;
                case SerializedPropertyType.Vector2:
                    return serializedProperty.vector2Value;
                case SerializedPropertyType.Vector3:
                    return serializedProperty.vector3Value;
                case SerializedPropertyType.Vector4:
                    return serializedProperty.vector4Value;
                case SerializedPropertyType.Rect:
                    return serializedProperty.rectValue;
                case SerializedPropertyType.ArraySize:
                    return serializedProperty.arraySize;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    return serializedProperty.animationCurveValue;
                case SerializedPropertyType.Bounds:
                    return serializedProperty.boundsValue;
                case SerializedPropertyType.Quaternion:
                    return serializedProperty.quaternionValue;
                case SerializedPropertyType.ExposedReference:
                    return serializedProperty.exposedReferenceValue;
                case SerializedPropertyType.FixedBufferSize:
                    return serializedProperty.fixedBufferSize;
                case SerializedPropertyType.Vector2Int:
                    return serializedProperty.vector2IntValue;
                case SerializedPropertyType.Vector3Int:
                    return serializedProperty.vector3IntValue;
                case SerializedPropertyType.RectInt:
                    return serializedProperty.rectIntValue;
                case SerializedPropertyType.BoundsInt:
                    return serializedProperty.boundsIntValue;
                case SerializedPropertyType.ManagedReference:
                    return serializedProperty.managedReferenceValue;
                case SerializedPropertyType.Hash128:
                    return serializedProperty.hash128Value;
            }
            return serializedProperty.serializedObject.targetObject;
        }
    }
#endif
}
