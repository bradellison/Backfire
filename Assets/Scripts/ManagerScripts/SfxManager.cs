using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;
        [SerializeField] private OnWorldHitScriptableObject onWorldHitScriptableObject;
        
        
        private void Awake()
        {
            _audioSource = this.gameObject.GetComponent<AudioSource>();
            LoadPrefs();
            SetVolume();
        }

        private void OnEnable()
        {
            sfxVolumeManagerScriptableObject.sfxVolumeChangeEvent.AddListener(UpdateVolume);
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(HitPlayer);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
            onWorldHitScriptableObject.onWorldHitEvent.AddListener(HitWorld);
        }

        private void OnDisable()
        {
            sfxVolumeManagerScriptableObject.sfxVolumeChangeEvent.RemoveListener(UpdateVolume);
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(HitPlayer);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.RemoveListener(ResetPrefs);
            onWorldHitScriptableObject.onWorldHitEvent.RemoveListener(HitWorld);

        }

        private void UpdateVolume(float newVolume)
        {
            volume = newVolume; 
            SavePrefs();
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
        
        private void SavePrefs()
        {
            PlayerPrefs.SetFloat("SfxVolume", volume);
            PlayerPrefs.Save();
        }

        private void LoadPrefs()
        {
            if (PlayerPrefs.HasKey("SfxVolume"))
            {
                volume = PlayerPrefs.GetFloat("SfxVolume");
            }        
        }
        
        private void ResetPrefs()
        {
            PlayerPrefs.SetFloat("SfxVolume", 40);
            UpdateVolume(40);
        }
    }
}
