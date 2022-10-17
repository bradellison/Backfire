using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BackgroundResolutionManagerScriptableObject", menuName = "ScriptableObjects/BackgroundResolutionManager")]
    public class BackgroundResolutionManagerScriptableObject : ScriptableObject
    {

        public UnityEvent<int> backgroundResolutionChangeEvent;

        private void OnEnable()
        {
            backgroundResolutionChangeEvent ??= new UnityEvent<int>();
        }

        public void SetBackgroundResolution(int value)
        {
            backgroundResolutionChangeEvent.Invoke(value);
        }
    }
}