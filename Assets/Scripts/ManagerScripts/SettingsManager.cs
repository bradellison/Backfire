using UnityEngine;

namespace ManagerScripts
{
    public class SettingsManager : MonoBehaviour
    {
        public bool isMenuOpen;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            isMenuOpen = false;
        }

    }
}
