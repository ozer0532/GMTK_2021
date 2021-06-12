using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Zro.UI.Editor
{
    [CustomPropertyDrawer(typeof(UIAnimationTrigger))]
    public class UIAnimationTriggerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("animations"), label, true);

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("animations"), label, true);
        }
    }
}
