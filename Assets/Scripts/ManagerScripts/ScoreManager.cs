using CanvasScripts;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class ScoreManager : MonoBehaviour
    {
    
        public int currentScore;
        public int[] lastScores = new int[5];
        public int highScore;
        public int[] highScores = new int[5];

        private GameManager _gameManager;
        private GameplayCanvas _gameplayCanvas;
        
        [Header("Scriptable Objects")]
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;
        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
        
        private void OnEnable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(GameEnd);
            onGameStartScriptableObject.onGameStartEvent.AddListener(GameStart);
        }

        private void OnDisable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(GameEnd);
            onGameStartScriptableObject.onGameStartEvent.RemoveListener(GameStart);
        }
        
        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            currentScore = 0;
        }

        private void GameStart() {
            currentScore = 0;
        }

        public void GameEnd() {
            if(currentScore > highScores[_gameManager.levelManager.level - 1]) {
                highScores[_gameManager.levelManager.level - 1] = currentScore;
            }
            lastScores[_gameManager.levelManager.level - 1] = currentScore;
        }

        public void HitWorld() {
            currentScore += 10;
            if (!_gameplayCanvas)
            {
                _gameplayCanvas = _gameManager.canvasManager.gameplayCanvas.GetComponent<GameplayCanvas>();
            }
            _gameplayCanvas.UpdateScore();
        }
    }
}
