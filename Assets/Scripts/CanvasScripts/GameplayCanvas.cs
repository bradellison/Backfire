using ManagerScripts;
using TMPro;
using UnityEngine;

namespace CanvasScripts
{
    public class GameplayCanvas : MonoBehaviour
    {

        public ForcefieldUIBar forcefieldBar;
        public TMP_Text scoreText;

        private GameManager _gameManager;

        private void Start() {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            UpdateScore();
        }

        public void UpdateScore()
        {
            scoreText.text = ($"Score: {_gameManager.scoreManager.currentScore.ToString()}");
        }

    }
}
