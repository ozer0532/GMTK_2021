using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zro.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class CarouselController : UIBehaviour
    {
        // TODO: Add multi-item per display support
        // TODO: Make loopable

        [Header("States")]
        [SerializeField] private int m_currentIndex;
        public int currentIndex 
        { 
            get { return m_currentIndex; } 
            private set { m_currentIndex = value % transform.childCount; } 
        }

        [Header("Animations")]
        public UIAnimationTrigger nextEnterAnimation;
        public UIAnimationTrigger nextExitAnimation;
        public UIAnimationTrigger prevEnterAnimation;
        public UIAnimationTrigger prevExitAnimation;

        [System.NonSerialized] private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (transform.childCount > 0)
            {
                m_currentIndex = Mathf.Clamp(m_currentIndex, 0, transform.childCount - 1);
            }
            else
            {
                m_currentIndex = 0;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateChildInstant();
        }

        public void UpdateChildInstant()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                // Place left of carousel
                if (i < currentIndex)
                {
                    nextExitAnimation.JumpToEnd(child);
                }
                // Place right of carousel
                else if (i > currentIndex)
                {
                    nextEnterAnimation.JumpToStart(child);
                }
                // Place at center of carousel
                else
                {
                    nextEnterAnimation.JumpToEnd(child);
                }
            }
        }

        public void Next()
        {
            nextExitAnimation.Play(transform.GetChild(currentIndex).gameObject);

            int childCount = transform.childCount;
            currentIndex = (currentIndex + 1) % childCount;

            nextEnterAnimation.Play(transform.GetChild(currentIndex).gameObject);
        }

        public void Previous()
        {
            prevExitAnimation.Play(transform.GetChild(currentIndex).gameObject);

            int childCount = transform.childCount;
            currentIndex = (currentIndex + childCount - 1) % childCount;

            prevEnterAnimation.Play(transform.GetChild(currentIndex).gameObject);
        }
    }
}
