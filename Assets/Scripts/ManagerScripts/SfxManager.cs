using System;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class SfxManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        public float volume;
        public AudioClip worldHitSound;
        public AudioClip bulletHitSound;
        
        [Header("Scriptable Objects")]
        [SerializeField] private SfxVolumeManagerScriptableObject sfxVolumeManagerScriptableObject;
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;
        
        private void Awake()
        {
            _audioSource = this.gameObject.GetComponent<AudioSource>();
            SetVolume();
        }

        private void OnEnable()
        {
            sfxVolumeManagerScriptableObject.sfxVolumeChangeEvent.AddListener(UpdateVolume);
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(HitPlayer);
        }

        private void OnDisable()
        {
            sfxVolumeManagerScriptableObject.sfxVolumeChangeEvent.RemoveListener(UpdateVolume);
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(HitPlayer);
        }

        private void UpdateVolume(float newVolume)
        {
            volume = newVolume; 
            SetVolume();
        }

        private void SetVolume()
        {
            _audioSource.volume = volume / 100.0f;
        }

        public void HitWorld() {
            _audioSource.PlayOneShot(worldHitSound);
        }

        private void HitPlayer() {
            _audioSource.PlayOneShot(bulletHitSound);
        }
    }
}
