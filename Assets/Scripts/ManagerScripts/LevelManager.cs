using CanvasScripts;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class LevelManager : MonoBehaviour
    {
    
        public int level;
        public bool[] levelsUnlocked;
        public int levelUnlockThreshold;

        private int _minLevel;
        private int _maxLevel;

        private OuterSpace _outerSpace;
        private GameManager _gameManager;

        [Header("Scriptable Objects")]
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;

        private void OnEnable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(GameOver);
        }

        private void OnDisable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(GameOver);
        }
        private void Awake()
        {
            levelsUnlocked = new []{true, false, false, false, false};

            _minLevel = 1;
            _maxLevel = 5;
            level = 1;

            _outerSpace = GameObject.FindGameObjectWithTag("OuterSpace").GetComponent<OuterSpace>();
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }


        private void UpdateLevel(int newLevel) {
            level = newLevel;
            _outerSpace.ChangeBackground(level);
        
            if (!_gameManager.canvasManager.titleCanvas) return;
            TitleCanvas titleCanvas = _gameManager.canvasManager.titleCanvas.GetComponent<TitleCanvas>();
            titleCanvas.CheckLevelUnlockText();
            titleCanvas.UpdateTextFields();
        }

        public void IncreaseLevel() {
            int newLevel = level + 1;
            if(level < _maxLevel) {
                UpdateLevel(newLevel);
            }
        }

        public void DecreaseLevel() {
            int newLevel = level - 1;
            if(level > _minLevel) {
                UpdateLevel(newLevel);
            }
        }

        public void UnlockAllLevels() {
            for (int i = 0; i < levelsUnlocked.Length; i++) {
                levelsUnlocked[i] = true;
            } 
        }

        private void GameOver()
        {
            int finalScore = _gameManager.scoreManager.currentScore;
            // If passed unlock threshold and next level isn't already unlocked
            if(finalScore >= levelUnlockThreshold && !levelsUnlocked[level]) {
                levelsUnlocked[level] = true;
                _gameManager.canvasManager.gameOverCanvas.GetComponent<GameOverCanvas>().nextLevelUnlockedText.enabled = true;
            }
        }
    
    }
}
