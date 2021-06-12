using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace Zro.UI
{
    [ExecuteAlways]
    public class TabGroupController : MonoBehaviour
    {
        public int activePageIndex;
        public List<GameObject> pages = new List<GameObject>();

        private void OnValidate()
        {
            if (pages.Count > 0)
            {
                activePageIndex = Mathf.Clamp(activePageIndex, 0, pages.Count - 1);
                UpdateActiveState();
            }
            else
            {
                activePageIndex = 0;
            }
        }

        private void OnEnable()
        {
            UpdateActiveState();

#if UNITY_EDITOR
            Selection.selectionChanged += UpdateActivePageBySelection;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            Selection.selectionChanged -= UpdateActivePageBySelection;
#endif
        }

        public void ChangeActivePage(int index)
        {
            activePageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
            UpdateActiveState();
        }

        public void ChangeActivePage(GameObject page)
        {
            int newPage = pages.FindIndex(e => e == page);
            if (newPage < 0)
            {
                Debug.LogError("The GameObject you are trying to activate is not part of this Tab Group!");
                return;
            }

            activePageIndex = newPage;
            UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i])
                {
                    pages[i].SetActive(i == activePageIndex);
                }
            }
        }

#if UNITY_EDITOR
        private void UpdateActivePageBySelection()
        {
            GameObject selection = Selection.activeGameObject;
            if (!selection) return; // Skip if selection is not a gameobject

            // Check if selection is one of our pages
            for (int i = 0; i < pages.Count; i++)
            {
                if (selection.transform.IsChildOf(pages[i].transform))
                {
                    // Set it as active in the editor
                    for (int j = 0; j < pages.Count; j++)
                    {
                        pages[j].SetActive(false);
                    }

                    pages[i].SetActive(true);
                    return;
                }
            }
        }
#endif
    }
}
