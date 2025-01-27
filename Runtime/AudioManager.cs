using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


namespace Utilities
{
    public class AudioManager : GenericSingletonClass<AudioManager>
    {
        [Header("Sounds")]
        public List<SoundCategory> soundList;
        public int musicCategoryID;

        [Header("Audio Sources")]
        public List<AudioSource> audioSources;

        [Header("Private Infos")]
        private List<AudioSource> musicAudioSources = new();
        private List<float> musicAudioSourcesOriginalVolumes = new();


        [Header("Volume")]
        [Range(0f, 1f)][SerializeField] private float masterVolume;
        [Range(0f, 1f)][SerializeField] private float musicVolume;
        [Range(0f, 1f)][SerializeField] private float sfxVolume;


        public void SetAudioSource(int index, AudioSource audioSource)
        {
            if (index > audioSources.Count)
                audioSources.Add(audioSource);

            else
                audioSources[index] = audioSource;
        }


        #region Play Sounds Functions

        /// <summary>
        /// WILL THE PLAY THE SOUND ONLY ONE TIME WITH NORMAL PITCH
        /// </summary>
        public void PlaySoundOneShot(int categoryId, int soundId, int audioSourceId = 0, AudioSource audioSource = null)
        {
            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            currentAudioSource.PlayOneShot(soundList[categoryId].listSoundIdentities[soundId].audioClip,
                soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * sfxVolume);

            for(int i = 0; i < musicAudioSources.Count; i++)
            {
                if (musicAudioSources[i] == currentAudioSource)
                {
                    musicAudioSources.RemoveAt(i);
                    musicAudioSourcesOriginalVolumes.RemoveAt(i);
                }
            }

            currentAudioSource.loop = false;

            currentAudioSource.volume = soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * sfxVolume;
        }

        /// <summary>
        /// WILL PLAY THE SOUND ENDLESSLY 
        /// </summary>
        public void PlaySoundContinuous(int categoryId, int soundId, int audioSourceId = 0, AudioSource audioSource = null)
        {
            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            currentAudioSource.clip = soundList[categoryId].listSoundIdentities[soundId].audioClip;

            currentAudioSource.loop = true;

            currentAudioSource.Play();

            if (!musicAudioSources.Contains(currentAudioSource) && musicCategoryID == categoryId)
            {
                musicAudioSources.Add(currentAudioSource);
                musicAudioSourcesOriginalVolumes.Add(soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * musicVolume);
            }

            if (musicCategoryID == categoryId)
                currentAudioSource.volume = soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * musicVolume;

            else
                currentAudioSource.volume = soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * sfxVolume;
        }

        /// <summary>
        /// WILL PLAY THE SOUND ONE TIME AND WITH A RANDOM PITCH VARIATION
        /// </summary>
        public void PlaySoundOneShotRandomPitch(float pitchRangeMin, float pitchRangeMax, int categoryId, int soundId, int audioSourceId = 0, AudioSource audioSource = null)
        {
            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            currentAudioSource.pitch = Random.Range(pitchRangeMin, pitchRangeMax);

            currentAudioSource.PlayOneShot(soundList[categoryId].listSoundIdentities[soundId].audioClip,
                soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * sfxVolume);
            //currentAudioSource.clip = soundList[categoryId].listSoundIdentities[soundId].audioClip;
            //currentAudioSource.pitch = 1;
        }


        public void PlaySoundOneShotFadingIn(float timeToFade, int categoryId, int soundId, int audioSourceId = 0, AudioSource audioSource = null)
        {
            PlaySoundOneShot(categoryId, soundId, audioSourceId, audioSource);

            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            float wantedVolume = currentAudioSource.volume;
            currentAudioSource.volume = 0;

            StartCoroutine(FadeValue(0, wantedVolume, timeToFade, currentAudioSource));
        }

        public void PlaySoundContinuousFadingIn(float timeToFade, int categoryId, int soundId, int audioSourceId = 0, AudioSource audioSource = null)
        {
            PlaySoundContinuous(categoryId, soundId, audioSourceId, audioSource);

            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            float wantedVolume = 0;
            if (musicCategoryID == categoryId)
                wantedVolume = soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * musicVolume;

            else
                wantedVolume = soundList[categoryId].listSoundIdentities[soundId].volume * masterVolume * sfxVolume;

            currentAudioSource.volume = 0;

            currentAudioSource.loop = true;

            StartCoroutine(FadeValue(0, wantedVolume, timeToFade, currentAudioSource));
        }


        public void FadeOutAudioSource(float timeToFade, int audioSourceId, AudioSource audioSource = null)
        {
            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            StartCoroutine(FadeValue(currentAudioSource.volume, 0, timeToFade, currentAudioSource));
        }

        public void FadeInAudioSource(float timeToFade, int wantedVolume, int audioSourceId, AudioSource audioSource = null)
        {
            AudioSource currentAudioSource = audioSource is not null ? audioSource : audioSources[audioSourceId];
            StartCoroutine(FadeValue(0, wantedVolume, timeToFade, currentAudioSource));
        }


        private IEnumerator FadeValue(float start, float wantedValue, float timeToFade, AudioSource audioSource)
        {
            float timer = 0;

            while (timer < timeToFade)
            {
                timer += Time.deltaTime;

                audioSource.volume = Mathf.Lerp(start, wantedValue, timer / timeToFade);

                yield return null;
            }

            audioSource.volume = wantedValue;
        }

        #endregion


        #region Volumes Functions

        public void SetMasterVolume(float newMasterVolume)
        {
            masterVolume = newMasterVolume;

            for(int i = 0; i < musicAudioSources.Count; i++)
            {
                musicAudioSources[i].volume = musicAudioSourcesOriginalVolumes[i] * masterVolume * musicVolume;
            }
        }

        public void SetSfxVolume(float newSfxVolume)
        {
            sfxVolume = newSfxVolume;
        }

        public void SetMusicVolume(float newMusicVolume)
        {
            musicVolume = newMusicVolume;

            for (int i = 0; i < musicAudioSources.Count; i++)
            {
                musicAudioSources[i].volume = musicAudioSourcesOriginalVolumes[i] * masterVolume * musicVolume;
            }
        }

        #endregion

    }

    [Serializable]
    public class SoundCategory
    {
        public List<SoundIdentity> listSoundIdentities;
    }

    [Serializable]
    public class SoundIdentity
    {
        public AudioClip audioClip;
        [Range(0, 5)] public float volume = 1;
    }

}
