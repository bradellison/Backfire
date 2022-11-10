using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ManagerScripts;
using ScriptableObjects;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public float moveSpeed;

    [Header("Script Managed Variables")]
    public Vector3 movementVector;
    public Vector2 camWorldSize;
    private Vector3[] _screenCornerPoints;
    private Vector3 _targetMoveVector;
    public bool shouldBounceBetweenCorners;
    
    private Collider2D _collider;
    private Transform _transform;
    
    private Vector2 _spriteSize;
    
    private bool _collisionsEnabled;

    private LineRenderer _lineRenderer;
    private float _laserMaxDistance;
    
    [SerializeField] private OnLaserHitScriptableObject onLaserHitScriptableObject;
    
    private void Awake()
    {
        _transform = transform;
        _collider = this.gameObject.GetComponent<Collider2D>();
        _lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        _laserMaxDistance = 0;
    }

    private void Start()
    {
        _spriteSize = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;
        
        int enemyLayer = LayerMask.GetMask("Enemies");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
    }

    public void SetStartLocation(int cornerSpawnIndex)
    {
        camWorldSize = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().camWorldSize;
        GenerateScreenCornerPoints();
        _transform.position = _screenCornerPoints[cornerSpawnIndex];
        int targetSpawnIndex = (System.Array.IndexOf(_screenCornerPoints, _transform.position) + 1) % 4;
        _targetMoveVector = _screenCornerPoints[targetSpawnIndex];
    }

    private void GenerateScreenCornerPoints()
    {
        _screenCornerPoints = new Vector3[4];
        float maxX = camWorldSize.x * 0.8f;
        float maxY = camWorldSize.y * 0.8f;
        _screenCornerPoints[0] = (new Vector3(-maxX, -maxY, 0));
        _screenCornerPoints[1] = (new Vector3(-maxX, maxY, 0));
        _screenCornerPoints[2] = (new Vector3(maxX, maxY, 0));
        _screenCornerPoints[3] = (new Vector3(maxX, -maxY, 0));
    }
    
    private void FixedUpdate()
    {
        _laserMaxDistance += 0.1f;
    }

    private void UpdateLaser()
    {
        Vector3 laserTargetVector = Vector3.zero;
        Vector3 direction = laserTargetVector - _lineRenderer.GetPosition(0).normalized ;
        Vector3 position = _transform.position;
        
        Debug.DrawRay(position, direction * _laserMaxDistance);
        laserTargetVector = position + direction * _laserMaxDistance;

        int enemyLayer = LayerMask.GetMask("Enemies");
        RaycastHit2D hit = Physics2D.Raycast(position, direction, _laserMaxDistance, ~enemyLayer);
        _lineRenderer.SetPosition(0, position);
        _lineRenderer.SetPosition(1, laserTargetVector);
        
        if (hit.collider)
        {
            _lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                onLaserHitScriptableObject.onLaserHitEvent.Invoke();
            }
        }
    }
    
    private void Update()
    {
        Move();
        CheckScreenEdgesWithSingle();
        UpdateLaser();
    }

    private void Move() {
        if (shouldBounceBetweenCorners)
        {
            MoveBetweenCorners();
        }
        else
        {
            transform.Translate(movementVector * (moveSpeed * Time.deltaTime));
        }
    }

    private void MoveBetweenCorners()
    {
        Vector3 direction = (_targetMoveVector - _transform.position).normalized;
        transform.Translate(direction * (moveSpeed * Time.deltaTime));

        if (Vector3.Distance(_transform.position, _targetMoveVector) < 0.1f)
        {
            int newCornerIndex = (System.Array.IndexOf(_screenCornerPoints, _targetMoveVector) + 1) % 4;
            _targetMoveVector = _screenCornerPoints[newCornerIndex];
        }
    }
    
    private void CheckScreenEdgesWithSingle() {
        float halfX = _spriteSize.x / 2;
        float halfY = _spriteSize.y / 2;

        //checks if the edge of the bullet has reached the border, and moves it to the other side of the screen
        Vector3 position = _transform.position;
        if(position.x - halfX > camWorldSize.x && movementVector.x > 0f) {
            HitEdge(new Vector3(-camWorldSize.x - halfX, position.y, position.z));
        } else if(position.x + halfX < -camWorldSize.x && movementVector.x < 0f) {
            HitEdge(new Vector3(camWorldSize.x + halfX, position.y, position.z));
        }
        
        if (position.y - halfY > camWorldSize.y && movementVector.y > 0f) {
            HitEdge(new Vector3(position.x, -camWorldSize.y - halfY, position.z));
        } else if (position.y + halfY < -camWorldSize.y && movementVector.y < 0f) {
            HitEdge(new Vector3(position.x, camWorldSize.y + halfY, position.z));
        }
        
    } 

    private void HitEdge(Vector3 newSpawnVector)
    {
        _transform.position = newSpawnVector;
    }
}
