using UnityEngine;

public class Forcefield : MonoBehaviour
{
    private PlayerController _playerController;
    private GameObject _player;
    private Transform _transform;
    private Vector3 _position;
    public float forcefieldBulletHitCounterDecrease;

    private void Start()
    {
        _transform = transform;
        _player = _transform.parent.gameObject;
        _playerController = _player.GetComponent<PlayerController>();
        Physics2D.IgnoreCollision(_playerController.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
    }

    private void Update()
    {
        _transform.position = _player.transform.position;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("World")) {
            _playerController.HitWorld(other.gameObject, true);
        } else if (other.gameObject.CompareTag("Bullet")) {
            HitBullet(other.gameObject);
        }
    }

    public void HitBullet(GameObject bullet) {
        _playerController.DecreaseForceFieldCounter(forcefieldBulletHitCounterDecrease);
        Destroy(bullet);
    }

}
