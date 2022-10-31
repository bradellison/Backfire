using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnPreferencesResetScriptableObject", menuName = "ScriptableObjects/OnPreferencesReset")]
    public class OnPreferencesResetScriptableObject : ScriptableObject
    {

        public UnityEvent onPreferencesResetEvent;

        private void OnEnable()
        {
            onPreferencesResetEvent ??= new UnityEvent();
        }

        public void OnPreferencesReset()
        {
            onPreferencesResetEvent.Invoke();
        }
    }
}