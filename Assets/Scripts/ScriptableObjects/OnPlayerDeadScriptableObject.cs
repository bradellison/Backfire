using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnPlayerDeadScriptableObject", menuName = "ScriptableObjects/OnPlayerDead")]
    public class OnPlayerDeadScriptableObject : ScriptableObject
    {
        public UnityEvent onPlayerDeadEvent;

        private void OnEnable()
        {
            onPlayerDeadEvent ??= new UnityEvent();
        }

        public void OnPlayerDead()
        {
            onPlayerDeadEvent.Invoke();
        }
    }
}