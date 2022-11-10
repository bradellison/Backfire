using System;
using System.Collections;
using ManagerScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Video;

public class OuterSpace : MonoBehaviour
{

    private MapGenerator _mapGenerator;
    private GameManager _gameManager;
    
    private VideoPlayer _videoPlayer;
    private Vector2 _camWorldSize;

    public bool isUsingVideoPlayer;

    [Header("Video Player Variables")]
    public VideoClip[] levelVideos;
    public RenderTexture textureForRender;
    public Material textureForRenderMat;

    [Header("Automatic Generation Values")]
    public int resolution;
    public float[] noiseScales;
    public int[] octaves;
    public float[] persistance;
    public float[] lacunarity;
    public Vector2[] maxOffsetIncrements;
    public bool[] shouldUpdateOffsetBools;
    public Gradient[] spaceColorings;

    [Header("Scriptable Objects")]
    [SerializeField] private BackgroundResolutionManagerScriptableObject backgroundResolutionManagerScriptableObject;
    [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;

    private void OnEnable()
    {
        backgroundResolutionManagerScriptableObject.backgroundResolutionChangeEvent.AddListener(ResolutionUpdated);
        onPreferencesResetScriptableObject.onPreferencesResetEvent.AddListener(ResetPrefs);
    }

    private void OnDisable()
    {
        backgroundResolutionManagerScriptableObject.backgroundResolutionChangeEvent.RemoveListener(ResolutionUpdated);
        onPreferencesResetScriptableObject.onPreferencesResetEvent.RemoveListener(ResetPrefs);
    }

    private IEnumerator ChangeVideo(VideoClip newClip)
    {
        double previousTime = _videoPlayer.time;
        _videoPlayer.Pause();
        _videoPlayer.clip = newClip;
        _videoPlayer.Prepare();
        while (!_videoPlayer.isPrepared) 
            yield return new WaitForEndOfFrame(); 

        _videoPlayer.time = previousTime; //Start new clip at same time as previous clip, so it flows smoothly
        _videoPlayer.Play();
    }
    
    private void Awake()
    {
        _videoPlayer = this.gameObject.GetComponent<VideoPlayer>();
        _mapGenerator = this.gameObject.GetComponent<MapGenerator>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        _camWorldSize = Utilities.GetCamWorldSize();
        if (isUsingVideoPlayer)
        {
            _mapGenerator.enabled = false;
            _videoPlayer.enabled = true;
            //textureForRender.width = Mathf.CeilToInt(_camWorldSize.x);
            //Debug.Log(_camWorldSize.x + " " + transform.localScale.x);
            //Not sure why size.y needs to be divided differently to x
            //transform.localScale = new Vector3(_camWorldSize.x / 5f, _camWorldSize.y / 4.8f, transform.localScale.z);
            textureForRenderMat.mainTexture = textureForRender;
            
            this.GetComponent<MeshRenderer>().material = textureForRenderMat;
        }
        else
        {
            _mapGenerator.enabled = true;
            _videoPlayer.enabled = false;
        }
    }

    private void Start()
    {
        //noiseScales = new float[0.1f, 30.0f, 1.0f, 1.0f, 1.0f];
        //octaves = new int[2, 4, 1, 1, 1];
        //persistance = new float[0.5f, 0.48f, 1.0f, 1.0f, 1.0f];
        //lacunarity = new float[1.75f, 1.9f, 1.0f, 1.0f, 1.0f];
        //offsetIncrements = new Vector2[new Vector2[0.01f, 0f], new Vector2[0.03f, 0.01f], new Vector2[0f, 0f], new Vector2[0f, 0f], new Vector2[0f, 0f]];
        LoadPrefs();

        _mapGenerator.objectSize.x = _camWorldSize.x / 5;
        _mapGenerator.objectSize.z = _camWorldSize.y / 5;

        ChangeBackground(1);
    }

    private void ResolutionUpdated(int newResolution)
    {
        resolution = newResolution;
        SavePrefs();
        float resDiff = resolution / 256f;

        _mapGenerator.resolution = resolution;
        _mapGenerator.noiseScale = noiseScales[_gameManager.levelManager.level - 1] * resDiff;
        _mapGenerator.maxOffsetAutomaticIncrement = maxOffsetIncrements[_gameManager.levelManager.level - 1] * resDiff;

        _mapGenerator.UpdateBackground();
    }

    public void ChangeBackground(int level) {
        if (isUsingVideoPlayer)
        {
            StartCoroutine(ChangeVideo(levelVideos[level - 1]));
            return;
        }
        
        if(level> noiseScales.Length) {
            level = 2;
        }
        
        //Values were configured at resolution of 256, some values must be altered to handle other resolutions
        float resDiff = resolution / 256f;

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
    private void SavePrefs()
    {
        PlayerPrefs.SetInt("BackgroundResolution", resolution);
        PlayerPrefs.Save();
    }

    private void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("BackgroundResolution"))
        {
            resolution = PlayerPrefs.GetInt("BackgroundResolution");
        }
    }
    
    private void ResetPrefs()
    {
        PlayerPrefs.SetInt("BackgroundResolution", 256);
        resolution = 256;
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