using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Zro.Audio
{
    public class GameAudioPlayer : MonoBehaviour
    {
        public int oneShotMixerIndex;
        public List<OneShotMixerInfo> oneShotMixers = new List<OneShotMixerInfo>();
        public int musicMixerIndex;
        public List<MusicMixerInfo> musicMixers = new List<MusicMixerInfo>();

        public static GameAudioPlayer main;

        // Get the default audio source
        public static AudioSource oneShotSource { get => main ? main.oneShotMixers[main.oneShotMixerIndex].audioSource : null; }

        private bool isMain = false;

        private void OnValidate()
        {
            if (oneShotMixers.Count > 0)
            {
                oneShotMixerIndex = Mathf.Clamp(oneShotMixerIndex, 0, oneShotMixers.Count - 1);
            }
            else
            {
                oneShotMixerIndex = 0;
            }

            if (musicMixers.Count > 0)
            {
                musicMixerIndex = Mathf.Clamp(musicMixerIndex, 0, musicMixers.Count - 1);
            }
            else
            {
                musicMixerIndex = 0;
            }
        }

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

        #region PlayOneShot

        public static void PlayOneShot(AudioClip clip)
        {
            PlayOneShot(clip, oneShotSource, 1f);
        }

        public static void PlayOneShot(AudioClip clip, float volumeScale)
        {
            PlayOneShot(clip, oneShotSource, volumeScale);
        }

        public static void PlayOneShot(AudioClip clip, AudioMixerGroup group, float volumeScale = 1f)
        {
            // Find the appropriate source
            if (!main)
            {
                Debug.LogError("You cannot play a global audio without an enabled Game Audio Player!");
                return;
            }

            OneShotMixerInfo oneShotMixerInfo = main.oneShotMixers.Where(e => e.mixerGroup == group).FirstOrDefault();

            if (string.IsNullOrEmpty(oneShotMixerInfo.mixerGroupName))
            {
                Debug.LogError($"The audio mixer group \"{group.name}\" does not have an audio source assigned! " +
                    $"Try assigning it first to your main Game Audio Player.");
                return;
            }

            PlayOneShot(clip, oneShotMixerInfo.audioSource, volumeScale);
        }

        private static void PlayOneShot(AudioClip clip, AudioSource source, float volumeScale)
        {
            if (!main)
            {
                Debug.LogError("You cannot play a global audio without an enabled Game Audio Player!");
                return;
            }

            oneShotSource.PlayOneShot(clip, volumeScale);
        }

        #endregion

        #region PlayMusic

        public static void PlayMusic(AudioClip clip)
        {
            PlayMusic(clip, main ? main.musicMixerIndex : 0, 0f);
        }

        public static void PlayMusic(AudioClip clip, float fadeDuration)
        {
            PlayMusic(clip, main ? main.musicMixerIndex : 0, fadeDuration);
        }

        public static void PlayMusic(AudioClip clip, AudioMixerGroup group, float fadeDuration = 0)
        {
            // Find the appropriate source
            if (!main)
            {
                Debug.LogError("You cannot play a global audio without an enabled Game Audio Player!");
                return;
            }

            int index = main.musicMixers.FindIndex(e => e.mixerGroup == group);

            if (index == -1)
            {
                Debug.LogError($"The audio mixer group \"{group.name}\" does not have an audio source assigned! " +
                    $"Try assigning it first to your main Game Audio Player.");
                return;
            }

            PlayMusic(clip, index, fadeDuration);
        }

        private static void PlayMusic(AudioClip clip, int index, float fadeDuration)
        {
            if (!main)
            {
                Debug.LogError("You cannot play a global audio without an enabled Game Audio Player!");
                return;
            }

            if (fadeDuration < 0f)
            {
                Debug.LogError("Music fade duration should be more than or equal to 0!");
                return;
            }

            if (fadeDuration == 0)
            {
                main.musicMixers[index].PlayInstant(clip);
            }
            else
            {
                main.musicMixers[index].FadeTo(clip, main, fadeDuration);
            }
        }

        #endregion

        [System.Serializable]
        public class OneShotMixerInfo
        {
            public string mixerGroupName;
            public AudioMixerGroup mixerGroup;
            public AudioSource audioSource;
        }

        [System.Serializable]
        public class MusicMixerInfo
        {
            public string mixerGroupName;
            public AudioMixerGroup mixerGroup;
            public AudioSource audioSource1;
            public AudioSource audioSource2;

            public bool isTransitioning { get; private set; }

            private bool firstSourceActive;
            private IEnumerator transition;

            public void PlayInstant(AudioClip clip)
            {
                AudioSource audioSource = firstSourceActive ? audioSource1 : audioSource2;

                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }

            public void FadeTo(AudioClip clip, GameAudioPlayer player, float duration)
            {
                if (isTransitioning)
                {
                    Debug.LogWarning("A music transition is running! Skipping the existing transition.");
                    player.StopCoroutine(transition);
                }

                isTransitioning = true;

                AudioSource fromSource = firstSourceActive ? audioSource1 : audioSource2;
                AudioSource toSource = firstSourceActive ? audioSource2 : audioSource1;

                toSource.clip = clip;

                transition = Fade(fromSource, toSource, duration);
                player.StartCoroutine(transition);

                firstSourceActive = !firstSourceActive;
            }

            private IEnumerator Fade(AudioSource fromSource, AudioSource toSource, float duration)
            {
                float endTime = Time.time + duration;

                fromSource.volume = 1;
                toSource.volume = 0;

                toSource.Play();

                while (Time.time < endTime)
                {
                    float t = (endTime - Time.time) / duration;

                    fromSource.volume = Mathf.Clamp01(t);
                    toSource.volume = Mathf.Clamp01(1 - t);

                    yield return null;
                }

                fromSource.Stop();

                fromSource.volume = 0;
                toSource.volume = 1;

                isTransitioning = false;
            }
        }
    }
}
