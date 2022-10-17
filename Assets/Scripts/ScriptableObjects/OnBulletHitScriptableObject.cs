using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnBulletHitScriptableObject", menuName = "ScriptableObjects/OnBulletHit")]
    public class OnBulletHitScriptableObject : ScriptableObject
    {

        public UnityEvent onBulletHitEvent;

        private void OnEnable()
        {
            onBulletHitEvent ??= new UnityEvent();
        }

        public void OnBulletHit()
        {
            onBulletHitEvent.Invoke();
        }
    }
}