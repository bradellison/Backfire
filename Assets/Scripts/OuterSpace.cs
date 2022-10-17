using ManagerScripts;
using ScriptableObjects;
using UnityEngine;

public class OuterSpace : MonoBehaviour
{
    [SerializeField] 
    private BackgroundResolutionManagerScriptableObject backgroundResolutionManagerScriptableObject;
    private MapGenerator _mapGenerator;
    private GameManager _gameManager;

    private Vector2 _camWorldSize;

    public float updateFreqInSeconds;

    public int resolution;
    public float[] noiseScales;
    public int[] octaves;
    public float[] persistance;
    public float[] lacunarity;
    public Vector2[] maxOffsetIncrements;
    public bool[] shouldUpdateOffsetBools;
    public Gradient[] spaceColorings;

    private void Start()
    {
        //noiseScales = new float[0.1f, 30.0f, 1.0f, 1.0f, 1.0f];
        //octaves = new int[2, 4, 1, 1, 1];
        //persistance = new float[0.5f, 0.48f, 1.0f, 1.0f, 1.0f];
        //lacunarity = new float[1.75f, 1.9f, 1.0f, 1.0f, 1.0f];
        //offsetIncrements = new Vector2[new Vector2[0.01f, 0f], new Vector2[0.03f, 0.01f], new Vector2[0f, 0f], new Vector2[0f, 0f], new Vector2[0f, 0f]];
        
        backgroundResolutionManagerScriptableObject.backgroundResolutionChangeEvent.AddListener(ResolutionUpdated);
        
        _mapGenerator = this.gameObject.GetComponent<MapGenerator>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _camWorldSize = _gameManager.camWorldSize;

        _mapGenerator.objectSize.x = _camWorldSize.x / 5;
        _mapGenerator.objectSize.z = _camWorldSize.y / 5;

        ChangeBackground(1);
    }

    private void ResolutionUpdated(int newResolution)
    {
        resolution = newResolution;
        float resDiff = resolution / 256f;

        _mapGenerator.resolution = resolution;
        _mapGenerator.noiseScale = noiseScales[_gameManager.levelManager.level - 1] * resDiff;
        _mapGenerator.maxOffsetAutomaticIncrement = maxOffsetIncrements[_gameManager.levelManager.level - 1] * resDiff;

        _mapGenerator.UpdateBackground();
    }

    public void ChangeBackground(int level) {
        if(level> noiseScales.Length) {
            level = 2;
        }
        
        //Values were configured at resolution of 256, some values must be altered to handle other resolutions
        float resDiff = resolution / 256f;

        _mapGenerator.updateFreqInSeconds = updateFreqInSeconds;
        _mapGenerator.resolution = resolution;
        _mapGenerator.noiseScale = noiseScales[level - 1] * resDiff;
        _mapGenerator.octaves = octaves[level - 1];
        _mapGenerator.persistance = persistance[level - 1];
        _mapGenerator.lacunarity = lacunarity[level - 1];
        _mapGenerator.maxOffsetAutomaticIncrement = maxOffsetIncrements[level - 1] * resDiff;
        _mapGenerator.shouldUpdateOffset = shouldUpdateOffsetBools[level - 1];
        _mapGenerator.coloring = spaceColorings[level - 1];

        _mapGenerator.UpdateBackground();
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