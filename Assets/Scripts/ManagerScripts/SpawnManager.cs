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

        public GameObject playerParent;
        public GameObject worldParent;
        public GameObject bulletParent;

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
        }
        
        public PlayerController SpawnPlayer() {
            GameObject playerGo = Instantiate(playerPrefab, playerParent.transform, true);
            playerGo.transform.position = Vector3.zero;
            return playerGo.GetComponent<PlayerController>();
        }

        public void SpawnWorld() {
            GameObject worldGo = Instantiate(worldPrefab, worldParent.transform, true);
            worldGo.transform.position = RandomOnScreenVector();
        }

        public void SpawnBullet(Vector3 spawnVector, Vector3 movementVector) {
            GameObject bulletGo = Instantiate(bulletPrefab, bulletParent.transform, true);
            bulletGo.transform.position = spawnVector;
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            bullet.movementVector = -movementVector;
            bullet.startingMovementDirection = -movementVector;
            bullet.camWorldSize = _camWorldSize;
            bullet.moveSpeed *= _gameManager.levelManager.level;
            if (Random.value >= 0.5) {
                bullet.amplitude = (_gameManager.levelManager.level) * 0.05f;
            } else {
                bullet.amplitude = (-_gameManager.levelManager.level) * 0.05f;
            }
        }

        public void DestroyAll() {
            foreach(Transform bullet in bulletParent.transform) {
                Destroy(bullet.gameObject);
            }
            foreach(Transform world in worldParent.transform) {
                Destroy(world.gameObject);
            }
        }

        private Vector3 RandomOnScreenVector() {
            Vector3 randomOnScreenVector = new Vector3(Random.Range(-_spawnWorldSize.x, _spawnWorldSize.x), Random.Range(-_spawnWorldSize.y, _spawnWorldSize.y), 0);
            return randomOnScreenVector;
        }
    }
}
