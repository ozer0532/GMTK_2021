using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zro.UI
{
    public class Popup : MonoBehaviour
    {
        [Header("Popup Settings")]
        //public bool startEnabled = false;
        public bool blockOffScreen = true;
        public bool closeOffScreen = true;

        [Header("Animations")]
        public UIAnimationTrigger appearAnimations;
        public UIAnimationTrigger disappearAnimations;

        [Header("References")]
        public Image blockerImage;
        public EventTrigger blockerTrigger;

        private void OnEnable()
        {
            blockerImage.raycastTarget = blockOffScreen;
            blockerTrigger.enabled = closeOffScreen;
        }

        public void Enable()
        {
            appearAnimations.Play(gameObject);
        }

        public void Disable()
        {
            disappearAnimations.Play(gameObject);
        }
    }
}
