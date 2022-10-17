using UnityEngine;

public class World : MonoBehaviour
{

    //public Gradient[] colorings;
    public GameObject worldDestroyParticles;
    private bool _isQuitting;

    private void OnApplicationQuit() {
        _isQuitting = true;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Bullet")) {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }
    private void OnDestroy() {
        if(!_isQuitting) {
            GameObject particles = Instantiate(worldDestroyParticles);
            particles.transform.position = this.transform.position;
        }
    }
}
