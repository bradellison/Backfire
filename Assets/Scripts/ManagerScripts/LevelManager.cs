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
        [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;

        private void OnEnable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.AddListener(GameOver);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
        }

        private void OnDisable()
        {
            onPlayerDeadScriptableObject.onPlayerDeadEvent.RemoveListener(GameOver);
            onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
        }
        private void Awake()
        {
            _minLevel = 1;
            _maxLevel = 5;
            level = 1;
            
            levelsUnlocked = new []{true, false, false, false, false};
            LoadPrefs();

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
                SavePrefs(5);
            } 
        }

        private void GameOver()
        {
            int finalScore = _gameManager.scoreManager.currentScore;
            // If passed unlock threshold and next level isn't already unlocked
            if(finalScore >= levelUnlockThreshold && !levelsUnlocked[level]) {
                levelsUnlocked[level] = true;
                SavePrefs(level + 1);
                _gameManager.canvasManager.gameOverCanvas.GetComponent<GameOverCanvas>().nextLevelUnlockedText.enabled = true;
            }
        }

        private void SavePrefs(int newLevelUnlocked)
        {
            PlayerPrefs.SetInt("MaxLevelUnlocked", newLevelUnlocked);
            PlayerPrefs.Save();
        }

        private void LoadPrefs()
        {
            if (!PlayerPrefs.HasKey("MaxLevelUnlocked")) { return; }
            
            int maxLevelUnlocked = PlayerPrefs.GetInt("MaxLevelUnlocked");
            for (int i = 0; i < maxLevelUnlocked; i++) {
                levelsUnlocked[i] = true;
            } 
        }
        
        private void ResetPrefs()
        {
            PlayerPrefs.SetInt("MaxLevelUnlocked", 1);
            levelsUnlocked = new []{true, false, false, false, false};
        }
    }
}
