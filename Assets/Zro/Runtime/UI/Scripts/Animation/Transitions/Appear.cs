using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zro.UI
{
    public class Appear : UITransition
    {
        [Tooltip("Set this to true to disappear instead of appear.")]
        public bool disappear = false;

        public override void Play(GameObject gameObject)
        {
            isPlaying = true;
            gameObject.SetActive(!disappear);
            isPlaying = false;
        }

        public override void JumpToStart(GameObject gameObject)
        {
            isPlaying = false;
            gameObject.SetActive(disappear);
        }

        public override void JumpToEnd(GameObject gameObject)
        {
            isPlaying = false;
            gameObject.SetActive(!disappear);
        }
    }
}
