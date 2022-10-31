using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class CanvasManager : MonoBehaviour
    {

    
        public GameObject titleCanvas;
        public GameObject gameplayCanvas;
        public GameObject gameOverCanvas;
        public GameObject settingsMenuCanvas;
        public GameObject debugCanvas;
        
        [Header("Scriptable Objects")]
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;

        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
        

        private void OnEnable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(GameOverToggle);
            onGameStartScriptableObject.onGameStartEvent.AddListener(GameStartToggle);
        }

        private void OnDisable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(GameOverToggle);
            onGameStartScriptableObject.onGameStartEvent.RemoveListener(GameStartToggle);
        }
        private void Start()
        {
            titleCanvas.SetActive(true);
        }

        private void GameStartToggle() {
            titleCanvas.SetActive(false);
            gameplayCanvas.SetActive(true);
        } 

        public void GameOverToggle() {
            gameOverCanvas.SetActive(true);
        }

        public void ToggleDebugCanvas()
        {
            //if(debugCanvas.activeSelf)
            debugCanvas.SetActive(!debugCanvas.activeSelf);
        }
        
        public void OpenSettingsMenu() {
            titleCanvas.SetActive(false);
            settingsMenuCanvas.SetActive(true);
        }

        public void CloseSettingsMenu() {
            settingsMenuCanvas.SetActive(false);
            titleCanvas.SetActive(true);
        }

        public void GameEndToggle()
        {
            gameplayCanvas.SetActive(false);
            gameOverCanvas.SetActive(false);
            titleCanvas.SetActive(true);
        }
    }
}
