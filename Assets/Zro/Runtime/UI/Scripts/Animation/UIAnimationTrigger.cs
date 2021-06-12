using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zro.UI
{
    [System.Serializable]
    public class UIAnimationTrigger
    {
        public List<UIAnimation> animations;

        public bool isPlaying { get; private set; } = false;

        public void Play(GameObject gameObject)
        {
            isPlaying = true;

            foreach (UIAnimation anim in animations)
            {
                UnityAction handler = null;
                handler = () =>
                {
                    anim.onAnimationEnd -= handler;
                    OnAnimationEnd();
                };
                anim.onAnimationEnd += handler;

                anim.Play(gameObject);
            }
        }

        public void JumpToStart(GameObject gameObject)
        {
            isPlaying = false;

            foreach (UIAnimation anim in animations)
            {
                anim.JumpToStart(gameObject);
            }
        }

        public void JumpToEnd(GameObject gameObject)
        {
            isPlaying = false;

            foreach (UIAnimation anim in animations)
            {
                anim.JumpToEnd(gameObject);
            }
        }

        private void OnAnimationEnd()
        {
            // Check if an animation is still playing
            foreach (UIAnimation animation in animations)
            {
                if (animation.isPlaying) return;
            }

            isPlaying = false;
        }
    }
}
