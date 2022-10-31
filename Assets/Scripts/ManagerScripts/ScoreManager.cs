using CanvasScripts;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class ScoreManager : MonoBehaviour
    {
    
        public int currentScore;
        [SerializeField]
        public int[] lastScores = new int[5];
        public int highScore;
        [SerializeField]
        public int[] highScores = new int[5];

        private GameManager _gameManager;
        private GameplayCanvas _gameplayCanvas;
        
        [Header("Scriptable Objects")]
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;
        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
        [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;

        private void Awake()
        {
            LoadPrefs();
        }
        
        private void OnEnable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(GameEnd);
            onGameStartScriptableObject.onGameStartEvent.AddListener(GameStart);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
        }

        private void OnDisable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(GameEnd);
            onGameStartScriptableObject.onGameStartEvent.RemoveListener(GameStart);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.RemoveListener(ResetPrefs);
        }
        
        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            currentScore = 0;
            _gameplayCanvas = _gameManager.canvasManager.gameplayCanvas.GetComponent<GameplayCanvas>();
        }

        private void GameStart() {
            currentScore = 0;
            _gameplayCanvas.UpdateScore(currentScore);
        }

        public void GameEnd() {
            if(currentScore > highScores[_gameManager.levelManager.level - 1])
            {
                highScores[_gameManager.levelManager.level - 1] = currentScore;
            }
            lastScores[_gameManager.levelManager.level - 1] = currentScore;
            SavePrefs();
        }

        public void HitWorld() {
            currentScore += 10;
            _gameplayCanvas.UpdateScore(currentScore);
        }

        private void SavePrefs()
        {
            PlayerPrefs.SetInt("HighScore1", highScores[0]);
            PlayerPrefs.SetInt("HighScore2", highScores[1]);
            PlayerPrefs.SetInt("HighScore3", highScores[2]);
            PlayerPrefs.SetInt("HighScore4", highScores[3]);
            PlayerPrefs.SetInt("HighScore5", highScores[4]);
            PlayerPrefs.Save();
        }

        private void LoadPrefs()
        {
            if (!PlayerPrefs.HasKey("HighScore1")) { return; }
            highScores[0] = PlayerPrefs.GetInt("HighScore1");
            highScores[1] = PlayerPrefs.GetInt("HighScore2");
            highScores[2] = PlayerPrefs.GetInt("HighScore3");
            highScores[3] = PlayerPrefs.GetInt("HighScore4");
            highScores[4] = PlayerPrefs.GetInt("HighScore5");            
        }

        private void ResetPrefs()
        {
            for (int i = 0; i < highScores.Length; i++)
            {
                highScores[i] = 0;
            }
            SavePrefs();
        }
    }
}
