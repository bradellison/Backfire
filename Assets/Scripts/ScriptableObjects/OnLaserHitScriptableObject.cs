using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnLaserHitScriptableObject", menuName = "ScriptableObjects/OnLaserHit")]
    public class OnLaserHitScriptableObject : ScriptableObject
    {

        public UnityEvent onLaserHitEvent;

        private void OnEnable()
        {
            onLaserHitEvent ??= new UnityEvent();
        }

        public void OnLaserHit()
        {
            onLaserHitEvent.Invoke();
        }
    }
}