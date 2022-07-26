using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextTitle : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text levelSelectText;

    ScoreManager scoreManager;
    LevelManager levelManager;
    // Update is called once per frame

    void Start() {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }


    void Update()
    {
        scoreText.text = ($"Score: {scoreManager.currentScore.ToString()}");
        highScoreText.text = ($"High Score: {scoreManager.highScores[levelManager.level - 1].ToString()}");
        levelSelectText.text = ($"Level: {levelManager.level.ToString()}");
    }
}
