using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zro.UI
{
    public class UIAnimationController : MonoBehaviour
    {
        public List<UIAnimation> animations = new List<UIAnimation>();

        public void Play()
        {
            animations[0].Play(gameObject);
        }

        public void Play(int animationIndex)
        {
            animations[animationIndex].Play(gameObject);
        }

        public void Play(string animationName)
        {
            animations.Find(e => e.animationName == animationName).Play(gameObject);
        }

        public void Play(UIAnimation animation)
        {
            animation.Play(gameObject);
        }
    }
}
