using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Zro.Scenes.Editor
{

    [CustomPropertyDrawer(typeof(ScenePathSelectAttribute))]
    public class ScenePathSelectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Only apply custom property if this is a string
            if (property.propertyType == SerializedPropertyType.String)
            {
                // Get the scene asset
                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

                // Check if scene is in the build settings
                bool inBuildSettings = scene == null || IsInBuildSettings(property.stringValue);

                // Set the field position
                position.Set(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.BeginChangeCheck();

                // Draws the main Scene property
                scene = (SceneAsset)EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), false);

                // Draws the 'Add to Build Settings' button if necessary
                if (!inBuildSettings)
                {
                    // Set the button position
                    position.Set(position.x, position.y + position.height + EditorGUIUtility.standardVerticalSpacing, position.width, position.height);

                    if (GUI.Button(position, "Add to Build Settings"))
                    {
                        if (scene)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(scene);

                            // Add the scene to the build settings if it is valid
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                List<EditorBuildSettingsScene> editorScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                                editorScenes.Add(new EditorBuildSettingsScene(assetPath, true));
                                EditorBuildSettings.scenes = editorScenes.ToArray();
                            }
                        }
                    }
                }

                // Check for changes
                if (EditorGUI.EndChangeCheck())
                {
                    // Check if any scene is selected
                    if (scene)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(scene);
                        property.stringValue = assetPath;
                    }
                    // Set string to empty otherwise
                    else
                    {
                        property.stringValue = string.Empty;
                    }
                }
            }
            // Draw the default property otherwise
            else
            {
                EditorGUI.PropertyField(position, property);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                // Get the scene asset
                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

                // Check if scene is in the build settings
                bool inBuildSettings = scene == null || IsInBuildSettings(property.stringValue);

                // Add space for the button
                if (!inBuildSettings)
                    return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
            }
            return base.GetPropertyHeight(property, label);
        }

        private bool IsInBuildSettings(string assetPath)
        {
            return EditorBuildSettings.scenes.Where(s => s.path == assetPath).FirstOrDefault() != null;
        }
    }
}
