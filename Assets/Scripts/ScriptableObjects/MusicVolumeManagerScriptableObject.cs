using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MusicVolumeManagerScriptableObject", menuName = "ScriptableObjects/MusicVolumeManager")]
    public class MusicVolumeManagerScriptableObject : ScriptableObject
    {

        public UnityEvent<float> musicVolumeChangeEvent;

        private void OnEnable()
        {
            musicVolumeChangeEvent ??= new UnityEvent<float>();
        }

        public void SetMusicVolume(float newVolume)
        {
            musicVolumeChangeEvent.Invoke(newVolume);
        }
    }
}
