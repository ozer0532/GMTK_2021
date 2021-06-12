using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zro.Audio;

namespace Zro.Examples
{
    public class MusicFadeExample : MonoBehaviour
    {
        public float duration;
        public AudioClip clip;

        public void PlayAudio()
        {
            GameAudioPlayer.PlayMusic(clip, duration);
        }
    }
}
