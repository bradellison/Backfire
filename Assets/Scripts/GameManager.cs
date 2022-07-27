using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public bool isGameRunning;
    SpawnManager spawnManager;
    ScoreManager scoreManager;
    CanvasManager canvasManager;
    LevelManager levelManager;
    OuterSpace outerSpace;
    //AudioManager audioManager;

    void Start()
    {   
        isGameRunning = false;
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        scoreManager =  GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        canvasManager =  GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        outerSpace = GameObject.FindGameObjectWithTag("OuterSpace").GetComponent<OuterSpace>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameRunning) {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                StartGame();
                isGameRunning = true;
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                if(levelManager.DecreaseLevel()) {
                    outerSpace.ChangeBackground(levelManager.level);
                }
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                if(levelManager.IncreaseLevel()) {
                    outerSpace.ChangeBackground(levelManager.level);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void StartGame() {
        spawnManager.DestroyAll();
        spawnManager.SpawnPlayer();
        spawnManager.SpawnWorld();
        scoreManager.GameStart();
        canvasManager.GameStartToggle();
    }

    public void EndGame() {
        isGameRunning = false;
        canvasManager.GameEndToggle();
        scoreManager.GameEnd();
    }
}
