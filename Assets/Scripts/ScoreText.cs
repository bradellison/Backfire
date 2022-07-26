using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    ScoreManager scoreManager;
    // Update is called once per frame

    void Start() {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }


    void Update()
    {
        scoreText.text = ($"Score: {scoreManager.currentScore.ToString()}");
        if(highScoreText != null) {
            highScoreText.text = ($"High Score: {scoreManager.highScore.ToString()}");
        }
    }
}
