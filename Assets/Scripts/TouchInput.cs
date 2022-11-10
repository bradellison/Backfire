using System;
using ScriptableObjects;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private PlayerController _playerController;

    public GameObject particle;
    public float swipeDistanceThreshold;
    public Vector3 currentSwipe;
    public Vector3 previousSwipe;

    [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;

    private void OnEnable()
    {
        onGameStartScriptableObject.onGameStartEvent.AddListener(GameStart);
    }
    
    private void OnDisable()
    {
        onGameStartScriptableObject.onGameStartEvent.RemoveListener(GameStart);
    }
    
    private void CatchSwipe(Touch touch)
    {
        Vector2 delta = touch.deltaPosition;
        if (delta == Vector2.zero || !_playerController) { return; }

        //Log previous successful swipe for comparison later
        //previousSwipe = currentSwipe;
        if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
            if(delta.x > swipeDistanceThreshold) {
                currentSwipe = Vector3.right;
            } else if(delta.x < -swipeDistanceThreshold) {
                currentSwipe = Vector3.left;
            }
        } else if(Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
            if (delta.y > swipeDistanceThreshold) {
                currentSwipe = Vector3.up;
            } else if(delta.y < -swipeDistanceThreshold) {
                currentSwipe = Vector3.down;            
            } 
        } 
        
        if(currentSwipe != previousSwipe) {
            _playerController.swipeVector = currentSwipe;
            previousSwipe = currentSwipe;
        }
    }

    private void ParticleOnTouch(Touch touch) {
        if (touch.phase == TouchPhase.Began) {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (Physics.Raycast(ray))
            {
                // Create a particle if hit
                Instantiate(particle, worldPosition, transform.rotation);
            }
        }
    }

    private void GameStart()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentSwipe = Vector3.zero;
        previousSwipe = Vector3.zero;
    }
    private void Update()
    {
        if (Input.touchCount <= 0) return;
        //Only handle the first touch, will ignore multiple touches
        Touch touch = Input.GetTouch(0);
        //ParticleOnTouch(touch);
        CatchSwipe(touch);
    }
}