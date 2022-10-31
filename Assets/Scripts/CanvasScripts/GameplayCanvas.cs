using ManagerScripts;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CanvasScripts
{
    public class GameplayCanvas : MonoBehaviour
    {

        public ForcefieldUIBar forcefieldBar;
        public TMP_Text scoreText;
        
        public void UpdateScore(int score)
        {
            scoreText.text = ($"Score: {score.ToString()}");
        }

    }
}
