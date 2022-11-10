using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnWorldHitScriptableObject", menuName = "ScriptableObjects/OnWorldHit")]
    public class OnWorldHitScriptableObject : ScriptableObject
    {

        public UnityEvent onWorldHitEvent;

        private void OnEnable()
        {
            onWorldHitEvent ??= new UnityEvent();
        }

        public void OnWorldHit()
        {
            onWorldHitEvent.Invoke();
        }
    }
}