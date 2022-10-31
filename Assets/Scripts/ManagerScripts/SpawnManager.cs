using System;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ManagerScripts
{
    public class SpawnManager : MonoBehaviour
    {
        private Vector2 _camWorldSize;
        private Vector2 _spawnWorldSize;

        public GameObject playerPrefab;
        public GameObject worldPrefab;
        public GameObject bulletPrefab;
        public GameObject laserEmitterPrefab;

        public GameObject playerParent;
        public GameObject worldParent;
        public GameObject bulletParent;
        public GameObject laserEmitterParent;

        private GameManager _gameManager;

        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;

        private void OnEnable()
        {
            onGameStartScriptableObject.onGameStartEvent.AddListener(GameStart);
        }

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _camWorldSize = _gameManager.camWorldSize;
            _spawnWorldSize = _camWorldSize * 0.9f;
        }

        private void GameStart()
        {
            DestroyAll();
            SpawnPlayer();
            SpawnWorld();
            SpawnLaserEmitters();
        }

        private PlayerController SpawnPlayer() {
            GameObject playerGo = Instantiate(playerPrefab, playerParent.transform, true);
            playerGo.transform.position = Vector3.zero;
            return playerGo.GetComponent<PlayerController>();
        }

        public void SpawnWorld() {
            GameObject worldGo = Instantiate(worldPrefab, worldParent.transform, true);
            worldGo.transform.position = RandomOnScreenVector(true);
        }

        private void SpawnLaserEmitters()
        {
            for (int i = 0; i < _gameManager.levelManager.level - 1; i++)
            {
                SpawnLaserEmitter(i);
            }
        }
        
        private void SpawnLaserEmitter(int cornerSpawnIndex)
        {
            GameObject laserEmitterGo = Instantiate(laserEmitterPrefab, laserEmitterParent.transform, true);
            //laserEmitterGo.transform.position = spawnLocation;
            LaserEmitter laserEmitter = laserEmitterGo.GetComponent<LaserEmitter>();
            laserEmitter.SetStartLocation(cornerSpawnIndex);
            laserEmitter.movementVector = Vector3.up;
        }
        
        public void SpawnBullet(Vector3 spawnVector, Vector3 movementVector) {
            GameObject bulletGo = Instantiate(bulletPrefab, bulletParent.transform, true);
            bulletGo.transform.position = spawnVector;
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            bullet.movementVector = -movementVector;
            bullet.startingMovementDirection = -movementVector;
            bullet.camWorldSize = _camWorldSize;
            bullet.moveSpeed *= 1 + ((_gameManager.levelManager.level - 1) * 0.2f);
            if (Random.value >= 0.5) {
                bullet.amplitude = 0.04f + (_gameManager.levelManager.level * 0.02f);
            } else {
                bullet.amplitude = 0.04f - (_gameManager.levelManager.level * 0.02f);
            }
        }

        private void DestroyAll() {
            foreach(Transform bullet in bulletParent.transform) {
                Destroy(bullet.gameObject);
            }
            foreach(Transform world in worldParent.transform) {
                Destroy(world.gameObject);
            }
            foreach(Transform laserEmitter in laserEmitterParent.transform) {
                Destroy(laserEmitter.gameObject);
            }
        }

        private Vector3 RandomOnScreenVector(bool avoidCenter) {
            Vector3 randomOnScreenVector = new Vector3(Random.Range(-_spawnWorldSize.x, _spawnWorldSize.x), Random.Range(-_spawnWorldSize.y, _spawnWorldSize.y), 0);
            // if avoiding centre, try again if too close to center
            if ((Mathf.Abs(randomOnScreenVector.x) < 1 || Mathf.Abs(randomOnScreenVector.y) < 1) && avoidCenter)
            {
                randomOnScreenVector = RandomOnScreenVector(true);
            }
            return randomOnScreenVector;
        }
    }
}
