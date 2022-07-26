using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public GameObject titleCanvasPrefab;
    public GameObject gameplayCanvasPrefab;
    
    GameObject titleCanvas;
    GameObject gameplayCanvas;

    GameManager gameManager;

    void Start()
    {
        gameManager = this.transform.parent.GetComponent<GameManager>();
        titleCanvas = Instantiate(titleCanvasPrefab);
    }

    public void GameStartToggle() {
        Destroy(titleCanvas);
        gameplayCanvas = Instantiate(gameplayCanvasPrefab);
    } 

    public void GameEndToggle() {
        Destroy(gameplayCanvas);
        titleCanvas = Instantiate(titleCanvasPrefab);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
