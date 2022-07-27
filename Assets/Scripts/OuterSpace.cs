using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterSpace : MonoBehaviour
{
    
    PlayerController player;
    MapGenerator mapGenerator;
    GameManager gameManager;

    Vector2 camWorldSize = new Vector2();

    public float[] noiseScales;
    public int[] octaves;
    public float[] persistance;
    public float[] lacunarity;
    public Vector2[] offsetIncrements;
    public Gradient[] spaceColorings;


    void Start()
    {
        //noiseScales = new float[0.1f, 30.0f, 1.0f, 1.0f, 1.0f];
        //octaves = new int[2, 4, 1, 1, 1];
        //persistance = new float[0.5f, 0.48f, 1.0f, 1.0f, 1.0f];
        //lacunarity = new float[1.75f, 1.9f, 1.0f, 1.0f, 1.0f];
        //offsetIncrements = new Vector2[new Vector2[0.01f, 0f], new Vector2[0.03f, 0.01f], new Vector2[0f, 0f], new Vector2[0f, 0f], new Vector2[0f, 0f]];

        camWorldSize.y = Camera.main.orthographicSize;
        camWorldSize.x = Camera.main.orthographicSize * Camera.main.aspect;

        mapGenerator = this.gameObject.GetComponent<MapGenerator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        mapGenerator.objectSize.x = camWorldSize.x / 5;
        mapGenerator.objectSize.z = camWorldSize.y / 5;

        ChangeBackground(1);
    }

    public void ChangeBackground(int level) {
        mapGenerator.noiseScale = noiseScales[level - 1];
        mapGenerator.octaves = octaves[level - 1];
        mapGenerator.persistance = persistance[level - 1];
        mapGenerator.lacunarity = lacunarity[level - 1];
        mapGenerator.offsetAutomaticIncrement = offsetIncrements[level - 1];
        mapGenerator.coloring = spaceColorings[level - 1];
    }

    // Update is called once per frame
    void Update()
    {

    }
}


//Level 1
//Noise Scale - 0.1
//Octaves - 2
//Persistance - 0.5
//Lacunarity - 1.75
//Offset Automatic Inc - x=0.01 y=0
//Gradient - mostly dark blue/black with small amount of white at far end


//Level 4?
//Noise Scale - 30
//Octaves - 4
//Persistance - 0.48
//Lacunarity - 1.9
//Offset Automatic Inc - x=0.03 y=0.01
//Gradient - mostly dark blue/black with small amount of white at far end