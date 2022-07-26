using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Vector3 objectSize; 

    [Range(1,512)]
    public int resolution;

    public float noiseScale;

    public int seed;

    [Range(1,8)]
    public int octaves;

    [Range(0, 1)]
    public float persistance;

    [Range(1,4)]
    public float lacunarity;

    public Vector2 offset;
    public Vector2 offsetAutomaticIncrement;

    public bool autoUpdate;

    public Gradient coloring;

    MapDisplay display;


    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(resolution, seed, noiseScale, octaves, persistance, lacunarity, offset);

        if (display == null) {
            display = this.gameObject.GetComponent<MapDisplay>();
        }

        display.DrawNoiseMap(noiseMap, objectSize, coloring);
    }

    void Start() {
        GenerateMap();
    }

    void Update() {
        offset.x += offsetAutomaticIncrement.x;
        offset.y += offsetAutomaticIncrement.y;
        GenerateMap();
    }

}
