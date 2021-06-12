using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UIElements;

namespace Zro.UI.Editor
{
    [CustomEditor(typeof(UIAnimationController))]
    public class UIAnimationControllerEditor : UnityEditor.Editor
    {
        private AnimBool showAnimations;

        private SerializedProperty animations;

        private VisualTreeAsset inspectorTreeAsset;
        private VisualTreeAsset animationMenuAsset;

        private void OnEnable()
        {
            showAnimations = new AnimBool(true);
            showAnimations.valueChanged.AddListener(Repaint);

            animations = serializedObject.FindProperty("animations");
        }

        const string inspectorFilename = "ui-animation-controller-uie";
        const string menuItemFilename = "ui-animation-menu-uie";

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            // Load assets
            inspectorTreeAsset = Resources.Load(inspectorFilename) as VisualTreeAsset;
            animationMenuAsset = Resources.Load(menuItemFilename) as VisualTreeAsset;

            // Inspector initialization
            VisualElement customInspector = new VisualElement();
            inspectorTreeAsset.CloneTree(customInspector);

            //customInspector.styleSheets.Add(Resources.Load($"{resourceFilename}-style") as StyleSheet);

            // Animation container
            VisualElement animationListContainer = customInspector.Q<VisualElement>("AnimationListContainer");

            // Draw menus for all animations
            GenerateAllMenus(animationListContainer, animations);

            // Create add button callback
            Button addButton = customInspector.Q<Button>("AddButton");
            addButton.clicked += () =>
            {
                GenericMenu menu = new GenericMenu();

                foreach(UIAnimationMapping option in UIAnimationMapping.GeneratePopupOptions())
                {
                    menu.AddItem(new GUIContent(option.name), false, () => { 
                        UIAnimationController target = (UIAnimationController)serializedObject.targetObject;
                        target.animations.Add((UIAnimation)CreateInstance(option.type));

                        // Update the object values
                        EditorUtility.SetDirty(target);
                        serializedObject.Update();

                        // Redraw animations
                        animationListContainer.Clear();
                        GenerateAllMenus(animationListContainer, animations);
                    });
                }

                menu.ShowAsContext();
                
            };

            return customInspector;
        }

        private void GenerateAllMenus(VisualElement animationListContainer, SerializedProperty animation)
        {
            for (int i = 0; i < animations.arraySize; i++)
            {
                SerializedProperty property = animations.GetArrayElementAtIndex(i);
                int index = i;
                VisualElement animationMenu = CreateMenu((UIAnimation)property.objectReferenceValue, property, () => {
                    // Remove the property
                    UIAnimationController target = (UIAnimationController)serializedObject.targetObject;
                    DestroyImmediate(target.animations[index]);
                    target.animations.RemoveAt(index);

                    // Update the object values
                    EditorUtility.SetDirty(target);
                    serializedObject.Update();

                    // Redraw animations
                    animationListContainer.Clear();
                    GenerateAllMenus(animationListContainer, animation);
                });
                animationListContainer.Add(animationMenu);
            }
        }

        private VisualElement CreateMenu(UIAnimation animation, SerializedProperty property, System.Action deleteEvent)
        {
            if (animation)
            {
                // Get the menu template
                VisualElement animationMenu = animationMenuAsset.CloneTree();
                VisualElement propertyContainer = animationMenu.Q<VisualElement>("PropertyContainer");

                // Setup foldout
                Foldout menuFoldout = animationMenu.Q<Foldout>("MenuFoldout");
                UpdateAnimationMenuName(animationMenu, animation);
                menuFoldout.value = property.isExpanded;
                menuFoldout.RegisterValueChangedCallback((e) =>
                {
                    property.isExpanded = e.newValue;

                    if (property.isExpanded)
                    {
                        propertyContainer.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        propertyContainer.style.display = DisplayStyle.None;
                    }
                });

                // !! TODO: Add removal button
                Button deleteButton = animationMenu.Q<Button>("DeleteButton");
                deleteButton.style.backgroundImage = new StyleBackground(EditorGUIUtility.FindTexture("d_Toolbar Minus"));
                deleteButton.clicked += deleteEvent;

                if (!property.isExpanded)
                {
                    propertyContainer.style.display = DisplayStyle.None;
                }

                PopulateMenu(animationMenu, animation);

                return animationMenu;
            }
            return null;
        }

