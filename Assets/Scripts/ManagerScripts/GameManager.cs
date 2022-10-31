using System;
using ScriptableObjects;
using UnityEngine;

namespace ManagerScripts
{
    public class GameManager : MonoBehaviour
    {

        public Vector2 camWorldSize;
        public bool isGameRunning;
        public bool isGameOver;

        public CanvasManager canvasManager;
        public LevelManager levelManager;
        public MusicManager musicManager;
        public ScoreManager scoreManager;
        public SettingsManager settingsManager;
        public SfxManager sfxManager;
        public SpawnManager spawnManager;
        public TouchInput touchInput;
    
        public OuterSpace outerSpace;
        public PlayerController playerController;

        [Header("Scriptable Objects")]
        [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;
        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
        
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
            Camera mainCamera = Camera.main;
            if (!mainCamera) {Debug.Log("No camera found", this); return;}
            camWorldSize.y = mainCamera.orthographicSize;
            camWorldSize.x = camWorldSize.y * mainCamera.aspect;
        }
        private void Start()
        {
            isGameRunning = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if(!isGameRunning) {
                if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    TryToStartGame();
                } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                    levelManager.DecreaseLevel();
                } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                    levelManager.IncreaseLevel();
                }
            } else {
                if(isGameOver) {
                    if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                        EndGame();
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        public void TryToStartGame() {
            if(levelManager.levelsUnlocked[levelManager.level - 1]) {
                isGameRunning = true;
                onGameStartScriptableObject.onGameStartEvent.Invoke();
                //spawnManager.DestroyAll();
                //playerController = spawnManager.SpawnPlayer();
                //spawnManager.SpawnWorld();
                //scoreManager.GameStart();
                //canvasManager.GameStartToggle();
                //touchInput.GameStart(playerController);
            }
        }

        public void ToggleSettingsMenu() {
            if(settingsManager.isMenuOpen) {
                canvasManager.CloseSettingsMenu();
            } else {
                canvasManager.OpenSettingsMenu();
            }
            settingsManager.isMenuOpen = !settingsManager.isMenuOpen;
        }

        private void GameOver() {
            //isGameRunning = false;
            isGameOver = true;
            //canvasManager.GameOverToggle();
            //scoreManager.GameEnd();
            //levelManager.GameOver();
        }

        public void EndGame() {
            isGameRunning = false;
            isGameOver = false;
            canvasManager.GameEndToggle();
        }
    }
}
