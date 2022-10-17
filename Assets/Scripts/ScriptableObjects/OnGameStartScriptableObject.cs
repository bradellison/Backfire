using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OnGameStartScriptableObject", menuName = "ScriptableObjects/OnGameStart")]
    public class OnGameStartScriptableObject : ScriptableObject
    {

        public UnityEvent onGameStartEvent;

        private void OnEnable()
        {
            onGameStartEvent ??= new UnityEvent();
        }

        public void OnGameStart()
        {
            onGameStartEvent.Invoke();
        }
    }
}