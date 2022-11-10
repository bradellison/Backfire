using System.Linq.Expressions;
using ScriptableObjects;
using UnityEngine;

public class Forcefield : MonoBehaviour
{
    private PlayerController _playerController;
    private GameObject _player;
    private Transform _transform;
    private Vector3 _position;
    public float forcefieldBulletHitCounterDecrease;
    public float forcefieldSpinnerHitCounterDecrease;
    private bool _willAllowDecreaseOnHit;

    [SerializeField] private OnWorldHitScriptableObject onWorldHitScriptableObject;

    private void Start()
    {
        _transform = transform;
        _player = _transform.parent.gameObject;
        _playerController = _player.GetComponent<PlayerController>();
        Physics2D.IgnoreCollision(_playerController.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        _willAllowDecreaseOnHit = false;
        Invoke(nameof(AllowDecrease), 0.1f);
    }

    private void AllowDecrease()
    {
        _willAllowDecreaseOnHit = true;
    }
    
    private void Update()
    {
        _transform.position = _player.transform.position;
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("World")) {
            HitWorld(other.gameObject);
        } else if (other.gameObject.CompareTag("Bullet")) {
            ForcefieldHitBullet(other.gameObject);
        } else if (other.gameObject.CompareTag("Spinner")) {
            ForcefieldHitSpinner(other.gameObject);
        }
    }

    private void ForcefieldHitBullet(GameObject bullet) {
        if (_willAllowDecreaseOnHit)
        {
            _playerController.DecreaseForceFieldCounter(forcefieldBulletHitCounterDecrease);
        }
        Destroy(bullet);
    }
    
    private void ForcefieldHitSpinner(GameObject spinner) {
        if (_willAllowDecreaseOnHit)
        {
            _playerController.DecreaseForceFieldCounter(forcefieldSpinnerHitCounterDecrease);
        }
        Destroy(spinner);
    }

    private void HitWorld(GameObject world)
    {
        Debug.Log("hit world with forcefield");
        onWorldHitScriptableObject.onWorldHitEvent.Invoke();
        Destroy(world);
    }
}
