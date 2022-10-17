using System;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        public float volume;
        
        [SerializeField]
        private MusicVolumeManagerScriptableObject musicVolumeManagerScriptableObject;
        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
            SetVolume();   
        }

        private void OnEnable()
        {
            musicVolumeManagerScriptableObject.musicVolumeChangeEvent.AddListener(UpdateVolume);
        }

        private void OnDisable()
        {
            musicVolumeManagerScriptableObject.musicVolumeChangeEvent.RemoveListener(UpdateVolume);
        }

        private void SetVolume()
        {
            _audioSource.volume = volume / 100.0f;
        }
        
        private void UpdateVolume(float newVolume)
        {
            volume = newVolume;
            SetVolume();
            
        }
    }
}
