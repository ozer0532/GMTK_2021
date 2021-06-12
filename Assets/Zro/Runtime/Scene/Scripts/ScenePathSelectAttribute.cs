using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zro.Scenes
{
    /// <summary>
    /// <para>Draws a Scene selection popup instead of the default string field.</para>
    /// <para>Note: this attribute will save the path of the scene, so references will be lost if the scene is moved elsewhere.</para>
    /// 
    /// <code>
    /// [ScenePathSelect] public string scene;
    /// </code>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ScenePathSelectAttribute : PropertyAttribute
    {
        public ScenePathSelectAttribute() { }
    }
}
