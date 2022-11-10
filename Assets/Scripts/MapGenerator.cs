using System.Collections;
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
    public float updateNoiseFreqInSeconds;
    private float _elapsedTimeNoiseUpdate;
    public Vector2 maxOffsetAutomaticIncrement;
    public Vector2 targetOffsetAutomaticIncrement;
    
    public bool shouldUpdateOffset;
    private float _elapsedTimeTargetOffset;
    public float updateTargetOffsetFreqInSeconds = 0.5f;
    private bool _isUpdatingOffset;

    public bool autoUpdate;

    public Gradient coloring;

    private MapDisplay _display;

    private Texture2D _texture;
    private Color[] _colourMap;

    private float[,] _noiseMap;

    public void GenerateMap() {
        _noiseMap = Noise.GenerateNoiseMap(resolution, seed, noiseScale, octaves, persistance, lacunarity, offset);
        
        if (!_display) {
            _display = this.gameObject.GetComponent<MapDisplay>();
        }

        _display.DrawNoiseMap(_texture, _colourMap, _noiseMap, objectSize, coloring);
    }

    private void CreateTextureAndColourMap() {
        //Debug.Log("Draw texture2D and colour map with width " + resolution + " and height " + resolution); 

        int width = resolution;
        int height = resolution;

        _texture = new Texture2D(width, height);
        _colourMap = new Color[width * height];
    }

    private void Start() {
        CreateTextureAndColourMap();
        GenerateMap();
    }
    
    private void UpdateIncrements() {
        //For random targets within max increments
        //targetOffsetAutomaticIncrement = new Vector2(Random.Range(-maxOffsetAutomaticIncrement.x, maxOffsetAutomaticIncrement.x), Random.Range(-maxOffsetAutomaticIncrement.y, maxOffsetAutomaticIncrement.y));
        
        //For basic one way then the other
        if (Mathf.Abs(targetOffsetAutomaticIncrement.x) != Mathf.Abs(maxOffsetAutomaticIncrement.x))
        {
            targetOffsetAutomaticIncrement = maxOffsetAutomaticIncrement;
        }
        else
        {
            targetOffsetAutomaticIncrement = -1 * targetOffsetAutomaticIncrement;
        }

        //offsetAutomaticIncrement = targetOffsetAutomaticIncrement;
        StartCoroutine(LerpOffsetVector());
    }

    public void UpdateBackground() {
        CreateTextureAndColourMap();
        if(!shouldUpdateOffset) {
            offsetAutomaticIncrement = maxOffsetAutomaticIncrement;
        } 

    }

    private IEnumerator LerpOffsetVector() {
        _isUpdatingOffset = true;
        float elapsedFrameCount = 0;
        float lerpFrameDuration = 300;
        //float timeElapsedLerp = 0f;
        //float lerpDuration = 20f;
        Vector2 startOffsetIncrement = offsetAutomaticIncrement;
        //while(timeElapsedLerp < lerpDuration && shouldUpdateOffset) {
        while(elapsedFrameCount < lerpFrameDuration && shouldUpdateOffset) {
            //offsetAutomaticIncrement = Vector2.Lerp(startOffsetIncrement, targetOffsetAutomaticIncrement, timeElapsedLerp / lerpDuration);
            offsetAutomaticIncrement = Vector2.Lerp(startOffsetIncrement, targetOffsetAutomaticIncrement, elapsedFrameCount / lerpFrameDuration);
            elapsedFrameCount += 1;
            //timeElapsedLerp += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        _isUpdatingOffset = false;
    }

    private void Update()
    {
        _elapsedTimeTargetOffset += Time.deltaTime;
        if (shouldUpdateOffset && !_isUpdatingOffset && _elapsedTimeTargetOffset > updateTargetOffsetFreqInSeconds)
        {
            UpdateIncrements();
            _elapsedTimeTargetOffset = 0;
        }

        _elapsedTimeNoiseUpdate += Time.deltaTime;
        if (_elapsedTimeNoiseUpdate > updateNoiseFreqInSeconds)
        {
            offset.x += offsetAutomaticIncrement.x;
            offset.y += offsetAutomaticIncrement.y;
            GenerateMap();
            _elapsedTimeNoiseUpdate = 0;
        }
    }
}
