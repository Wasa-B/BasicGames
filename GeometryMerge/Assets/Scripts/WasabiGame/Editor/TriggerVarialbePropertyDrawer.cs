using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace WasabiGame
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(TriggerVarialbe<>))]
    public class TriggerVarialbePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position,label,property);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property.FindPropertyRelative("data"), label);
            EditorGUI.EndProperty();
        }
    }

#endif
}