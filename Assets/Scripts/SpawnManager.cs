using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Vector2 camWorldSize;
    Vector2 spawnWorldSize;

    public GameObject playerPrefab;
    public GameObject worldPrefab;
    public GameObject bulletPrefab;

    public GameObject playerParent;
    public GameObject worldParent;
    public GameObject bulletParent;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        camWorldSize.y = Camera.main.orthographicSize;
        camWorldSize.x = Camera.main.orthographicSize * Camera.main.aspect;
        spawnWorldSize.y = camWorldSize.y * 0.9f;
        spawnWorldSize.x = camWorldSize.x * 0.9f;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void SpawnPlayer() {
        GameObject playerGO = Instantiate(playerPrefab);
        playerGO.transform.parent = playerParent.transform;
        playerGO.transform.position = Vector3.zero;
    }

    public void SpawnWorld() {
        GameObject worldGO = Instantiate(worldPrefab);
        worldGO.transform.parent = worldParent.transform;
        worldGO.transform.position = RandomOnScreenVector();
    }

    public void SpawnBullet(Vector3 spawnVector, Vector3 movementVector) {
        GameObject bulletGO = Instantiate(bulletPrefab);
        bulletGO.transform.parent = bulletParent.transform;
        bulletGO.transform.position = spawnVector;
        bulletGO.GetComponent<Bullet>().movementVector = -movementVector;
        bulletGO.GetComponent<Bullet>().camWorldSize = camWorldSize;
        bulletGO.GetComponent<Bullet>().moveSpeed *= levelManager.level;
    }

    public void DestroyAll() {
        foreach(Transform bullet in bulletParent.transform) {
            Destroy(bullet.gameObject);
        }
        foreach(Transform world in worldParent.transform) {
            Destroy(world.gameObject);
        }
    }

    Vector3 RandomOnScreenVector() {
        Vector3 randomOnScreenVector = new Vector3(Random.Range(-spawnWorldSize.x, spawnWorldSize.x), Random.Range(-spawnWorldSize.y, spawnWorldSize.y), 0);
        return randomOnScreenVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
