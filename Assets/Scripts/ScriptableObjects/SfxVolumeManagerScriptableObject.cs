using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SfxVolumeManagerScriptableObject", menuName = "ScriptableObjects/SfxVolumeManager")]
    public class SfxVolumeManagerScriptableObject : ScriptableObject
    {

        public UnityEvent<float> sfxVolumeChangeEvent;

        private void OnEnable()
        {
            sfxVolumeChangeEvent ??= new UnityEvent<float>();
        }

        public void SetSfxVolume(float newVolume)
        {
            sfxVolumeChangeEvent.Invoke(newVolume);
        }
    }
}