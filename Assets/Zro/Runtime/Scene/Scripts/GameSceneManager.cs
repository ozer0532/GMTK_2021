using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Zro.Scenes
{
    public class GameSceneManager : MonoBehaviour
    {
        //public UnityEvent sceneLoadedEvent;
        // public UnityEvent sceneUnloadingEvent;
        // public UnityAction onSceneLoaded;
        // public UnityAction onSceneUnloading;

        public static GameSceneManager main;

        public static float loadProgress;

        public static bool _isLoading { get; private set; } = false;

        private static List<AsyncOperation> loadingScenes = new List<AsyncOperation>();

        private bool isMain = false;

        private void OnEnable()
        {
            if (!main)
            {
                isMain = true;
                main = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDisable()
        {
            if (isMain)
            {
                isMain = false;
                main = null;
                Destroy(gameObject);
            }
        }

        public static void ChangeScene(string sceneName)
        {
            ChangeScene(sceneName, true);
        }

        public static void ChangeScene(string sceneName, bool loadImmediately)
        {
            if (_isLoading)
            {
                Debug.LogError($"You cannot load \"{sceneName}\" when another scene is currently loading!");
                return;
            }
            
            if (!Application.CanStreamedLevelBeLoaded(sceneName)) 
            {
                Debug.LogError($"Scene \"{sceneName}\" does not exist! You may need to add it to the build settings.");
                return;
            }

            loadingScenes.Clear();
            loadingScenes.Add(SceneManager.LoadSceneAsync(sceneName));

            if (!loadImmediately)
            {
                loadingScenes[0].allowSceneActivation = false;
            }

            if (main)
            {
                main.StartCoroutine(TrackSceneLoadProgress());
            }
            else
            {
                Debug.LogWarning("Loading progress will not be tracked without an enabled Game Scene Manager!");
            }

        }

        private static IEnumerator TrackSceneLoadProgress()
        {
            for (int i = 0; i < loadingScenes.Count; i++)
            {
                while (!loadingScenes[i].isDone)
                {
                    loadProgress = 0;

                    foreach (AsyncOperation operation in loadingScenes)
                    {
                        loadProgress += operation.progress;
                    }

                    loadProgress /= loadingScenes.Count;

                    yield return null;
                }
            }
        }
    }
}

