using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public Vector3 movementVector;

    Vector2 camWorldSize = new Vector2();
    Vector2 spriteSize = new Vector2(); 

    public GameObject playerPrefab;

    public bool canSpawnX;
    public bool canSpawnY;
    
    public int playerCount;

    GameManager gameManager;
    SpawnManager spawnManager;
    ScoreManager scoreManager;
    SFXManager sfxManager;

    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite downSprite;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        sfxManager = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
        camWorldSize.y = Camera.main.orthographicSize;
        camWorldSize.x = Camera.main.orthographicSize * Camera.main.aspect;
        spriteSize.x = this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSize.y = this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void SpawnNewPlayer(Vector3 newSpawnVector, bool xSide) {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.position = newSpawnVector;
        newPlayer.GetComponent<PlayerController>().movementVector = movementVector;
        if(xSide) {
            newPlayer.GetComponent<PlayerController>().canSpawnX = false;
            canSpawnX = false;
        } else {
            newPlayer.GetComponent<PlayerController>().canSpawnY = false;
            canSpawnY = false;
        }
    }

    void UpdatePlayerCounts() {
        // if final player, allow it to spawn regardless of screen position
        if(GameObject.FindGameObjectsWithTag("Player").Length == 1) {
            canSpawnX = true;
            canSpawnY = true;
        }
    }

    void CheckScreenEdgesWithDuplicatePlayers() {
        float fullX = spriteSize.x;
        float fullY = spriteSize.y;
        float halfX = spriteSize.x / 2;
        float halfY = spriteSize.y / 2;

        UpdatePlayerCounts();
        //checks if the edge of the player has reached the border, and spawns a new player on the other side if so
        //New spawns can't spawn new players until they have been fully onscreen, function below
        if(transform.position.x + halfX > camWorldSize.x && movementVector == Vector3.right && canSpawnX) {
            Vector3 newSpawnVector = new Vector3(-camWorldSize.x - halfX, transform.position.y, transform.position.z);
            SpawnNewPlayer(newSpawnVector, true);
        } else if(transform.position.x - halfX < -camWorldSize.x && movementVector == Vector3.left && canSpawnX) {
            Vector3 newSpawnVector = new Vector3(camWorldSize.x + halfX, transform.position.y, transform.position.z);
            SpawnNewPlayer(newSpawnVector, true);
        } else if (transform.position.y + halfY > camWorldSize.y && movementVector == Vector3.up && canSpawnY) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, -camWorldSize.y - halfY, transform.position.z);
            SpawnNewPlayer(newSpawnVector, false);
        } else if (transform.position.y - halfY < -camWorldSize.y && movementVector == Vector3.down && canSpawnY) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, camWorldSize.y + halfY, transform.position.z);
            SpawnNewPlayer(newSpawnVector, false);
        }

        //if a player is fully off-screen, destroy that player
        if(transform.position.x - halfX > camWorldSize.x && movementVector == Vector3.right) {
            Destroy(this.gameObject);
        } else if(transform.position.x + halfX < -camWorldSize.x && movementVector == Vector3.left) {
            Destroy(this.gameObject);
        } else if(transform.position.y - halfY > camWorldSize.y && movementVector == Vector3.up) {
            Destroy(this.gameObject);
        } else if(transform.position.y + halfY < -camWorldSize.y && movementVector == Vector3.down) {
            Destroy(this.gameObject);
        }

        //New spawns can't spawn new players until they have been fully onscreen
        if(!canSpawnX) {
            if(transform.position.x + halfX < camWorldSize.x && transform.position.x - halfX > -camWorldSize.x) {
                canSpawnX = true;
            }
        }
        if(!canSpawnY) {
            if(transform.position.y + halfY < camWorldSize.y && transform.position.y - halfY > -camWorldSize.y) {
                canSpawnY = true;
            }
        }
    }

   
    void CheckScreenEdgesWithSinglePlayer() {
        float halfX = spriteSize.x / 2;
        float halfY = spriteSize.y / 2;

        //checks if the edge of the player has reached the border, and moves it to the other side of the screen
        if(transform.position.x - halfX > camWorldSize.x && movementVector == Vector3.right) {
            Vector3 newSpawnVector = new Vector3(-camWorldSize.x - halfX, transform.position.y, transform.position.z);
            transform.position = newSpawnVector;
        } else if(transform.position.x + halfX < -camWorldSize.x && movementVector == Vector3.left) {
            Vector3 newSpawnVector = new Vector3(camWorldSize.x + halfX, transform.position.y, transform.position.z);
            transform.position = newSpawnVector;
        } else if (transform.position.y - halfY > camWorldSize.y && movementVector == Vector3.up) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, -camWorldSize.y - halfY, transform.position.z);
            transform.position = newSpawnVector;
        } else if (transform.position.y + halfY < -camWorldSize.y && movementVector == Vector3.down) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, camWorldSize.y + halfY, transform.position.z);
            transform.position = newSpawnVector;
        }
    } 

    void MovePlayer() {
        if(Input.GetKeyDown(KeyCode.W)) {
            this.GetComponent<SpriteRenderer>().sprite = upSprite;
            movementVector = Vector3.up;
        } else if(Input.GetKeyDown(KeyCode.A)) {
            this.GetComponent<SpriteRenderer>().sprite = leftSprite;
            movementVector = Vector3.left;
        } else if(Input.GetKeyDown(KeyCode.S)) {
            this.GetComponent<SpriteRenderer>().sprite = downSprite;
            movementVector = Vector3.down;
        } else if(Input.GetKeyDown(KeyCode.D)) {
            this.GetComponent<SpriteRenderer>().sprite = rightSprite;
            movementVector = Vector3.right;
        } 
        transform.Translate(movementVector * moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerCounts();
        CheckScreenEdgesWithSinglePlayer();

        MovePlayer();

        transform.Translate(movementVector * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "World") {
            HitWorld(other.gameObject);
        } else if (other.gameObject.tag == "Bullet") {
            sfxManager.HitPlayer();
            GameOver();
        }
    }

    void HitWorld(GameObject world) {
        scoreManager.HitWorld();
        spawnManager.SpawnBullet(transform.position, movementVector);
        spawnManager.SpawnWorld();
        sfxManager.HitWorld();
        Destroy(world);
    }

    void GameOver() {
        gameManager.EndGame();
        Destroy(this.gameObject);
    }
}
