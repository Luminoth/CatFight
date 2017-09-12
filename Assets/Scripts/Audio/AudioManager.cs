using CatFight.Util;

using UnityEngine;
using UnityEngine.Audio;

namespace CatFight.Audio
{
    public sealed class AudioManager : SingletonBehavior<AudioManager>
    {
        [SerializeField]
        private AudioMixer _audioMixer;

        [SerializeField]
        private AudioSource _oneShotAudioSource;

#region Unity Lifecycle
        private void Start()
        {
            // TODO: why isn't this working???
            _oneShotAudioSource.outputAudioMixerGroup = _audioMixer.outputAudioMixerGroup;
        }
#endregion

        public void PlayAudioOneShot(AudioClip audioClip)
        {
            _oneShotAudioSource.PlayOneShot(audioClip);
        }
    }
}
