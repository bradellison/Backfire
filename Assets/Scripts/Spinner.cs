using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class Spinner : MonoBehaviour
{
    [Header("Spinner Variables")]
    public float moveSpeed;

    [Header("Script Managed Variables")]
    public Vector3 movementVector;
    public Vector2 camWorldSize;

    private GameObject _player;
    private Collider2D _playerCollider;
    private Collider2D _collider;
    private Transform _transform;

    public int laserCount;
    public GameObject laserPrefab;

    private Vector3 _previousLocation;

    private float _spriteAngle;
    private float _spriteSpinSpeed;
    private Transform _spriteTransform;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteSize;

    private bool _hitEdge;

    private bool _collisionsEnabled;

    [SerializeField] private OnBulletHitScriptableObject onBulletHitScriptableObject;

    private void Awake()
    {
        camWorldSize = Utilities.GetCamWorldSize();
        _transform = transform;
        _collider = this.gameObject.GetComponent<Collider2D>();

        _spriteTransform = this.gameObject.transform.GetChild(0);
        _spriteRenderer = _spriteTransform.GetComponent<SpriteRenderer>();
        _spriteSize = _spriteRenderer.bounds.size;
        _spriteSpinSpeed = Random.Range(0.4f, 0.8f);
    }
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerCollider = _player.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_playerCollider, _collider);
        }

        int enemyLayer = LayerMask.GetMask("Enemies");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);

        _hitEdge = false;
        _collisionsEnabled = false;

        for (int i = 0; i < Random.Range(1,4); i++)
        {
            SpawnLaser();
        }

        //Enable collisions after 0.1 seconds to avoid instant death
        Invoke(nameof(EnableCollisions), 0.5f);
    }

    private void SpawnLaser()
    {
        GameObject newLaserGameObject = Instantiate(laserPrefab, _transform);
    }
    
    private void EnableCollisions()
    {
        _collisionsEnabled = true;
        if (_player) { Physics2D.IgnoreCollision(_playerCollider, _collider, false); }         
    }

    private void CheckCollisionBetweenFrames()
    {
        // Return if hit edge this frame
        if (_hitEdge)
        {
            _hitEdge = false;
            return;
        }

        Vector3 position = _transform.position;
        float distance = Vector3.Distance(_previousLocation, position);

        Debug.DrawRay(position, -movementVector * distance);
        
        //Cast ray between this position and previous and check for collision with player
        RaycastHit2D hit = Physics2D.Raycast(position, -movementVector, distance);

        // If nothing was hit, return
        if (!hit.collider || !_collisionsEnabled) return;

        if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Forcefield"))
        {
            onBulletHitScriptableObject.OnBulletHit();
        }
    }

    private void Update()
    {
        Move();
        RotateSprite();
        CheckScreenEdgesWithSingle();
        CheckCollisionBetweenFrames();
    }

    private void FixedUpdate()
    {
        _spriteAngle += _spriteSpinSpeed;
    }
    
    private void RotateSprite()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, _spriteAngle);
        _spriteTransform.rotation = rotation;
    }
    
    private void Move() {
        _previousLocation = _transform.position;
        transform.Translate(movementVector * (moveSpeed * Time.deltaTime));
    }

    private void CheckScreenEdgesWithSingle() {
        float halfX = (_spriteSize.x / 2) + 1.5f;
        float halfY = (_spriteSize.y / 2) + 1.5f;

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
        _hitEdge = true;
        if(!_collisionsEnabled) { EnableCollisions(); }
        _transform.position = newSpawnVector;
    }
}
