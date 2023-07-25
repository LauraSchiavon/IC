using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace JigsawPuzzlesCollection.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        [System.Serializable]
        public class Sound
        {
            public AudioClip clip;
            [HideInInspector]
            public int simultaneousPlayCount;
        }

        [Header("Max number allowed of same sounds playing together")]
        public int maxSimultaneousSounds = 7;

        public Sound Button;
        public Sound LevelCompleted;
        public Sound ShufflePieces;
        public Sound[] BackgroundMusic;

        private AudioSource m_audioSource;
        private AudioSource m_audioSourceMusic;
        private Random m_random = new Random();

        private const string c_mutePrefKey = "MutePreference";
        private const string c_musicMutePrefKey = "MusicMutePreference";
        private const string c_volumePrefKey = "VolumePreference";
        private const int c_muted = 1;
        private const int c_unMuted = -1;

        public delegate void OnMuteStatusChanged(bool isMuted);

        public static event OnMuteStatusChanged MuteStatusChanged;

        protected override void OnAwake()
        {
            // Get audio source component
            var audioSources = GetComponents<AudioSource>();
            m_audioSource = audioSources[0];
            m_audioSourceMusic = audioSources[1];
        }

        private void Start()
        {
            // Set mute based on the valued stored in PlayerPrefs
            SetMute(IsMuted());
            SetMusicMute(IsMusicMuted());

            var volume = GetVolume();
            m_audioSource.volume = volume;
            m_audioSourceMusic.volume = volume;

        }

        private void Update()
        {
            if (!m_audioSourceMusic.isPlaying &&
                !m_audioSourceMusic.mute)
            {
                PlayRandomBackgroundMusic();
            }
        }

        /// <summary>
        /// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
        /// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
        /// </summary>
        /// <param name="sound">Sound.</param>
        /// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
        /// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
        public void PlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (sound == null)
            {
                return;
            }
            StartCoroutine(CRPlaySound(sound, autoScaleVolume, maxVolumeScale));
        }

        private IEnumerator CRPlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
            {
                yield break;
            }

            sound.simultaneousPlayCount++;

            float vol = maxVolumeScale;

            // Scale down volume of same sound played subsequently
            if (autoScaleVolume && sound.simultaneousPlayCount > 0)
            {
                vol = vol / sound.simultaneousPlayCount;
            }

            m_audioSource.PlayOneShot(sound.clip, vol);

            // Wait til the sound almost finishes playing then reduce play count
            float delay = sound.clip.length * 0.7f;

            yield return new WaitForSeconds(delay);

            sound.simultaneousPlayCount--;
        }

        public void PlayRandomBackgroundMusic()
        {
            if (BackgroundMusic == null || BackgroundMusic.Length == 0)
            {
                return;
            }

            var randomIndex = m_random.Next(BackgroundMusic.Length);
            var music = BackgroundMusic[randomIndex];
            m_audioSourceMusic.clip = music.clip;
            m_audioSourceMusic.Play();

        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void Stop()
        {
            m_audioSource.Stop();
        }

        public bool IsMuted()
        {
            return PlayerPrefs.GetInt(c_mutePrefKey, c_unMuted) == c_muted;
        }

        public bool IsMusicMuted()
        {
            return PlayerPrefs.GetInt(c_musicMutePrefKey, c_unMuted) == c_muted;
        }

        public float GetVolume()
        {
            return PlayerPrefs.GetFloat(c_volumePrefKey, 0.5f);
        }

        public void SetSounds(bool mute)
        {
            // Un-muted
            PlayerPrefs.SetInt(c_mutePrefKey, mute ? c_muted : c_unMuted);

            MuteStatusChanged?.Invoke(mute);

            SetMute(mute);
        }

        public void SetMusic(bool mute)
        {
            // Un-muted
            PlayerPrefs.SetInt(c_musicMutePrefKey, mute ? c_muted : c_unMuted);

            MuteStatusChanged?.Invoke(mute);

            SetMusicMute(mute);
        }

        public void SetVolume(float value)
        {
            PlayerPrefs.SetFloat(c_volumePrefKey, value);

            m_audioSourceMusic.volume = value;
            m_audioSource.volume = value;
        }

        private void SetMute(bool isMuted)
        {
            m_audioSource.mute = isMuted;
        }

        private void SetMusicMute(bool isMuted)
        {
            m_audioSourceMusic.mute = isMuted;
            if (isMuted)
            {
                m_audioSourceMusic.Stop();
            }
            else
            {
                m_audioSourceMusic.Play();
            }
        }
    }

}