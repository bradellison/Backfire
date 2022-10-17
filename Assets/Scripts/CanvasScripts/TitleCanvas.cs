using ManagerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class TitleCanvas : MonoBehaviour
    {
        public Button levelIncreaseButton;
        public Button levelDecreaseButton;
        public Button startGameButton;
        public Button settingsMenuButton;

        public TextMeshProUGUI levelUnlockText;
        public TextMeshProUGUI touchToStartText;

        public TMP_Text scoreText;
        public TMP_Text highScoreText;
        public TMP_Text levelSelectText;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); 

            levelIncreaseButton.onClick.AddListener(IncreaseLevel);
            levelDecreaseButton.onClick.AddListener(DecreaseLevel);
            startGameButton.onClick.AddListener(StartGame);
            settingsMenuButton.onClick.AddListener(ToggleSettingsMenu);

            UpdateTextFields();
            CheckLevelUnlockText();
        }

        public void UpdateTextFields() {
            scoreText.text = ($"Score: {_gameManager.scoreManager.currentScore.ToString()}");
            highScoreText.text = ($"High Score: {_gameManager.scoreManager.highScores[_gameManager.levelManager.level - 1].ToString()}");
            levelSelectText.text = ($"Level: {_gameManager.levelManager.level.ToString()}");
        }


        public void CheckLevelUnlockText() {
            if(_gameManager.levelManager.levelsUnlocked[_gameManager.levelManager.level - 1]) {
                touchToStartText.enabled = true;
                levelUnlockText.enabled = false;
            } else {
                touchToStartText.enabled = false;
                levelUnlockText.enabled = true;
            }
        }

        private void ToggleSettingsMenu() {
            _gameManager.ToggleSettingsMenu();
        }


        private void IncreaseLevel() {
            _gameManager.levelManager.IncreaseLevel();
            CheckLevelUnlockText();
        }

        private void DecreaseLevel() {
            _gameManager.levelManager.DecreaseLevel();
            CheckLevelUnlockText();
        }

        private void StartGame() {
            _gameManager.TryToStartGame();
        }

    }
}
