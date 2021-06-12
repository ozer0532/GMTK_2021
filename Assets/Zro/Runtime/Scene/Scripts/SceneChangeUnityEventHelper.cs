using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zro.Scenes
{
    public class SceneChangeUnityEventHelper : MonoBehaviour
    {
        [ScenePathSelect]
        public string sceneToChangeTo;

        public void ChangeScene()
        {
            GameSceneManager.ChangeScene(sceneToChangeTo);
        }
    }
}
