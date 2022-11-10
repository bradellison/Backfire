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
        public GameObject spinnerPrefab;

        public GameObject playerParent;
        public GameObject worldParent;
        public GameObject bulletParent;
        public GameObject laserEmitterParent;
        public GameObject spinnerParent;

        private PlayerController _playerController;
        private GameManager _gameManager;

        [SerializeField] private OnGameStartScriptableObject onGameStartScriptableObject;
        [SerializeField] private OnWorldHitScriptableObject onWorldHitScriptableObject;

        private void OnEnable()
        {
            onGameStartScriptableObject.onGameStartEvent.AddListener(GameStart);
            onWorldHitScriptableObject.onWorldHitEvent.AddListener(WorldHit);
        }

        private void OnDisable()
        {
            onGameStartScriptableObject.onGameStartEvent.RemoveListener(GameStart);
            onWorldHitScriptableObject.onWorldHitEvent.RemoveListener(WorldHit);        }

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

        private void SpawnPlayer() {
            GameObject playerGo = Instantiate(playerPrefab, playerParent.transform, true);
            playerGo.transform.position = Vector3.zero;
            _playerController = playerGo.GetComponent<PlayerController>();
        }

        private void WorldHit()
        {
            SpawnWorld();
            Vector3 spawnVector = _playerController.transform.position;
            Vector3 movementVector = _playerController.movementVector;
            //SpawnBullet(spawnVector, movementVector);

            switch (_gameManager.levelManager.level)
            {
                case 1:
                    SpawnBullet(spawnVector, movementVector);
                    break;
                case 2:
                    SpawnBullet(spawnVector, movementVector);
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        SpawnSpinner(spawnVector, movementVector);
                    }

                    break;
                case 3:
                    SpawnBullet(spawnVector, movementVector);
                    if (Random.Range(0f, 1f) > 0.0f)
                    {
                        SpawnSpinner(spawnVector, movementVector);
                    }

                    break;
                case 4:
                    SpawnBullet(spawnVector, movementVector);
                    break;
                case 5:
                    SpawnBullet(spawnVector, movementVector);
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        SpawnSpinner(spawnVector, movementVector);
                    }

                    break;
                default:
                    SpawnBullet(spawnVector, movementVector);
                    break;
            }
        }


        private void SpawnWorld() {
            GameObject worldGo = Instantiate(worldPrefab, worldParent.transform, true);
            worldGo.transform.position = RandomOnScreenVector(true);
        }

        private void SpawnLaserEmitters()
        {
            //for (int i = 0; i < _gameManager.levelManager.level - 1; i++)
            //{
            //    SpawnLaserEmitter(i);
            //}
            if (_gameManager.levelManager.level >= 4)
            {
                SpawnLaserEmitter(Random.Range(1,4));
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

        public void SpawnSpinner(Vector3 spawnVector, Vector3 movementVector)
        {
            GameObject spinnerGo = Instantiate(spinnerPrefab, spinnerParent.transform, true);
            spinnerGo.transform.position = spawnVector;
            Spinner spinner = spinnerGo.GetComponent<Spinner>();
            spinner.movementVector = -movementVector;
            spinner.camWorldSize = _camWorldSize;
            spinner.moveSpeed *= 1 + ((_gameManager.levelManager.level - 1) * 0.2f);
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
            foreach(Transform spinner in spinnerParent.transform) {
                Destroy(spinner.gameObject);
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
