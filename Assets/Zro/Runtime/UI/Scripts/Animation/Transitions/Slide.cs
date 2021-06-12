using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zro.UI
{
    public class Slide : UITransition
    {
        [Header("Timing Options")]
        public float duration;

        [Header("Transition Options")]
        public bool relativeToObject = false;
        public Vector3 from;
        public Vector3 to;

        private IEnumerator currentAnimation;
        private Vector3 startPos, endPos;

        public override void Play(GameObject gameObject)
        {
            isPlaying = true;

            currentAnimation = DoTransition(gameObject.transform);
            coroutineRunner.StartCoroutine(currentAnimation);
        }

        public override void JumpToStart(GameObject gameObject)
        {
            isPlaying = false;
            if (currentAnimation != null)
            {
                coroutineRunner.StopCoroutine(currentAnimation);
                gameObject.transform.localPosition = startPos;
            }
            else
            {
                if (relativeToObject)
                {
                    // Do nothing
                }
                else
                {
                    gameObject.transform.localPosition = from;
                }
            }
        }

        public override void JumpToEnd(GameObject gameObject)
        {
            isPlaying = false;
            if (currentAnimation != null)
            {
                coroutineRunner.StopCoroutine(currentAnimation);
                gameObject.transform.localPosition = endPos;
            }
            else
            {
                if (relativeToObject)
                {
                    gameObject.transform.localPosition += to - from;
                }
                else
                {
                    gameObject.transform.localPosition = to;
                }
            }
        }

        private IEnumerator DoTransition(Transform transform)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            // Get the position
            if (relativeToObject)
            {
                Vector3 direction = to - from;
                startPos = transform.localPosition;
                endPos = startPos + direction;
            }
            else
            {
                startPos = from;
                endPos = to;
            }

            while (endTime > Time.time)
            {
                float currentTime = Time.time;
                float elapsedTime = currentTime - startTime;
                transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

                yield return null;
            }

            transform.localPosition = endPos;

            isPlaying = false;
        }
    }
}
