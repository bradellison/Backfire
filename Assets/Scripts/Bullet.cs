using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float moveSpeed;

    [Header("Script Managed Variables")]
    public Vector3 movementVector;
    public Vector3 startingMovementDirection ;
    public Vector2 camWorldSize;

    private GameObject _player;
    private Collider2D _playerCollider;
    private Collider2D _collider;
    private Transform _transform;
    
    private Vector3 _previousLocation;
    private Vector2 _spriteSize;

    private TrailRenderer _trailRenderer;
    private bool _hitEdge;

    private bool _collisionsEnabled;

    public float elapsedTime;
    public float frequency;
    public float angularFrequency;
    public float period;
    public float amplitude;
    public float phase;

    [SerializeField] private OnBulletHitScriptableObject onBulletHitScriptableObject;

    private void Awake()
    {
        _transform = transform;
        _collider = this.gameObject.GetComponent<Collider2D>();
    }
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(_playerCollider, _collider);
        _spriteSize = this.gameObject.GetComponent<SpriteRenderer>().bounds.size;

        //Enable collisions after 0.1 seconds to avoid instant death
        Invoke(nameof(EnableCollisions), 0.5f);
        
        int enemyLayer = LayerMask.GetMask("Enemies");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);

        _trailRenderer = this.gameObject.transform.GetChild(0).GetComponent<TrailRenderer>();
        _hitEdge = false;
        _collisionsEnabled = false;

        period = 5f;    
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

    // Update is called once per frame
    private void Update()
    {
        MoveBullet();
        HandleWave();
        CheckScreenEdgesWithSingle();
        CheckCollisionBetweenFrames();
    }

    private void HandleWave() {
        if (Math.Abs(1 / (period) - frequency) > 0.001f) {
            // Recalculate frequency & omega.
            frequency = 1 / (period);
            angularFrequency = (2 * Mathf.PI) * frequency;
        }
        // Update elapsed time.
        elapsedTime += Time.deltaTime;
        // Calculate new omega-time product.
        float omegaProduct = (angularFrequency * elapsedTime);
        // Plug in all calculated variables into the complete Sine wave equation.
        float waveHeight = (amplitude * Mathf.Sin (omegaProduct + phase));
        
        if(startingMovementDirection == Vector3.right || startingMovementDirection == Vector3.left) {
            movementVector = new Vector3(movementVector.x, waveHeight, 0f);
        } else {
            movementVector = new Vector3(waveHeight, movementVector.y, 0f);
        }

        elapsedTime += Time.deltaTime;
    }

    private void MoveBullet() {
        _previousLocation = _transform.position;
        transform.Translate(movementVector * (moveSpeed * Time.deltaTime));
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
        _hitEdge = true;
        if(!_collisionsEnabled) { EnableCollisions(); }
        _transform.position = newSpawnVector;
        _trailRenderer.Clear();
    }
}
