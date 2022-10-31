using CanvasScripts;
using ManagerScripts;
using ScriptableObjects;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")] 
    private Transform _transform;
    public float moveSpeed;
    public Vector3 movementVector;
    public Vector3 swipeVector;    
    public float speedBoostMultiplier;
    private bool _isSpeedBoostActive;

    private Vector2 _camWorldSize;

    private GameManager _gameManager;

    [Header("Sprite")]
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteSize;
    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite downSprite;

    [Header("Forcefield")]
    public GameObject forcefieldPrefab;
    private GameObject _forcefield;
    
    public bool isForcefieldActive;
    public float forcefieldCounter;
    public float forcefieldTarget;
    public float forcefieldFalloff;
    public float forcefieldActiveFalloffMultiplier;

    public float forcefieldBulletHitCounterDecrease;

    [SerializeField] private OnBulletHitScriptableObject onBulletHitScriptableObject;
    [SerializeField] private OnLaserHitScriptableObject onLaserHitScriptableObject;
    [SerializeField] private OnPlayerDeadScriptableObject onPlayerDeadScriptableObject;
    

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _spriteSize = _spriteRenderer.bounds.size;
    }

    private void OnEnable()
    {
        onBulletHitScriptableObject.onBulletHitEvent.AddListener(HitBullet);
        onLaserHitScriptableObject.onLaserHitEvent.AddListener(HitBullet);
    }

    private void OnDisable()
    {
        onBulletHitScriptableObject.onBulletHitEvent.RemoveListener(HitBullet);
        onLaserHitScriptableObject.onLaserHitEvent.RemoveListener(HitBullet);
    }
    
    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        Camera mainCamera = Camera.main;
        if (!mainCamera)
        {
            return;
        }
        _camWorldSize.y = mainCamera.orthographicSize; 
        _camWorldSize.x = _camWorldSize.y * mainCamera.aspect;
    }
    
    private void CheckScreenEdges() {
        float halfX = _spriteSize.x / 2;
        float halfY = _spriteSize.y / 2;
        
        //checks if the edge of the player has reached the border, and moves it to the other side of the screen
        Vector3 position = _transform.position;
        if(position.x - halfX > _camWorldSize.x && movementVector == Vector3.right) {
            Vector3 newSpawnVector = new Vector3(-_camWorldSize.x - halfX, position.y, position.z);
            _transform.position = newSpawnVector;
        } else if(position.x + halfX < -_camWorldSize.x && movementVector == Vector3.left) {
            Vector3 newSpawnVector = new Vector3(_camWorldSize.x + halfX, position.y, position.z);
            _transform.position = newSpawnVector;
        } else if (position.y - halfY > _camWorldSize.y && movementVector == Vector3.up) {
            Vector3 newSpawnVector = new Vector3(position.x, -_camWorldSize.y - halfY, position.z);
            _transform.position = newSpawnVector;
        } else if (position.y + halfY < -_camWorldSize.y && movementVector == Vector3.down) {
            Vector3 newSpawnVector = new Vector3(position.x, _camWorldSize.y + halfY, position.z);
            _transform.position = newSpawnVector;
        }
    }

    private void MovePlayer() {
        if(Input.GetKeyDown(KeyCode.W) || swipeVector == Vector3.up) {
            _spriteRenderer.sprite = upSprite;
            movementVector = Vector3.up;
        } else if(Input.GetKeyDown(KeyCode.A) || swipeVector == Vector3.left) {
            _spriteRenderer.sprite = leftSprite;
            movementVector = Vector3.left;
        } else if(Input.GetKeyDown(KeyCode.S) || swipeVector == Vector3.down) {
            _spriteRenderer.sprite = downSprite;
            movementVector = Vector3.down;
        } else if(Input.GetKeyDown(KeyCode.D) || swipeVector == Vector3.right) {
            _spriteRenderer.sprite = rightSprite;
            movementVector = Vector3.right;
        } 
        
        if(Input.GetKeyDown(KeyCode.Space) && !_isSpeedBoostActive) {
            _isSpeedBoostActive = true;
        } else if(Input.GetKeyUp(KeyCode.Space) && _isSpeedBoostActive) {
            _isSpeedBoostActive = false;
        }

        float boostMultiplier = 1f; 
        if(_isSpeedBoostActive) {
            boostMultiplier = speedBoostMultiplier;
        }
        transform.Translate(movementVector * (moveSpeed * boostMultiplier * Time.deltaTime));
    }

    private void IncrementForceFieldCounter()
    {
        if (isForcefieldActive) return;
        
        forcefieldCounter += 1;
        if (forcefieldCounter > forcefieldTarget)
        {
            forcefieldCounter = forcefieldTarget;
        }

        if (forcefieldTarget - forcefieldCounter > 0.001f) return;
        isForcefieldActive = true;
        CreateForceField();
    }

    private void CreateForceField() {
        _forcefield = Instantiate(forcefieldPrefab, transform, true);
        _forcefield.transform.position = _transform.position;
        _forcefield.GetComponent<Forcefield>().forcefieldBulletHitCounterDecrease = forcefieldBulletHitCounterDecrease;
        _gameManager.canvasManager.gameplayCanvas.GetComponent<GameplayCanvas>().forcefieldBar.UpdateColor();
    }

    public void DecreaseForceFieldCounter(float amount) {
        //Different amount to allow for usual falloff, and also accelerated drop when hitting a bullet
        if(isForcefieldActive) {
            amount *= forcefieldActiveFalloffMultiplier;
        }
        
        //Don't allow forcefield counter to go below 0
        forcefieldCounter -= amount;
        if (forcefieldCounter > 0) return;
        forcefieldCounter = 0;
        
        //If it does hit 0, destroy forcefield if active
        if (!isForcefieldActive) return;
        isForcefieldActive = false;
        Destroy(_forcefield);
        _gameManager.canvasManager.gameplayCanvas.GetComponent<GameplayCanvas>().forcefieldBar.UpdateColor();
    }

    private void Update()
    {
        CheckScreenEdges();
        MovePlayer();
    }

    private void FixedUpdate() {
        ForcefieldUpdates();
    }

    private void ForcefieldUpdates() {
        DecreaseForceFieldCounter(forcefieldFalloff);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("World")) {
            HitWorld(other.gameObject, false);
        } else if (other.gameObject.CompareTag("Bullet")) {
            HitBullet();
        }
    }

    private void HitBullet()
    {
        if (isForcefieldActive) return;
        onPlayerDeadScriptableObject.OnPlayerDead();
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void HitWorld(GameObject world, bool hitByForcefield) {
        _gameManager.scoreManager.HitWorld();
        _gameManager.spawnManager.SpawnBullet(transform.position, movementVector);
        
        //Below is to stop multiple worlds spawning if forcefield and player collide with world
        if(!isForcefieldActive || (isForcefieldActive && hitByForcefield)) {
            _gameManager.spawnManager.SpawnWorld();
        }
        
        if(!isForcefieldActive) {
            IncrementForceFieldCounter();
        }
        
        _gameManager.sfxManager.HitWorld();
        Destroy(world);
    }

    private void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, movementVector);
    }

}
