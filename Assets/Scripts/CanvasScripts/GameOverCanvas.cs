using ManagerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class GameOverCanvas : MonoBehaviour
    {
        public Button endGameButton;
        public TextMeshProUGUI nextLevelUnlockedText;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();     
            endGameButton.onClick.AddListener(EndGame);
        }

        private void EndGame() {
            _gameManager.EndGame();
        }

    }
}
