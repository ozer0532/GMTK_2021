using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zro.UI
{
    public abstract class UIAnimation : ScriptableObject
    {
        /// <summary>
        /// The name of the animation (to be searched by the UI Animation Controller)
        /// </summary>
        [Tooltip("The name of the animation (to be searched by the UI Animation Controller)")]
        public string animationName;

        // Should change this to a singleton later
        public MonoBehaviour coroutineRunner;

        private bool m_isPlaying = false;
        /// <summary>
        /// Whether or not the animation is currently playing.
        /// </summary>
        public bool isPlaying 
        { 
            get => m_isPlaying;
            protected set
            {
                m_isPlaying = value;
                if (m_isPlaying)
                {
                    if (onAnimationStart != null) onAnimationStart.Invoke();
                }
                else
                {
                    if (onAnimationEnd != null) onAnimationEnd.Invoke();
                }
            } 
        }

        /// <summary>
        /// Invoked right before an animation starts.
        /// </summary>
        public UnityAction onAnimationStart;

        /// <summary>
        /// Invoked right after an animation ends.
        /// </summary>
        public UnityAction onAnimationEnd;

        /// <summary>
        /// <para>Play the animation.</para>
        /// <remarks>
        /// When overriding this method, make sure to set isPlaying to true before 
        /// playing the animation and set isPlaying to false after the animation ends.
        /// </remarks>
        /// </summary>
        /// <param name="gameObject">The GameObject this animation is attached to.</param>
        public abstract void Play(GameObject gameObject);

        /// <summary>
        /// <para>Jumps to the start of the animation.</para>
        /// <remarks>
        /// When overriding this method, make sure to set isPlaying to false before 
        /// jumping to the start of the animation.
        /// </remarks>
        /// </summary>
        /// <param name="gameObject">The GameObject this animation is attached to.</param>
        public abstract void JumpToStart(GameObject gameObject);

        /// <summary>
        /// <para>Jumps to the end of the animation.</para>
        /// <remarks>
        /// When overriding this method, make sure to set isPlaying to false before 
        /// jumping to the end of the animation.
        /// </remarks>
        /// </summary>
        /// <param name="gameObject">The GameObject this animation is attached to.</param>
        public abstract void JumpToEnd(GameObject gameObject);
    }
}