        private void PopulateMenu(VisualElement animationMenu, UIAnimation animation)
        {
            VisualElement propertyContainer = animationMenu.Q<VisualElement>("PropertyContainer");
            propertyContainer.Clear();

            // Get the SerializedObject for the UIAnimation
            SerializedObject serObjAnimation = new SerializedObject(animation);
            SerializedProperty iterator = serObjAnimation.GetIterator();

            // Create PropertyFields for all fields in the UIAnimation
            bool isEmpty = true;
            if (iterator.NextVisible(true))
            {
                do
                {
                    // Skip if this is the script field
                    if (iterator.name == "m_Script") continue;

                    // TODO: Add functionality for structs and non-Object classes

                    var propertyField = new PropertyField() { name = "PropertyField:" + iterator.propertyPath };
                    propertyField.BindProperty(iterator.Copy());

                    SerializedProperty prop = iterator.Copy();
                    IMGUIContainer imgui = new IMGUIContainer();
                    imgui.onGUIHandler = () =>
                    {
                        EditorGUI.PropertyField(new Rect(0, EditorGUIUtility.standardVerticalSpacing, imgui.worldBound.width, EditorGUI.GetPropertyHeight(prop)), prop, true);
                        prop.serializedObject.ApplyModifiedProperties();

                        UpdateAnimationMenuName(animationMenu, animation);
                        imgui.style.height = EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                    };
                    propertyContainer.Add(imgui);

                    //propertyContainer.Add(propertyField);

                    if (iterator.name != "animationName")
                    {
                        isEmpty = false;
                    }
                }
                while (iterator.NextVisible(false));
            }

            if (isEmpty)
            {
                Label label = new Label("This UI Animation does not contain any parameters.");
                label.style.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                label.style.unityTextAlign = TextAnchor.LowerLeft;
                propertyContainer.Add(label);
            }
        }

        private void UpdateAnimationMenuName(VisualElement animationMenu, UIAnimation animation)
        {
            Foldout menuFoldout = animationMenu.Q<Foldout>("MenuFoldout");
            if (string.IsNullOrWhiteSpace(animation.animationName))
            {
                menuFoldout.text = ObjectNames.NicifyVariableName(animation.GetType().Name);
            }
            else
            {
                menuFoldout.text = $"{animation.animationName} ({ObjectNames.NicifyVariableName(animation.GetType().Name)})";
            }
        }

        private void PopulateMenuManual(VisualElement animationMenu, UIAnimation animation)
        {
            VisualElement propertyContainer = animationMenu.Q<VisualElement>("PropertyContainer");
            propertyContainer.Clear();

            var fields = animation.GetType().GetFields();

            foreach (var field in fields)
            {
                // Select proper fields to the type
                if (typeof(int).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new IntegerField(), animation, field);
                }
                else if (typeof(long).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new LongField(), animation, field);
                }
                else if (typeof(float).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new FloatField(), animation, field);
                }
                else if (typeof(string).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new TextField(), animation, field);
                }
                else if (typeof(Vector2).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new Vector2Field(), animation, field);
                }
                else if (typeof(Vector3).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new Vector3Field(), animation, field);
                }
                else if (typeof(Vector4).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new Vector4Field(), animation, field);
                }
                else if (typeof(Rect).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new RectField(), animation, field);
                }
                else if (typeof(Bounds).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new BoundsField(), animation, field);
                }
                else if (typeof(Vector2Int).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new Vector2IntField(), animation, field);
                }
                else if (typeof(Vector3Int).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new Vector3IntField(), animation, field);
                }
                else if (typeof(RectInt).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new RectIntField(), animation, field);
                }
                else if (typeof(BoundsInt).Equals(field.FieldType))
                {
                    CreateField(propertyContainer, new BoundsIntField(), animation, field);
                }
                else if (typeof(Object).IsAssignableFrom(field.FieldType))
                {
                    CreateField(propertyContainer, new ObjectField() { objectType = field.FieldType }, animation, field);
                }
                else
                {
                    Label label = new Label();
                    label.text = $"{field.Name} cannot be displayed correctly.";
                    propertyContainer.Add(label);
                }
            }
        }

        private void CreateField<T>(VisualElement propertyContainer, BaseField<T> uieField, UIAnimation animation, FieldInfo field)
        {
            uieField.label = field.Name;
            uieField.value = (T)field.GetValue(animation);
            uieField.RegisterValueChangedCallback((e) =>
            {
                field.SetValue(animation, e.newValue);
                EditorUtility.SetDirty(animation);
            });

            propertyContainer.Add(uieField);
        }
    }
}
