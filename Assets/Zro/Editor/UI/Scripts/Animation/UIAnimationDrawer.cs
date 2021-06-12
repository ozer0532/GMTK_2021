using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Zro.UI.Editor
{
    [CustomPropertyDrawer(typeof(UIAnimation))]
    public class UIAnimationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            UIAnimation animation = property.objectReferenceValue as UIAnimation;

            if (animation)
            {
                DrawAnimationProperties(position, animation, property, label);
            }
            else
            {
                DrawAddButton(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            UIAnimation animation = property.objectReferenceValue as UIAnimation;

            if (animation && property.isExpanded)
            {
                return GetAnimationPropertiesHeight(animation);
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        private void DrawAnimationProperties(Rect position, UIAnimation animation, SerializedProperty property, GUIContent label)
        {
            SerializedObject serObj = new SerializedObject(animation);
            SerializedProperty iterator = serObj.GetIterator();

            // Reset line height
            position.height = EditorGUIUtility.singleLineHeight;

            Vector2 pos;
            Vector2 size;

            // Draw foldout
            pos = position.position;
            size = new Vector2(position.width - position.height, position.height);
            if (!string.IsNullOrWhiteSpace(animation.animationName)) label.text = animation.animationName;
            label.text += $" ({ObjectNames.NicifyVariableName(animation.GetType().Name)})"; // Add animation type
            property.isExpanded = EditorGUI.Foldout(new Rect(pos, size), property.isExpanded, label, true);

            // Draw delete button
            pos = new Vector2(position.width - position.height + position.x, position.y);
            size = new Vector2(position.height, position.height);
            if (GUI.Button(new Rect(pos, size), new GUIContent("-")))
            {
                property.objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();

                // This is not needed as Unity will automatically remove this
                //Object.DestroyImmediate(animation);
            }

            if (property.isExpanded)
            {
                // Make space for the control buttons
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Draw properties of child object
                EditorGUIUtility.labelWidth = 0;
                using (new EditorGUI.IndentLevelScope(1))
                {
                    if (iterator.NextVisible(true))
                    {
                        do
                        {
                            // Skip if this is the script field
                            if (iterator.name == "m_Script") continue;

                            // TODO: Add functionality for structs and non-Object classes

                            EditorGUI.PropertyField(position, iterator, true);
                            iterator.serializedObject.ApplyModifiedProperties();
                            position.y += EditorGUI.GetPropertyHeight(iterator) + EditorGUIUtility.standardVerticalSpacing;
                        }
                        while (iterator.NextVisible(false));
                    }
                }
            }
        }

        private float GetAnimationPropertiesHeight(UIAnimation animation)
        {
            SerializedObject serObj = new SerializedObject(animation);
            SerializedProperty iterator = serObj.GetIterator();

            // Make space for the control buttons
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (iterator.NextVisible(true))
            {
                do
                {
                    // Skip if this is the script field
                    if (iterator.name == "m_Script") continue;

                    // TODO: Add functionality for structs and non-Object classes

                    height += EditorGUI.GetPropertyHeight(iterator) + EditorGUIUtility.standardVerticalSpacing;
                }
                while (iterator.NextVisible(false));
            }

            return height;
        }

        private void DrawAddButton(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position.position, new Vector2(EditorGUIUtility.labelWidth, position.height));
            Rect buttonRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);

            GUI.Label(labelRect, label);
            if (GUI.Button(buttonRect, new GUIContent("Add Animation")))
            {
                GenericMenu menu = new GenericMenu();

                foreach (UIAnimationMapping option in UIAnimationMapping.GeneratePopupOptions())
                {
                    menu.AddItem(new GUIContent(option.name), false, () => {
                        property.objectReferenceValue = (UIAnimation)ScriptableObject.CreateInstance(option.type);
                        property.isExpanded = false;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }
        }


    }

    public struct UIAnimationMapping
    {
        public string name;
        public System.Type type;

        public UIAnimationMapping(string name, System.Type type)
        {
            this.name = name;
            this.type = type;
        }

        public static UIAnimationMapping[] GeneratePopupOptions()
        {
            return Assembly.GetAssembly(typeof(UIAnimation)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(UIAnimation)))
                .Select(type => {
                    string optionName = type.Name;
                    System.Type baseType = type;
                    while (true)
                    {
                        baseType = baseType.BaseType;

                        if (baseType.Equals(typeof(UIAnimation))) break;

                        optionName = $"{baseType.Name}/{optionName}";
                    }
                    return new UIAnimationMapping(optionName, type);
                }).ToArray();
        }
    }
}