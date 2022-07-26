using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public int currentScore;
    public int[] lastScores = new int[5];
    public int highScore;
    public int[] highScores = new int[5];

    LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart() {
        currentScore = 0;
    }

    public void GameEnd() {
        if(currentScore > highScores[levelManager.level - 1]) {
            highScores[levelManager.level - 1] = currentScore;
        }
        lastScores[levelManager.level - 1] = currentScore;
    }

    public void HitWorld() {
        currentScore += 10;
    }
}
