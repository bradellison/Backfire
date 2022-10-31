using System;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        public float volume;
        
        [SerializeField] private MusicVolumeManagerScriptableObject musicVolumeManagerScriptableObject;
        [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;
        
        private void Awake()
        {
            LoadPrefs();
            _audioSource = this.GetComponent<AudioSource>();
            SetVolume();   
        }

        private void OnEnable()
        {
            musicVolumeManagerScriptableObject.musicVolumeChangeEvent.AddListener(UpdateVolume);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
        }

        private void OnDisable()
        {
            musicVolumeManagerScriptableObject.musicVolumeChangeEvent.RemoveListener(UpdateVolume);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.RemoveListener(ResetPrefs);
        }

        private void SetVolume()
        {
            _audioSource.volume = volume / 100.0f;
        }
        
        private void UpdateVolume(float newVolume)
        {
            volume = newVolume;
            SavePrefs();
            SetVolume();
        }

        private void SavePrefs()
        {
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }

        private void LoadPrefs()
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                volume = PlayerPrefs.GetFloat("MusicVolume");
            }
        }
        
        private void ResetPrefs()
        {
            PlayerPrefs.SetFloat("MusicVolume", 40);
            UpdateVolume(40);
        }
    }
}
